using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct InputItemUseImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<InputItemUseBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputItemUseBehaviour>()
		.Get(behaviourEntity);

	    var requests = world.Filter<ItemUseInputRequest>().End();
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
		.GetPool<InputItemUseBehaviour>()
		.Get(behaviourEntity);

	    Vector2 position = world
		.GetPool<Position>().Get(entity).position;
	    ref Inventory inventory = ref world
		.GetPool<Inventory>().Get(entity);

	    var requestPool = world.GetPool<ItemUseInputRequest>();
	    var usableItemPool = world.GetPool<UsableItem>();
	    var itemUsedTagPool = world.GetPool<ItemUsedTag>();

	    var requests = world.Filter<ItemUseInputRequest>().End();
	    foreach (int requestEntity in requests) {
		int slot = requestPool.Get(requestEntity).slot;

		if (inventory.items[slot] == null) {
		    continue;
		}

		int itemEntity = (int) inventory.items[slot];
		if (!usableItemPool.Has(itemEntity)) {
		    continue;
		}

		if (!itemUsedTagPool.Has(itemEntity)) {
		    itemUsedTagPool.Add(itemEntity);
		}
		inventory.items[slot] = null;
	    }
	}
    }
}
