using UnityEngine;

public struct PlayerData {
    public int experience;
    public string[] items;

    private PlayerData(GameRules rules) {
	experience = 0;
	items = new string[rules.inventoryCapacity];
    }

    public static PlayerData Load(GameRules rules) {
	if (PlayerPrefs.HasKey("playerData")) {
	    PlayerData result = JsonUtility.FromJson<PlayerData>(
		PlayerPrefs.GetString("playerData")
	    );

	    // Fixing number of slots

	    int expected = rules.inventoryCapacity;
	    int actual = result.items.Length;

	    if (actual != expected) {
		string[] items = new string[expected];
		for (int i = 0; i < actual && i < expected; ++i) {
		    items[i] = result.items[i];
		}
		result.items = items;
	    }

	    // Fixing empty slots

	    for (int i = 0; i < result.items.Length; ++i) {
		if (result.items[i] == string.Empty) {
		    result.items[i] = null;
		}
	    }

	    return result;
	}

	return new PlayerData(rules);
    }

    public void Save() {
	PlayerPrefs.SetString("playerData", JsonUtility.ToJson(this));
    }
}
