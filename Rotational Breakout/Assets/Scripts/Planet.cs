using UnityEngine;

public class Planet : HasHealth, IDamageable {

	// Spins an object around at a set rate (for example, to rotate planets).

#pragma warning disable CS0649
	[Tooltip ("float speed is the speed at which the object rotates.")]
	[SerializeField] private float spinSpeed = 10f;
#pragma warning disable CS0649

	private float direction = 1f;
	private float directionChangeSpeed = 2f;


	private void Update () {
		if (direction < 1f) {
			direction += Time.deltaTime / (directionChangeSpeed / 2);
		}

		transform.Rotate (Vector3.up, (spinSpeed * direction) * Time.deltaTime);
	}


	public override void TakeDamage (int damage) {
		base.TakeDamage (damage);

		UIManager.instance.AdjustHealthBar (100 / maximumHealth * currentHealth);
	}

	protected override void Die () {
		// Play explosion ofzo

		GameManager.instance.OnGameOver ();
	}
}
