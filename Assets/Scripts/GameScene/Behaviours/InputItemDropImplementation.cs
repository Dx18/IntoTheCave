using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct InputItemDropImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<InputItemDropBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputItemDropBehaviour>()
		.Get(behaviourEntity);

	    var requests = world.Filter<ItemDropInputRequest>().End();
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
		.GetPool<InputItemDropBehaviour>()
		.Get(behaviourEntity);

	    Vector2 position = world
		.GetPool<Position>().Get(entity).position;
	    ref Inventory inventory = ref world
		.GetPool<Inventory>().Get(entity);

	    var requestPool = world.GetPool<ItemDropInputRequest>();

	    var requests = world.Filter<ItemDropInputRequest>().End();
	    foreach (int requestEntity in requests) {
		int slot = requestPool.Get(requestEntity).slot;

		if (inventory.items[slot] == null) {
		    continue;
		}

		ItemUtils.MakeItemDropped(
		    world, (int) inventory.items[slot], position, data
		);
		inventory.items[slot] = null;
	    }
	}
    }
}
