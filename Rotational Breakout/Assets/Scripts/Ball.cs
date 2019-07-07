using UnityEngine;

public class Ball : MonoBehaviour {

	// Ball is attatched to ball objects, and handle all ball-related logic and physics. 

	[Header ("Movement Related Variables")]
	[Tooltip ("float ballInitialVelocity is the velocity the ball is launched with when fired.")]
	[SerializeField] private float ballBaseSpeed = 3;
	[Tooltip ("int impactDamage is the 'damage' the ball does to damagable blocks.")]
	[SerializeField] private int impactDamage = 1;
	[Tooltip ("float gravityPullAngle is minimum and maximum angle the ball has to face a gravity object to be affected by the pull.")]
	[SerializeField] private float gravityPullAngle = 35;

	private bool ballInPlay = false;

	//Object Components
	private Rigidbody2D rigidBody;
	 

	private void Awake () {
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	private void Start () {
		rigidBody.Sleep ();
	}


	private void OnCollisionEnter2D (Collision2D col) {
		IDamageable damageableObject = col.transform.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeDamage (impactDamage);
		}

		// Attempt 42 to apply some of the player's movement to the ball without adjusting the balls speed.
		/*Player playerObject = col.transform.GetComponent<Player> ();
		if (playerObject != null) {
			Vector2 velocityTransferVector = playerObject.VelocityTransfer ();
			if (velocityTransferVector == Vector2.zero) return;

			Vector2 forceDirection = rigidBody.velocity.normalized;
			Vector2 newDirection = Vector2.MoveTowards (rigidBody.velocity.normalized, velocityTransferVector, 1);
			rigidBody.velocity = newDirection * ballBaseSpeed;
		}*/
	}

	public void Launch (Vector2 direction) {
		if (ballInPlay) return;

		ballInPlay = true;
		rigidBody.WakeUp ();

		rigidBody.velocity = direction * ballBaseSpeed;
	}

	// Stops the ball from continueing after victory has been achieved.
	public void StopBall () {
		rigidBody.Sleep ();
	}

	public void GravityPull (Vector2 pullingObject, float pullStrength) {
		if (!ballInPlay) return;

		if (Vector2.Angle (rigidBody.velocity.normalized, pullingObject) < gravityPullAngle) {
			Vector2 newDirection = Vector2.MoveTowards (rigidBody.velocity.normalized, pullingObject.normalized, pullStrength);
			rigidBody.velocity = newDirection * ballBaseSpeed;
		}
	}

}
