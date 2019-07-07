using UnityEngine;

public interface IDamageable {

	void TakeHit (int damage, Vector2 hitPoint);

	void TakeDamage (int damage);
}
