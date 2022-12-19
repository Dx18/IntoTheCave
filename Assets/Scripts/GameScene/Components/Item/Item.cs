using UnityEngine;

namespace Game {
    public struct Item {
	public string id;
	public string name;
	public Sprite sprite;

	public Item(string id, string name, Sprite sprite) {
	    this.id = id;
	    this.name = name;
	    this.sprite = sprite;
	}
    }
}
