using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public static class ItemUtils {
	public static void MakeItemDropped(
	    EcsWorld world, int itemEntity, Vector2 position,
	    GameSharedData data
	) {
	    var transformPool = world.GetPool<TransformInfo>();
	    var positionPool = world.GetPool<Position>();
	    var droppedItemPool = world.GetPool<DroppedItem>();
	    var itemPool = world.GetPool<Item>();

	    ref Item item = ref itemPool.Get(itemEntity);

	    GameObject instance = GameObject.Instantiate(
		data.prefabs.droppedItemPrefab,
		new Vector3(position.x, position.y, 0),
		Quaternion.identity
	    );

	    instance.GetComponent<SpriteRenderer>().sprite =
		item.sprite;

	    transformPool.AddInitialized(
		itemEntity, new TransformInfo(instance)
	    );
	    positionPool.AddInitialized(
		itemEntity, new Position(position)
	    );
	    droppedItemPool.AddInitialized(
		itemEntity, new DroppedItem()
	    );
	}

	public static void MakeItemPickedUp(
	    EcsWorld world, int itemEntity
	) {
	    var transformPool = world.GetPool<TransformInfo>();
	    var positionPool = world.GetPool<Position>();
	    var droppedItemPool = world.GetPool<DroppedItem>();

	    GameObject instance = transformPool
		.Get(itemEntity).gameObject;

	    GameObject.Destroy(instance);

	    transformPool.Del(itemEntity);
	    positionPool.Del(itemEntity);
	    droppedItemPool.Del(itemEntity);
	}
    }
}
