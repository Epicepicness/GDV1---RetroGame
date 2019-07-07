using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//Managers
	private static UIManager _instance = null;              //Static instance of UIManager which allows it to be accessed by any other script.
	public static UIManager instance { get { return _instance; } }

#pragma warning disable CS0649
	[SerializeField] private GameObject menuScreenCanvas;
	[SerializeField] private GameObject inGameScreenCanvas;
	[SerializeField] private GameObject gameOverScreenCanvas;
	[SerializeField] private GameObject victoryScreenCanvas;

	[SerializeField] private Image healthbarImage;
	[SerializeField] private Text scoreText;
	[SerializeField] private Text victoryScoreText;
#pragma warning restore CS0649


	private void Awake () {
		if (_instance == null)
			_instance = this;
		else if (_instance != this)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
	}

	private void Update () {
		if (GameManager.instance.gameIsPauzed) return;

		if (InputManager.Current.MenuKey) {
			ShowGameMenu ();
		}
	}


	public void NewSceneSetup () {
		AdjustHealthBar (100);

		gameOverScreenCanvas.SetActive (false);
		victoryScreenCanvas.SetActive (false);

		CloseGameMenu ();
		ShowGameUI ();
	}

	// Button Functions
	public void OnRetryButton () {
		GameManager.instance.RestartLevel ();
	}
	public void OnMainMenuButton () {
		GameManager.instance.LoadMainMenu ();
	}
	public void CloseMenuButton () {
		CloseGameMenu ();
	}
	public void OnVictoryButton () {
		GameManager.instance.LoadNextLevel ();
	}

	// Other Functions
	public void AdjustHealthBar (float healthPercentage) {
		healthbarImage.fillAmount = healthPercentage / 100;
	}
	public void AdjustScore (int currentScore) {
		scoreText.text = "Score: " + currentScore;
	}
	public void HideEverything () {
		inGameScreenCanvas.SetActive (false);
		menuScreenCanvas.SetActive (false);
		gameOverScreenCanvas.SetActive (false);
		victoryScreenCanvas.SetActive (false);
	}

	// Menu Functions
	public void ShowVictoryScreen () {
		GameManager.instance.PauzeGame ();

		victoryScoreText.text = scoreText.text;

		victoryScreenCanvas.SetActive (true);
		inGameScreenCanvas.SetActive (false);
		menuScreenCanvas.SetActive (false);
	}
	public void ShowGameOverMenu () {
		GameManager.instance.PauzeGame ();

		gameOverScreenCanvas.SetActive (true);
		inGameScreenCanvas.SetActive (false);
		menuScreenCanvas.SetActive (false);
	}
	private void ShowGameUI () {
		GameManager.instance.UnpauzeGame ();

		menuScreenCanvas.SetActive (false);
		inGameScreenCanvas.SetActive (true);
	}
	public void ShowGameMenu () {
		GameManager.instance.PauzeGame ();

		inGameScreenCanvas.SetActive (false);
		menuScreenCanvas.SetActive (true);
	}
	private void CloseGameMenu () {
		GameManager.instance.UnpauzeGame ();

		ShowGameUI ();
	}

}
