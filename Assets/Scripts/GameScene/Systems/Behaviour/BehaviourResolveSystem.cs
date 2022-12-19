using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class BehaviourResolveSystem : IEcsRunSystem {
	private struct BestUtility {
	    public float utility;
	    public int behaviourEntity;
	}
	
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    DetermineBestUtilities(world);
	    ActivateBestUtilities(world);

	    world.DeleteAll<BestUtility>();
	}

	private void DetermineBestUtilities(EcsWorld world) {
	    var behaviourPool = world.GetPool<AgentBehaviour>();
	    var bestUtilityPool = world.GetPool<BestUtility>();

	    var behaviours = world.Filter<AgentBehaviour>().End();
	    foreach (int behaviourEntity in behaviours) {
		ref AgentBehaviour behaviour = ref behaviourPool
		    .Get(behaviourEntity);

		if (behaviour.utility == 0) {
		    continue;
		}

		if (!bestUtilityPool.Has(behaviour.entity)) {
		    bestUtilityPool.AddInitialized(
			behaviour.entity,
			new BestUtility {
			    utility = behaviour.utility,
			    behaviourEntity = behaviourEntity
			}
		    );
		} else {
		    ref BestUtility best = ref bestUtilityPool
			.Get(behaviour.entity);

		    if (behaviour.utility > best.utility) {
			best.utility = behaviour.utility;
			best.behaviourEntity = behaviourEntity;
		    }
		}
	    }
	}

	private void ActivateBestUtilities(EcsWorld world) {
	    var behaviourPool = world.GetPool<AgentBehaviour>();
	    var bestUtilityPool = world.GetPool<BestUtility>();

	    var behaviours = world.Filter<AgentBehaviour>().End();
	    foreach (int behaviourEntity in behaviours) {
		ref AgentBehaviour behaviour = ref behaviourPool
		    .Get(behaviourEntity);

		if (behaviour.utility == 0) {
		    behaviour.isActive = false;
		    continue;
		}

		ref BestUtility best = ref bestUtilityPool
		    .Get(behaviour.entity);

		behaviour.isActive =
		    best.behaviourEntity == behaviourEntity;
	    }
	}
    }
}
