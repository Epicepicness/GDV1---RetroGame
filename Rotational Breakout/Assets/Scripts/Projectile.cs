using UnityEngine;

public class Projectile : MonoBehaviour {

	// Projectile script is added to bullets, and checks for enemy/destructable collision and moves the projectile forward.

		// Verander dit uiteindelijk naar een Scriptable object?

#pragma warning disable CS0649
	[Tooltip ("Float damage is the damage the projectile will inflict when hitting something that can be damaged.")]
	[SerializeField] private int damage = 1;
	[Tooltip ("Float damage is the damage the projectile will inflict when hitting something that can be damaged.")]
	[SerializeField] private float speed = 50;                                       //the movement speed the projectile has.

	private float skinWidth = .1f;

	[Tooltip ("LayerMask collisionMask are all the things the projectile will collide with.")]
	[SerializeField] private LayerMask collisionMask;

	[Tooltip ("AudioClip impactSound is the audio clip that will be played when the projectile hits Terrain.")]
	[SerializeField] private AudioClip impactSoundTerrain;
#pragma warning restore CS0649


	//Update moves the projectile as well as calls for collision checks.
	private void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);

		transform.Translate (Vector2.right * moveDistance);
	}


	//SetupProjectile is called when the projectile is spawned. It checks if the projectile is spawned inside of another collider, as well as sets the range, speed and duration.
	public void SetupProjectile () {
		Collider2D initialCollisions = Physics2D.OverlapCircle (transform.position, .1f, collisionMask);
		if (initialCollisions) {
			OnHitObject (initialCollisions, transform.position);
		}

		Destroy (this.gameObject, 10f);	// Magic number; adjust later
	}

	//CheckCollisions uses a raycast to check if it's about to collide with something.
	private void CheckCollisions (float moveDistance) {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, transform.right, moveDistance + skinWidth, collisionMask);
		Debug.DrawRay (transform.position, transform.right * (moveDistance + 1), Color.yellow);

		if (hit) {
			OnHitObject (hit.collider, hit.point);
		}
	}

	//OnHitObject checks if the object the projectile collided with can be damaged and if so calls for it to be damaged, then destroys the projectile.
	private void OnHitObject (Collider2D other, Vector2 hitPoint) {
		if (impactSoundTerrain != null) {
			SoundManager.PlaySoundEffect (impactSoundTerrain, transform.position);
		}
		IDamageable damageableObject = other.GetComponent<IDamageable> ();
		if (damageableObject != null && other.tag != "ScoreBlock") {
			damageableObject.TakeHit (damage, hitPoint);
		}

		Destroy (this.gameObject);
	}

}
