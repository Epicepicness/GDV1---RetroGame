using UnityEngine;

public class HasHealth : MonoBehaviour, IDamageable {

	//Class HasHealth manages health on object, damaging the object, as well as destroying the object when health reaches 0.
	//This class is implemented by the Player and Enemy classes.

	[Tooltip ("Float startHealth is the total starting health of the creature or object.")]
	[SerializeField] protected float maximumHealth;

	protected float currentHealth;                          //Stores the current health of the object.
	protected bool isDead;                                  //Stores if the object is currently 'dead'. 


	protected virtual void Start () {
		currentHealth = maximumHealth;
	}


	//TakeHit does damage when hit by another object (such as a bullet). - Overridden in other classes to add effects.
	public virtual void TakeHit (int damage, Vector2 hitPoint) {
		TakeDamage (damage);
	}

	//TakeDamage does damage when hit by any source. Called from TakeHit, and by damage sources that don't hit a specific point or direction.
	public virtual void TakeDamage (int damage) {
		currentHealth -= damage;

		if (currentHealth <= 0 && !isDead) {
			Die ();
		}
	}

	//Die is called when an object's health reaches 0 and destroys the gameobject.
	protected virtual void Die () {
		isDead = true;
	}

}
