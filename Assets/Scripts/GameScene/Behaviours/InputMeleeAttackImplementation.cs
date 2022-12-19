using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public struct InputMeleeAttackImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<InputMeleeAttackBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputMeleeAttackBehaviour>()
		.Get(behaviourEntity);

	    var requests = world
		.Filter<MeleeAttackInputRequest>()
		.End();
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
		.GetPool<InputMeleeAttackBehaviour>()
		.Get(behaviourEntity);

	    float meleeAttackSqrDistance =
		data.rules.meleeAttackDistance *
		data.rules.meleeAttackDistance;

	    var healthPool = world.GetPool<Health>();
	    var positionPool = world.GetPool<Position>();
	    var teamPool = world.GetPool<Team>();

	    Vector2 position = positionPool.Get(entity).position;
	    float damage = world.GetPool<Damage>().Get(entity).damage;
	    AgentTeam team = teamPool.Get(entity).team;

	    var targets = world
		.Filter<Health>()
		.Inc<Position>()
		.Inc<Team>()
		.End();
	    foreach (int targetEntity in targets) {
		Vector2 targetPosition = positionPool
		    .Get(targetEntity).position;
		AgentTeam targetTeam = teamPool
		    .Get(targetEntity).team;

		float sqrDistance =
		    (targetPosition - position).sqrMagnitude;
		if (sqrDistance < meleeAttackSqrDistance &&
		    targetTeam != team) {
		    ref Health targetHealth = ref healthPool
			.Get(targetEntity);

		    targetHealth.health -= damage;
		}
	    }
	}
    }
}
