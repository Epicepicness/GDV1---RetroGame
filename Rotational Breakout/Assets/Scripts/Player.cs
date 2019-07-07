using UnityEngine;

public class Player : MonoBehaviour {

	// Player takes in player input from the InputManager, and handles the logic for the player's paddle.

#pragma warning disable CS0649
	[Header ("Movement Related Variables")]
	[Tooltip ("float maximumSpeed is the maximum movement speed for the player's bar.")]
	[SerializeField] private float maximumSpeed;
	[Tooltip ("float timeToAccelerate is the time in seconds to reach maximumSpeed.")]
	[SerializeField] private float timeToAccelerate;
	[Tooltip ("float timeToDeccelerate is the time in seconds to stand still from maximumSpeed.")]
	[SerializeField] private float timeToDeccelerate;
	private float accelerationSpeed;						// Stores the increase in speed per second, calculated based on the maximumSpeed and the timeToAccelerate.
	private float deccelerationSpeed;						// Stores the decrease in speed per second, calculated based on the maximumSpeed and the timeToDeccelerate.
	private Vector2 currentSpeed;                           // Stores the current speed the player is moving in.

	[Header ("Collision Related Variables")]
	[Tooltip ("LayerMask collisionMask are the layers the player's bar can collide with.")]
	[SerializeField] private LayerMask collisionMask;

	[SerializeField] private GameObject ballBeingHeld;
	[SerializeField] private Transform centerObject;

	private InputManager inputManager;
	private ControleMode controlMode;
#pragma warning restore CS0649

	private enum ControleMode {
		horizontal,
		rotational
	};


	private void Start () {
		inputManager = GameManager.instance.inputManager;

		controlMode = ControleMode.rotational;

		accelerationSpeed = Mathf.Abs (maximumSpeed / timeToAccelerate);
		deccelerationSpeed = Mathf.Abs (maximumSpeed / timeToDeccelerate);
	}

	private void Update () {
		if (GameManager.instance.gameIsPauzed) return;

		if (controlMode == ControleMode.horizontal) {
			MoveHorizontal ();
		} else {
			MoveRotational ();
		}

		if (ballBeingHeld != null && InputManager.Current.ActivateKey) {
			LaunchBall ();
		}
	}


	public void LaunchBall () {
		if (ballBeingHeld == null) return;      // Overbodig; but better safe than sorry
		Ball b = ballBeingHeld.GetComponent<Ball> ();
		BallNoPhysx bNP = ballBeingHeld.GetComponent<BallNoPhysx> ();
		if (b == null && bNP == null) {
			Debug.LogError ("Ball being launched does not have Ball component.");
			return;
		}

		Vector2 directionToLaunchAt = (this.transform.position - centerObject.transform.position).normalized;
		if (b != null) b.Launch (directionToLaunchAt);
		else bNP.LaunchBall (directionToLaunchAt);

		ballBeingHeld.transform.SetParent (null);
		ballBeingHeld = null;
	}

	// Called from the Ball script to get the movement/angle of the player to add to the angle of the ball on impact (speed remains same).
	public Vector2 VelocityTransfer () {
		if (currentSpeed == Vector2.zero) //return Vector2.zero;
			return Vector2.zero;

		float movementAngle = this.transform.rotation.eulerAngles.z + (90 * Mathf.Sign (currentSpeed.x));
		Vector2 movementVector = new Vector2 (Mathf.Cos (movementAngle * Mathf.Deg2Rad), Mathf.Sin (movementAngle * Mathf.Deg2Rad));
		movementVector *= currentSpeed;

		return movementVector;
	}


	private void MoveRotational () {
		float horizontalInput = InputManager.Current.DirectionalInput.x;

		if (horizontalInput != 0) {
			if (Mathf.Abs (currentSpeed.x) < maximumSpeed) {
						//Increase until maximumSpeed
				currentSpeed.x += (accelerationSpeed * horizontalInput) * Time.deltaTime;
			}
			else {		//Movement at maximumSpeed
				currentSpeed.x = maximumSpeed * horizontalInput;
			}
		}
		else {
			if (Mathf.Abs (currentSpeed.x) - deccelerationSpeed * 2 * Time.deltaTime > 0) {
						//Decrease in speed until it stands still
				currentSpeed.x -= deccelerationSpeed * Mathf.Sign (currentSpeed.x) * Time.deltaTime;
			}
			else {		//No Movement
				currentSpeed.x = 0;
				return;
			}
		}

		transform.RotateAround (centerObject.position, Vector3.forward, -currentSpeed.x * Time.deltaTime);
	}

	//The function that handles Horizontal movement of the pallet.
	private void MoveHorizontal () {
		float horizontalInput = InputManager.Current.DirectionalInput.x;

		if (InputManager.Current.DirectionalInput.x != 0) {
			if (Mathf.Abs (currentSpeed.x) < maximumSpeed) {
				//Increase until maximumSpeed
				currentSpeed.x += (accelerationSpeed * horizontalInput) * Time.deltaTime;
			} else {	//Movement at maximumSpeed
				currentSpeed.x = maximumSpeed * horizontalInput;
			}
		} else {
			if (Mathf.Abs (currentSpeed.x) - deccelerationSpeed * 2 * Time.deltaTime > 0) {
				//Decrease in speed until it stands still
				currentSpeed.x -= deccelerationSpeed * Mathf.Sign (currentSpeed.x) * Time.deltaTime;
			} else {	//No Movement
				currentSpeed.x = 0;
				return;
			}
		}
		//Checking if a wall is hit
		float rayDistance = transform.localScale.x / 2 + currentSpeed.x * horizontalInput * Time.deltaTime;
		RaycastHit2D hit = Physics2D.Raycast (transform.position, new Vector2 (Mathf.Sign (currentSpeed.x), 0), rayDistance, collisionMask);
		float newPositionX;
		if (hit) {
			newPositionX = transform.position.x + (hit.distance - transform.localScale.x / 2) * Mathf.Sign (currentSpeed.x);
			currentSpeed.x = 0;
		} else {
			newPositionX = transform.position.x + currentSpeed.x;
		}
		//The actual movement of the paddle
		Vector2 newPosition = new Vector2 (newPositionX, transform.position.y);
		transform.position = newPosition;
	}

}
