using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MainMenu {
    public sealed class CraftSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    ref MainMenuUI mainMenuUi = ref world
		.FindFirstComponent<MainMenuUI>();

	    if (data.input.actions["Craft"].WasPressedThisFrame()) {
		HandleCrafting(world, data);
		mainMenuUi.updateRequired = true;
	    }
	}

	private void HandleCrafting(
	    EcsWorld world, GameSharedData data
	) {
	    ref MainMenuUI mainMenuUi = ref world
		.FindFirstComponent<MainMenuUI>();

	    RecipeInfo recipe =
		data.rules.recipes[mainMenuUi.selectedRecipe];
	    string[] items = data.playerData.items;

	    Dictionary<string, int> expected =
		new Dictionary<string, int>();
	    foreach (string item in recipe.inputs) {
		if (expected.ContainsKey(item)) {
		    ++expected[item];
		} else {
		    expected[item] = 1;
		}
	    }

	    Dictionary<string, int> actual =
		new Dictionary<string, int>();
	    foreach (string item in items) {
		if (item == null) {
		    continue;
		}

		if (actual.ContainsKey(item)) {
		    ++actual[item];
		} else {
		    actual[item] = 1;
		}
	    }

	    foreach (var entry in expected) {
		if (!actual.ContainsKey(entry.Key) ||
		    actual[entry.Key] < expected[entry.Key]) {
		    return;
		}
	    }

	    // Amount of resources is sufficient here

	    // Removing all recipe inputs from items
	    for (int i = 0; i < items.Length; ++i) {
		if (items[i] == null ||
		    !expected.ContainsKey(items[i]) ||
		    expected[items[i]] == 0) {
		    continue;
		}

		--expected[items[i]];
		items[i] = null;
	    }

	    // Adding output to items
	    for (int i = 0; i < items.Length; ++i) {
		if (items[i] == null) {
		    items[i] = recipe.output;
		    break;
		}
	    }
	}
    }
}
