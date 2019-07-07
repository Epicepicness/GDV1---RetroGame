using UnityEngine;

public class GravityObject : MonoBehaviour {

	// Placed on object; scans all objects in range, and adds a gravitational force towards this object.

	// Mogelijk voor voor 'global' pulls (zoals center planet) gewoon een Ball[] ergens bij houden, bespaard een circle cast

#pragma warning disable CS0649
	[Tooltip ("float pullRadius is the radius of the gravitational pull around this object.")]
	[SerializeField] private float pullRadius;
	[Tooltip ("float pullStrength is the amount of force applied to objects in the gravitational pull.")]
	[SerializeField] private float pullStrength;
	[Tooltip ("float minimumRadius is the radius in which the pull doesn't apply anymore.")]
	[SerializeField] private float minimumRadius;
	[Tooltip ("float distanceFalloff is the percentage of pullStrength that is influenced by the distance between the two objects.")]
	[SerializeField] private float distanceFalloff;
#pragma warning restore CS0649

	private void FixedUpdate () {
		Collider2D [] objectsToPull = Physics2D.OverlapCircleAll (this.transform.position, pullRadius);

		foreach (Collider2D collider in objectsToPull) {
			Ball ball = collider.GetComponent<Ball> ();
			if (ball == null) continue;

			Vector2 direction = this.transform.position - collider.transform.position;
			if (direction.magnitude < minimumRadius) continue;

			//float distance = direction.sqrMagnitude * distanceMultiplyer + 1;
			//						Base Strength (pull at maximum distance) + leftover strength * distance percentage
			float adjustedStrength = ((1 - distanceFalloff) * pullStrength) + distanceFalloff * pullStrength * (1 / (minimumRadius / direction.magnitude));

			ball.GravityPull (direction, adjustedStrength * Time.fixedDeltaTime);
			//Rigidbody.AddForce (direction.normalized * (pullStrength / distance) * rb.mass * Time.fixedDeltaTime);
		}
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (this.transform.position, pullRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (this.transform.position, minimumRadius);
	}

}
