using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct PotionThrowImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<PotionThrowBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<PotionThrowBehaviour>()
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
		.GetPool<PotionThrowBehaviour>()
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

	    Vector2 velocity =
		(targetPosition - position).normalized *
		data.rules.potionSpeed;

	    int potionEntity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.potionPrefab, position
		)
		.WithMonoBehaviour<TriggerEnterEmitter>(
		    emitter => emitter.world = world
		)
		.With(new Position(position))
		.With(new Velocity(velocity))
		.With(new ThrownPotion(targetPosition))
		.Build();

	    int level = data.rules.CalculateLevel(
		data.playerData.experience
	    );

	    PotionUtils.AddRandomPotion(
		world, potionEntity, level, false, data
	    );
	}
    }
}
