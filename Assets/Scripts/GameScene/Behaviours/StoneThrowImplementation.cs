using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct StoneThrowImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<StoneThrowBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<StoneThrowBehaviour>()
		.Get(behaviourEntity);

	    behaviour.cooldown -= Time.deltaTime;
	    if (behaviour.cooldown > 0) {
		return 0;
	    }

	    int? targetEntity = world
		.GetPool<TrackedTarget>().Get(entity).target;

	    return targetEntity != null ? 1 : 0;
	}

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<StoneThrowBehaviour>()
		.Get(behaviourEntity);

	    if (behaviour.cooldown > 0) {
		return;
	    }
	    behaviour.cooldown = behaviour.baseCooldown;

	    int? targetEntity = world
		.GetPool<TrackedTarget>().Get(entity).target;

	    if (targetEntity == null) {
		return;
	    }

	    Vector2 targetPosition = world
		.GetPool<Position>().Get((int) targetEntity).position;

	    Vector2 position = world
		.GetPool<Position>().Get(entity).position;
	    float damage = world
		.GetPool<Damage>().Get(entity).damage;

	    Vector2 velocity =
		(targetPosition - position).normalized *
		data.rules.stoneSpeed;

	    EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.stonePrefab, position
		)
		.WithMonoBehaviour<TriggerEnterEmitter>(
		    emitter => emitter.world = world
		)
		.With(new Position(position))
		.With(new Velocity(velocity))
		.With(new ThrownStone(damage));
	}
    }
}
