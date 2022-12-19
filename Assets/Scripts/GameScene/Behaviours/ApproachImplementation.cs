using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct ApproachImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<ApproachBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<ApproachBehaviour>()
		.Get(behaviourEntity);

	    int? targetEntity = world
		.GetPool<TrackedTarget>().Get(entity).target;

	    if (targetEntity == null) {
		return 0;
	    }

	    Vector2 targetPosition = world
		.GetPool<Position>()
		.Get((int) targetEntity)
		.position;

	    Vector2 position = world
		.GetPool<Position>().Get(entity).position;

	    float currSqrDistance =
		(targetPosition - position).sqrMagnitude;
	    float approachSqrDistance =
		behaviour.distance * behaviour.distance;

	    return currSqrDistance > approachSqrDistance
		? 1 / (1 + currSqrDistance - approachSqrDistance)
		: 0;
	}

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<ApproachBehaviour>()
		.Get(behaviourEntity);
	    
	    int? targetEntity = world
		.GetPool<TrackedTarget>().Get(entity).target;

	    if (targetEntity == null) {
		return;
	    }

	    Vector2 targetPosition = world
		.GetPool<Position>()
		.Get((int) targetEntity)
		.position;

	    Vector2 position = world
		.GetPool<Position>().Get(entity).position;
	    float speed = world
		.GetPool<Speed>().Get(entity).speed;
	    ref Velocity velocity = ref world
		.GetPool<Velocity>().Get(entity);

	    velocity.velocity =
		(targetPosition - position).normalized * speed;
	}
    }
}
