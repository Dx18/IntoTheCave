using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct InputItemPickupImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<InputItemPickupBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputItemPickupBehaviour>()
		.Get(behaviourEntity);

	    var requests = world.Filter<ItemPickupInputRequest>().End();
	    foreach (int requestEntity in requests) {
		return 1;
	    }

	    return 0;
	}

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputItemPickupBehaviour>()
		.Get(behaviourEntity);

	    Vector2 position = world
		.GetPool<Position>().Get(entity).position;
	    ref Inventory inventory = ref world
		.GetPool<Inventory>().Get(entity);

	    int nextSlot = 0;
	    while (nextSlot < inventory.items.Length &&
		   inventory.items[nextSlot] != null) {
		++nextSlot;
	    }
	    if (nextSlot == inventory.items.Length) {
		return;
	    }

	    var requests = world
		.Filter<ItemPickupInputRequest>().End();
	    foreach (int requestEntity in requests) {
		int? itemEntity = FindNearestPickableItem(
		    world, position, data
		);

		if (itemEntity == null) {
		    break;
		}

		ItemUtils.MakeItemPickedUp(world, (int) itemEntity);

		inventory.items[nextSlot] = itemEntity;

		while (nextSlot < inventory.items.Length &&
		       inventory.items[nextSlot] != null) {
		    ++nextSlot;
		}
		if (nextSlot == inventory.items.Length) {
		    return;
		}
	    }
	}

	private int? FindNearestPickableItem(
	    EcsWorld world, Vector2 position, GameSharedData data
	) {
	    var positionPool = world.GetPool<Position>();

	    float minSqrDistance = data.rules.itemPickupDistance *
		data.rules.itemPickupDistance;
	    int? bestItemEntity = null;

	    var items = world.Filter<DroppedItem>().End();
	    foreach (int itemEntity in items) {
		Vector2 itemPosition = positionPool
		    .Get(itemEntity).position;

		float sqrDistance =
		    (itemPosition - position).sqrMagnitude;

		if (sqrDistance <= minSqrDistance) {
		    minSqrDistance = sqrDistance;
		    bestItemEntity = itemEntity;
		}
	    }

	    return bestItemEntity;
	}
    }
}
