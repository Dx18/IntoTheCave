using UnityEngine.InputSystem;

namespace Game {
    public struct InputMovementBehaviour {
	public PlayerInput input;

	public InputMovementBehaviour(PlayerInput input) {
	    this.input = input;
	}
    }
}
