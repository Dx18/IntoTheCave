using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;

public static class UIUtils {
    public static int CreateSlotUI(
	EcsWorld world, Transform parent, GameSharedData data
    ) {
	// Instantiating object and adding it to parent
	GameObject slot = GameObject.Instantiate(
	    data.prefabs.slotPrefab, Vector3.zero,
	    Quaternion.identity
	);
	Transform slotTransform = slot.transform;
	slotTransform.SetParent(parent, false);

	// Getting necessary UI elements
	Image item = slotTransform.Find("SlotItem")
	    .GetComponent<Image>();

	// Creating component for slot UI
	SlotUI slotUi = new SlotUI { item = item };

	// Creating entity
	return EntityBuilder.BeginEmpty(world)
	    .With(slotUi)
	    .Build();
    }

    public static int[] CreateMultipleSlotUI(
	EcsWorld world, int slotCount, Transform parent,
	GameSharedData data
    ) {
	int[] slots = new int[slotCount];
	for (int i = 0; i < slotCount; ++i) {
	    slots[i] = CreateSlotUI(world, parent, data);
	}
	return slots;
    }

    public static void UpdateSlotUI(
	EcsWorld world, int slotUiEntity, Sprite itemSprite
    ) {
	ref SlotUI slotUi = ref world
	    .GetPool<SlotUI>().Get(slotUiEntity);
	UpdateSlotUI(world, ref slotUi, itemSprite);
    }

    public static void UpdateSlotUI(
	EcsWorld world, ref SlotUI slotUi, Sprite itemSprite
    ) {
	slotUi.item.sprite = itemSprite;
	slotUi.item.color = new Color(
	    1, 1, 1, itemSprite == null ? 0 : 1
	);
    }
}
