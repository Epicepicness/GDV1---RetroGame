using UnityEngine;

public class InputManager : MonoBehaviour {

	// InputManager gets and holds all the player input in a static struct.

	public static PlayerInput Current;

	private void Update () {
		Vector2 directionalInput = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		bool activateKey = Input.GetButtonDown ("Submit");
		bool menuKey = Input.GetButtonDown ("Menu");

		Current = new PlayerInput () {
			DirectionalInput = directionalInput,
			ActivateKey = activateKey,
			MenuKey = menuKey
		};
	}

}

public struct PlayerInput {
	public Vector2 DirectionalInput;
	public bool ActivateKey;
	public bool MenuKey;
}
