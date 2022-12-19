using UnityEngine;
using UnityEngine.SceneManagement;
using Leopotam.EcsLite;

namespace Game {
    public sealed class GameFinishSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    var positionPool = world.GetPool<Position>();
	    var exitPool = world.GetPool<ExitZone>();

	    int playersFound = 0;
	    int? playerReachedExit = null;

	    var players = world.Filter<Player>().End();
	    foreach (int playerEntity in players) {
		Vector2 position = positionPool
		    .Get(playerEntity).position;

		++playersFound;

		var exits = world.Filter<ExitZone>().End();
		foreach (int exitEntity in exits) {
		    Rect rectangle = exitPool
			.Get(exitEntity).rectangle;

		    if (rectangle.Contains(position)) {
			playerReachedExit = playerEntity;
			break;
		    }
		}

		if (playerReachedExit != null) {
		    break;
		}
	    }

	    if (playersFound == 0 || playerReachedExit != null) {
		FinishGame(world, playerReachedExit, data);
	    }
	}

	private void FinishGame(
	    EcsWorld world, int? playerEntity, GameSharedData data
	) {
	    var inventoryPool = world.GetPool<Inventory>();
	    var itemPool = world.GetPool<Item>();

	    // Updating and saving items and experience
	    int inventoryCapacity = data.rules.inventoryCapacity;
	    if (playerEntity != null) {
		// All items remain and all experience gained is added

		ref Inventory inventory = ref inventoryPool
		    .Get((int) playerEntity);

		for (int i = 0; i < inventoryCapacity; ++i) {
		    int? itemEntity = inventory.items[i];

		    if (itemEntity == null) {
			data.playerData.items[i] = null;
			continue;
		    }

		    ref Item item = ref itemPool.Get((int) itemEntity);
		    data.playerData.items[i] = item.id;
		}

		ref GameStats gameStats = ref world
		    .FindFirstComponent<GameStats>();

		data.playerData.experience +=
		    gameStats.experienceGained;
	    } else {
		// No items remain, no experience gained is added

		for (int i = 0; i < inventoryCapacity; ++i) {
		    data.playerData.items[i] = null;
		}
	    }

	    data.playerData.Save();
	    SceneManager.LoadScene("Scenes/MainMenuScene");
	}
    }
}
