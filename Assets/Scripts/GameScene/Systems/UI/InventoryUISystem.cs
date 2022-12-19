using System;
using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

namespace Game {
    public sealed class InventoryUISystem
	: IEcsInitSystem, IEcsRunSystem
    {
	private EcsUguiEmitter _uguiEmitter;

	[EcsUguiNamed("Inventory")]
	private Transform _inventory;

	public void Init(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    CreateInventoryUI(world, data);
	}

	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    UpdateInventoryUI(world);
	}

	private void CreateInventoryUI(
	    EcsWorld world, GameSharedData data
	) {
	    // Getting inventory capacity
	    int capacity = data.rules.inventoryCapacity;

	    // Creating slots
	    int[] slots = UIUtils.CreateMultipleSlotUI(
		world, capacity, _inventory, data
	    );

	    EntityBuilder.BeginEmpty(world)
		.With(new InventoryUI(slots));
	}

	private void UpdateInventoryUI(EcsWorld world) {
	    // Obtaining player's inventory and inventory UI
	    int playerEntity;
	    try {
		playerEntity = world.FindFirstEntityWith<Player>();
	    } catch (Exception) {
		return;
	    }
	    ref Inventory inventory = ref world
		.GetPool<Inventory>()
		.Get(playerEntity);
	    ref InventoryUI inventoryUi = ref world
		.FindFirstComponent<InventoryUI>();

	    var inventorySlotUiPool = world.GetPool<SlotUI>();

	    // Updating slots
	    for (int i = 0; i < inventory.items.Length; ++i) {
		UIUtils.UpdateSlotUI(
		    world, inventoryUi.slots[i],
		    GetItem(world, inventory.items[i])?.sprite
		);
	    }
	}

	private Item? GetItem(EcsWorld world, int? itemEntity) {
	    if (itemEntity == null) {
		return null;
	    }
	    return world.GetPool<Item>().Get((int) itemEntity);
	}
    }
}
