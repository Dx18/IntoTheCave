using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

namespace MainMenu {
    public sealed class MainMenuUISystem
	: IEcsInitSystem, IEcsRunSystem
    {
	private EcsUguiEmitter _uguiEmitter;

	[EcsUguiNamed("PlayerStats")]
	private TMP_Text _playerStatsText;

	[EcsUguiNamed("ItemImage")]
	private Image _itemImage;

	[EcsUguiNamed("ItemDescription")]
	private TMP_Text _itemDescriptionText;

	[EcsUguiNamed("Inputs")]
	private Transform _inputs;

	[EcsUguiNamed("Items")]
	private Transform _items;

	public void Init(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    InitMainMenuUI(world, data);
	}

	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    ref MainMenuUI mainMenuUi = ref world
		.FindFirstComponent<MainMenuUI>();

	    if (mainMenuUi.updateRequired) {
		UpdateMainMenuUI(world, data);
	    }
	}

	private void InitMainMenuUI(
	    EcsWorld world, GameSharedData data
	) {
	    InitPlayerStatsUI(data);

	    int[] recipeInputSlots =
		CreateRecipeInputSlots(world, data);
	    int[] playerItemsSlots =
		CreatePlayerItemsSlots(world, data);

	    EntityBuilder.BeginEmpty(world)
		.With(
		    new MainMenuUI(recipeInputSlots, playerItemsSlots)
		);

	    UpdateMainMenuUI(world, data);
	}

	private void InitPlayerStatsUI(GameSharedData data) {
	    int level = data.rules.CalculateLevel(
		data.playerData.experience
	    );
	    float health = data.rules.CalculateMaxHealth(level);
	    float speed = data.rules.CalculateBaseSpeed(level);
	    float damage = data.rules.CalculateBaseDamage(level);

	    _playerStatsText.text =
		$"Level: {level}\n" +
		$"Health: {health}\n" +
		$"Speed: {speed}\n" +
		$"Damage: {damage}";
	}

	private int[] CreateRecipeInputSlots(
	    EcsWorld world, GameSharedData data
	) {
	    int slotCount = data.rules.recipes
		.Select(recipe => recipe.inputs.Length).Max();

	    return UIUtils.CreateMultipleSlotUI(
		world, slotCount, _inputs, data
	    );
	}

	private int[] CreatePlayerItemsSlots(
	    EcsWorld world, GameSharedData data
	) {
	    int slotCount = data.playerData.items.Length;

	    return UIUtils.CreateMultipleSlotUI(
		world, slotCount, _items, data
	    );
	}

	private void UpdateMainMenuUI(
	    EcsWorld world, GameSharedData data
	) {
	    ref MainMenuUI mainMenuUi = ref world
		.FindFirstComponent<MainMenuUI>();

	    UpdateRecipesUI(world, ref mainMenuUi, data);
	    UpdatePlayerItemsUI(world, ref mainMenuUi, data);
	}

	private void UpdateRecipesUI(
	    EcsWorld world, ref MainMenuUI mainMenuUi,
	    GameSharedData data
	) {
	    RecipeInfo recipe =
		data.rules.recipes[mainMenuUi.selectedRecipe];

	    ItemInfo output = data.rules.FindItemInfo(recipe.output);

	    _itemImage.sprite = output.sprite;
	    _itemDescriptionText.text =
		$"{output.name}\n{output.description}";

	    // Updating slots
	    for (int i = 0; i < mainMenuUi.recipeInputSlots.Length; ++i) {
		ItemInfo? item = i < recipe.inputs.Length
		    ? data.rules.FindItemInfo(recipe.inputs[i])
		    : null;
		UIUtils.UpdateSlotUI(
		    world, mainMenuUi.recipeInputSlots[i],
		    item?.sprite
		);
	    }
	}

	private void UpdatePlayerItemsUI(
	    EcsWorld world, ref MainMenuUI mainMenuUi,
	    GameSharedData data
	) {
	    string[] items = data.playerData.items;

	    // Updating slots
	    for (int i = 0; i < mainMenuUi.playerItemsSlots.Length; ++i) {
		ItemInfo? item = items[i] != null
		    ? data.rules.FindItemInfo(items[i])
		    : null;
		UIUtils.UpdateSlotUI(
		    world, mainMenuUi.playerItemsSlots[i],
		    item?.sprite
		);
	    }
	}
    }
}
