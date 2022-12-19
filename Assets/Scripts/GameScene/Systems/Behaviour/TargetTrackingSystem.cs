using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class TargetTrackingSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var trackedTargetPool = world.GetPool<TrackedTarget>();
	    var visionPool = world.GetPool<Vision>();
	    var positionPool = world.GetPool<Position>();
	    var teamPool = world.GetPool<Team>();

	    var entities = world.Filter<TrackedTarget>().End();
	    foreach (int entity in entities) {
		ref TrackedTarget target = ref trackedTargetPool
		    .Get(entity);

		float visionDistance = visionPool.Get(entity).distance;
		Vector2 position = positionPool.Get(entity).position;
		AgentTeam team = teamPool.Get(entity).team;

		float visionSqrDistance =
		    visionDistance * visionDistance;

		// Step 1: losing target if necessary
		if (target.target != null) {
		    target.target = TryLoseTarget(
			world, (int) target.target, position,
			visionDistance
		    );
		}
	    
		// Step 2: acquiring new target if necessary
		if (target.target == null) {
		    target.target = TryFindTarget(
			world, position, visionDistance, team
		    );
		}
	    }
	}

	private int? TryLoseTarget(
	    EcsWorld world, int target, Vector2 position,
	    float visionDistance
	) {
	    Vector2 targetPosition = world
		.GetPool<Position>()
		.Get(target)
		.position;

	    float currSqrDistance =
		(targetPosition - position).sqrMagnitude;
	    float visionSqrDistance =
		visionDistance * visionDistance;

	    return currSqrDistance > visionSqrDistance ? null : target;
	}

	private int? TryFindTarget(
	    EcsWorld world, Vector2 position, float visionDistance,
	    AgentTeam team
	) {
	    var teamPool = world.GetPool<Team>();
	    var positionPool = world.GetPool<Position>();

	    float visionSqrDistance =
		visionDistance * visionDistance;

	    var entities = world
		.Filter<Team>()
		.Inc<Position>()
		.End();
	    foreach (int targetEntity in entities) {
		ref Team targetTeam = ref teamPool.Get(targetEntity);
		if (targetTeam.team == team) {
		    continue;
		}

		Vector2 targetPosition = positionPool
		    .Get(targetEntity)
		    .position;

		float currSqrDistance =
		    (targetPosition - position).sqrMagnitude;

		if (currSqrDistance <= visionSqrDistance) {
		    return targetEntity;
		}
	    }

	    return null;
	}
    }
}
