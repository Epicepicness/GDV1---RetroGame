using UnityEngine;

public class Enemy : HasHealth, IDamageable {

	// Enemy handles an enemy's individual logic (such as movement, taking damage, etc). 
	// An individual enemy is called upon from the LevelManager script.

#pragma warning disable CS0649
	[SerializeField] private GameObject projectile;
	[SerializeField] private AudioClip fireSoundEffect;
#pragma warning restore CS0649

	// Movement Variables
	private LevelManager levelManager;


	public void SetupEnemy (LevelManager levelManager) {
		currentHealth = maximumHealth;
		this.levelManager = levelManager;
	}

	public void Move (bool moveRight) {
		transform.RotateAround (levelManager.centerObject.position, Vector3.forward, ((moveRight) ? 1 : -1) * levelManager.enemyMovementSpeed * Time.deltaTime);
	}

	public void Attack () {
		GameObject newProjectile = Instantiate (projectile, this.transform.position, this.transform.rotation * Quaternion.Euler (0, 0, 270));
		SoundManager.PlaySoundEffect (fireSoundEffect, this.transform.position);
		newProjectile.GetComponent<Projectile>().SetupProjectile ();
	}

	protected override void Die () {
		base.Die ();

		levelManager.IDied (this);
		Destroy (this.gameObject, 0.1f);
	}

}
