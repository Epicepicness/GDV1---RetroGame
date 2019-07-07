using UnityEngine;
using Utilities;

[CreateAssetMenu (fileName = "NewLevel", menuName = "Data/LevelData", order = 1)]
public class LevelData : ScriptableObject {

	// LevelData stores all level-specific data. 

	[Header ("Overal Scene Data")]
	public SceneField scene;
	public AudioClip backgroundSong;

	[Header ("Enemy data")]
	public float enemyMovementSpeed;
	public float enemyMovementTime;
	public float enemyFireRate;

}
