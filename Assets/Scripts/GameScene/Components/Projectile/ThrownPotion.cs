using UnityEngine;

namespace Game {
    public struct ThrownPotion {
	public Vector2 targetPosition;

	public ThrownPotion(Vector2 targetPosition) {
	    this.targetPosition = targetPosition;
	}
    }
}
