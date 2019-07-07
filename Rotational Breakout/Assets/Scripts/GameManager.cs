using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Utilities;

public class GameManager : MonoBehaviour {

	// GameManager oversees the general state of the game, as well as making sure all other components (managers) exist and are set up correctly.
	// GameManager manages global pause state; loading new scenes/levels; setting up and linking other managers.

	//Managers
	private static GameManager _instance = null;					//Static instance of GameManager which allows it to be accessed by any other script.
	public static GameManager instance { get { return _instance; } }

	public bool gameIsPauzed { get; private set; }					// "Pausing" in this case means "is gameplay going on"

#pragma warning disable CS0649
	// Manager references
	[HideInInspector] public InputManager inputManager;				// Reference to the InputManager
	[HideInInspector] public LevelManager levelManager;             // Reference to the LevelManager
	[HideInInspector] public UIManager uIManager;                   // Reference to the UIManager
	[HideInInspector] public SoundManager soundManager;                   // Reference to the UIManager

	// Scene Selection Variables
	[SerializeField] private LevelData mainMenu;                   // The Main Menu Scene
	[SerializeField] private List<LevelData> levelDataList;			// List of StoryChapter Scriptable objects; containing the scenes for a specific chapter.

	private const int maxLevels = 3;								//Saves the total amount of scenes in the game.
	[HideInInspector] private int currentLevel = 0;                 //Saves the number for the current level.
#pragma warning restore CS0649


	private void Awake () {
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);

		inputManager = this.gameObject.GetComponent<InputManager> ();
		levelManager = this.gameObject.GetComponent<LevelManager> ();

		uIManager = (GameObject.Find ("UIManager")) ? GameObject.Find ("UIManager").GetComponent<UIManager> () :
			((GameObject) Instantiate (Resources.Load ("Managers/UIManager"), Vector3.zero, Quaternion.identity)).GetComponent<UIManager> ();
		soundManager= (GameObject.Find ("SoundManager")) ? GameObject.Find ("SoundManager").GetComponent<SoundManager> () :
			((GameObject) Instantiate (Resources.Load ("Managers/SoundManager"), Vector3.zero, Quaternion.identity)).GetComponent<SoundManager> ();
	}

	private void Start () {
		PauzeGame ();
		SceneManager.sceneLoaded += OnSceneChanged;

		// Purely for testing purposes; making sure all the scene setup stuff is done when loading a non-Main Menu scene initually.
		string sceneName = mainMenu.scene.SceneName.Substring (mainMenu.scene.SceneName.LastIndexOf ('/') + 1);
		if (SceneManager.GetActiveScene().name != sceneName) {
			SetupSceneComponents ();
		} else {
			SetupMainMenuComponents ();
		}
	}

	public void PauzeGame () {
		gameIsPauzed = true;
	}
	public void UnpauzeGame () {
		gameIsPauzed = false;
	}

	public void OnLevelVictory () {
		PauzeGame ();
		uIManager.ShowVictoryScreen ();
	}
	public void OnGameOver () {
		PauzeGame ();
		uIManager.ShowGameOverMenu ();
	}

	//--- Scene Load Functions ---------------------------------------------------------------------------------------------------
	public void LoadMainMenu () {
		currentLevel = 0;
		UIManager.instance.HideEverything ();
		soundManager.PlayBackgroundMusic (mainMenu.backgroundSong);
		LoadScene (mainMenu.scene);
	}

	public void StartFirstLevel () {
		currentLevel = 0;
		LoadScene (levelDataList [currentLevel].scene);
	}

	public void RestartLevel () {
		LoadScene (levelDataList [currentLevel].scene);
	}

	public void LoadNextLevel () {
		if (currentLevel + 1 >= maxLevels) {
			LoadScene (mainMenu.scene);
			return;
		}

		currentLevel++;
		LoadScene (levelDataList [currentLevel].scene);
	}

	private void LoadScene (SceneField scene) {
		SceneManager.LoadScene (scene);
	}

	private void OnSceneChanged (Scene current, LoadSceneMode mode) {
		string sceneName = mainMenu.scene.SceneName.Substring (mainMenu.scene.SceneName.LastIndexOf ('/') + 1);
		if (SceneManager.GetActiveScene ().name != sceneName) {
			SetupSceneComponents ();
		}
		else {
			SetupMainMenuComponents ();
		}
	}

	// SetupSceneComponents sets up all the Managers, and unpauzes the game for gameplay.
	private void SetupSceneComponents () {
		soundManager = (GameObject.Find ("SoundManager")) ? GameObject.Find ("SoundManager").GetComponent<SoundManager> () :
			((GameObject) Instantiate (Resources.Load ("Managers/SoundManager"), Vector3.zero, Quaternion.identity)).GetComponent<SoundManager> ();

		levelManager.NewSceneSetup (levelDataList [currentLevel].enemyMovementSpeed, levelDataList [currentLevel].enemyMovementTime, levelDataList [currentLevel].enemyFireRate);
		uIManager.NewSceneSetup ();
		Debug.Log ("Song: " + levelDataList [currentLevel].backgroundSong + ", in: " + levelDataList [currentLevel].scene);
		soundManager.PlayBackgroundMusic (levelDataList [currentLevel].backgroundSong);

		UnpauzeGame ();
	}
	private void SetupMainMenuComponents () {
		soundManager = (GameObject.Find ("SoundManager")) ? GameObject.Find ("SoundManager").GetComponent<SoundManager> () :
			((GameObject) Instantiate (Resources.Load ("Managers/SoundManager"), Vector3.zero, Quaternion.identity)).GetComponent<SoundManager> ();

		soundManager.PlayBackgroundMusic (mainMenu.backgroundSong);
	}

}
