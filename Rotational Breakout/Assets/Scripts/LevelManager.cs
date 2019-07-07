using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	// LevelManager orders enemy units; tracks a levels state, as well as victory/loss conditions of a level.

	// Level Variables
	[HideInInspector] public int score;
	[HideInInspector] public Transform centerObject;

	// Enemy Unit variables
	[SerializeField] public float enemyMovementSpeed = 1f;

	private List<Enemy> enemyList = new List<Enemy> ();
	private bool enemiesMovingRight;
	private float movementDirectionTime = 2f;
	private float internalDirectionTimer = 0;
	private float enemyFireRate = 5f;
	private float internalFireRateTimer = 0;

	private Ball ball;


	private void Update () {
		if (GameManager.instance.gameIsPauzed) return;

		// Check if enemies have to move into a different direction
		internalDirectionTimer += Time.deltaTime;
		if (internalDirectionTimer > movementDirectionTime) {
			internalDirectionTimer = 0;
			enemiesMovingRight = !enemiesMovingRight;
		}

		// Move enemies
		for (int i = 0; i < enemyList.Count; i++) {
			if (enemyList [i] != null)
				enemyList [i].Move (enemiesMovingRight);
		}
		foreach (Enemy e in enemyList) {
			if (e != null) {
				e.Move (enemiesMovingRight);
			}
		}
		
		// Timer to decide which random enemy should attack
		internalFireRateTimer += Time.deltaTime;
		if (internalFireRateTimer > enemyFireRate) {
			internalFireRateTimer = 0;

			Enemy attackingEnemy = enemyList [Random.Range (0, enemyList.Count)];
			if (attackingEnemy != null) {
				attackingEnemy.Attack ();
			}
		}
	}


	public void NewSceneSetup (float enemySpeed, float enemyTime, float enemyFireRate) {
		this.enemyMovementSpeed = enemySpeed;
		this.movementDirectionTime = enemyTime;
		this.internalDirectionTimer = movementDirectionTime / 2;
		this.enemyFireRate = enemyFireRate;

		centerObject = GameObject.Find ("CenterPlanet").transform;  // Vind een betere manier om dit te doen
		ball = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Ball>();

		// Get a list of all enemies in the level, and gives those enemies a reference to this LevelManager
		enemyList.Clear ();
		GameObject [] enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");
		Enemy enemyToAdd;
		for (int i = 0; i < enemyArray.Length; i++) {
			enemyToAdd = enemyArray [i].GetComponent<Enemy> ();
			enemyList.Add (enemyToAdd);
			enemyToAdd.SetupEnemy (this);
		}
	}

	public void AddScore (int score) {
		this.score += score;
		UIManager.instance.AdjustScore (this.score);
	}

	// Once an enemy died, it's removed from the Enemies list, and if the list reaches 0 there will appear a score screen. 
	public void IDied (Enemy diedEnemy) {
		enemyList.Remove (diedEnemy);

		if (enemyList.Count == 0) {
			ball.StopBall ();
			GameManager.instance.OnLevelVictory ();
		}
	}

}
