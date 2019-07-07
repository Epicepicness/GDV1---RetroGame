using UnityEngine;

public class ScoreBrick : HasHealth, IDamageable {

	// ScoreBrick is a destructable that grants score on destruction

#pragma warning disable CS0649
	[SerializeField] private int scoreGiven;
#pragma warning restore CS0649

	protected override void Die () {
		base.Die ();

		GameManager.instance.levelManager.AddScore (scoreGiven); // Maak een betere reference; fix later

		Destroy (this.gameObject, 0.1f);
	}

}
