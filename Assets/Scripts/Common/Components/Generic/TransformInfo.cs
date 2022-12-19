using UnityEngine;

public struct TransformInfo {
    public GameObject gameObject;
    public Transform transform;

    public TransformInfo(GameObject gameObject) {
	this.gameObject = gameObject;
	this.transform = gameObject.transform;
    }

    public TransformInfo(Transform transform) {
	this.gameObject = transform.gameObject;
	this.transform = transform;
    }
}
