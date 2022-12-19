using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct IdleImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<IdleBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<IdleBehaviour>()
		.Get(behaviourEntity);

	    return data.rules.idleUtility;
	}

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<IdleBehaviour>()
		.Get(behaviourEntity);

	    ref Velocity velocity = ref world
		.GetPool<Velocity>().Get(entity);

	    velocity.velocity = Vector2.zero;
	}
    }
}
