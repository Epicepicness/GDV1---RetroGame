using UnityEngine;

public class BrickSpawner : MonoBehaviour {

#pragma warning disable CS0649
	[SerializeField] private Vector2 startingLocation = Vector2.zero;
	[SerializeField] private GameObject brick;
	[SerializeField] private Transform brickParent;

	[SerializeField] private int rows;
	[SerializeField] private int amountPerRow;
	[SerializeField] private float distanceX;
	[SerializeField] private float distanceY;
#pragma warning restore CS0649

	private void Start () {
		SpawnBricksLiniar ();
	}

	private void SpawnBricksLiniar() {
		Vector2 spawnPosition = startingLocation;
		for (int r = 0; r <= rows; r++) {
			//For each row:

			for (int i = 0; i <= amountPerRow; i++) {
				Instantiate (brick, spawnPosition, Quaternion.identity, brickParent);
				spawnPosition.y -= distanceY;
			}
		
			spawnPosition.y = startingLocation.y;
			spawnPosition.x += distanceX;
		}
	}

	private void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(startingLocation - Vector2.up, startingLocation + Vector2.up);
		Gizmos.DrawLine(startingLocation - Vector2.left, startingLocation + Vector2.left);
	}

}
