using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class BehaviourEvaluateSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new BehaviourEvaluateSystem<IdleImplementation>(),
		new BehaviourEvaluateSystem<InputMovementImplementation>(),
		new BehaviourEvaluateSystem<InputMeleeAttackImplementation>(),
		new BehaviourEvaluateSystem<InputItemPickupImplementation>(),
		new BehaviourEvaluateSystem<InputItemDropImplementation>(),
		new BehaviourEvaluateSystem<InputItemUseImplementation>(),
		new BehaviourEvaluateSystem<ApproachImplementation>(),
		new BehaviourEvaluateSystem<FleeImplementation>(),
		new BehaviourEvaluateSystem<MeleeAttackImplementation>(),
		new BehaviourEvaluateSystem<StoneThrowImplementation>(),
		new BehaviourEvaluateSystem<PotionThrowImplementation>(),
		new BehaviourEvaluateSystem<SlimeAttackImplementation>(),
		new BehaviourEvaluateSystem<SlimeSpawnImplementation>()
	    };
	}
    }
}
