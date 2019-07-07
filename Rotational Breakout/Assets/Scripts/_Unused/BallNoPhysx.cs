using UnityEngine;

public class BallNoPhysx : MonoBehaviour {

	// Oude poging, eigen bounce logic ipv Unity's physics, voor wat extra controle en makkelijker opties toe te voegen.
	// Had een issue waar in die af en toe in dezelfde hoek terug stuiterde, nooit de issue kunnen vinden. 

	[Header ("Movement Related Variables")]
	[Tooltip ("float movementSpeed is the maximum movement speed for the ball.")]
	[SerializeField] private float movementSpeed = .2f;
	[Tooltip ("LayerMask collisionMask are the layers the player's bar can collide with.")]
	[SerializeField] private LayerMask collisionMask = 0;

	[SerializeField] private int damage = 1;

	private bool ballInPlay = false;
	private float ballDiameter;


	private void Awake () {
		ballDiameter = transform.localScale.x;
	}

	private void Update () {
		//CheckCollision ();
		if (ballInPlay) {
			CheckCollision ();
			Vector2 moveVector = Vector2.right * movementSpeed * Time.deltaTime;
			transform.Translate (moveVector);
		}
	}


	public void LaunchBall (Vector2 direction) {
		if (ballInPlay) return;
		ballInPlay = true;

		float zAngle = Vector2.Angle (Vector2.right, direction);
		Quaternion rotation = Quaternion.Euler (0, 0, zAngle);

		this.transform.rotation = rotation;
	}

	private void CheckCollision () {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, DegreeToVector2 (transform.rotation.eulerAngles.z), ballDiameter / 2 + movementSpeed * Time.deltaTime, collisionMask);
		Debug.DrawRay (transform.position, DegreeToVector2 (transform.rotation.eulerAngles.z) * (ballDiameter / 2 + movementSpeed), Color.yellow);

		if (hit) {
			Debug.Log ("hit");
			OnHitObject (ref hit);
		}
	}

	private void OnHitObject (ref RaycastHit2D hit) {
		//Vector2 incomingVec = hit.point - (Vector2)transform.position;
		Vector2 incomingVec = DegreeToVector2 (transform.rotation.eulerAngles.z);
		Vector2 reflectVec = Vector2.Reflect (incomingVec, hit.normal);

		float turningAngle = Vector2.Angle (incomingVec, reflectVec);

		transform.rotation = Quaternion.Euler (0, 0, turningAngle + transform.rotation.eulerAngles.z);

		IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit (damage, hit.point);
		}
	}

	private Vector2 RadianToVector2 (float r) {
		return new Vector2 (Mathf.Cos (r), Mathf.Sin (r));
	}

	private Vector2 DegreeToVector2 (float d) {
		return RadianToVector2 (d * Mathf.Deg2Rad);
	}

}
