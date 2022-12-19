using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct SlimeAttackImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<SlimeAttackBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<SlimeAttackBehaviour>()
		.Get(behaviourEntity);

	    behaviour.cooldown -= Time.deltaTime;
	    if (behaviour.cooldown > 0) {
		return 0;
	    }

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
	    float attackSqrDistance =
		behaviour.attackDistance * behaviour.attackDistance;

	    return currSqrDistance < attackSqrDistance ? 1 : 0;
	}

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<SlimeAttackBehaviour>()
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

	    ref Health targetHealth = ref world
		.GetPool<Health>()
		.Get((int) targetEntity);

	    float damage = world
		.GetPool<Damage>().Get(entity).damage;

	    targetHealth.health -= damage;

	    var speedBuffPool = world.GetPool<SpeedBuff>();

	    if (!speedBuffPool.Has((int) targetEntity)) {
		speedBuffPool.Add((int) targetEntity);
	    }

	    speedBuffPool.Get((int) targetEntity) = new SpeedBuff(
		behaviour.speedMultiplier,
		behaviour.duration
	    );
	}
    }
}
