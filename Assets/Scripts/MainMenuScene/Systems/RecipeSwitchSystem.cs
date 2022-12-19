using Leopotam.EcsLite;

namespace MainMenu {
    public sealed class RecipeSwitchSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    ref MainMenuUI mainMenuUi = ref world
		.FindFirstComponent<MainMenuUI>();

	    int recipeCount = data.rules.recipes.Length;
	    ref int index = ref mainMenuUi.selectedRecipe;
	    ref bool updateRequired = ref mainMenuUi.updateRequired;

	    if (data.input.actions["NextRecipe"].WasPressedThisFrame()) {
		++index;
		updateRequired = true;
	    }
	    if (data.input.actions["PrevRecipe"].WasPressedThisFrame()) {
		--index;
		updateRequired = true;
	    }
	    index = (index + recipeCount) % recipeCount;
	}
    }
}
