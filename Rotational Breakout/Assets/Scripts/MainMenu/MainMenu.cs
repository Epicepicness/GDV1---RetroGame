using UnityEngine;

public class MainMenu : MonoBehaviour {

	// Class MainMenu handles all the MainMenu UI calls; as well as some of in-menu effects (NYI).

	// Button Functions
	public void StartGameButton () {
		GameManager.instance.StartFirstLevel ();
	}

	public void OptionsButton () {

	}
	public void EndOptionsButton () {

	}

	public void QuitButton () {
		Application.Quit ();
	}



}
