using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class BehaviourApplySystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new BehaviourApplySystem<IdleImplementation>(),
		new BehaviourApplySystem<InputMovementImplementation>(),
		new BehaviourApplySystem<InputMeleeAttackImplementation>(),
		new BehaviourApplySystem<InputItemPickupImplementation>(),
		new BehaviourApplySystem<InputItemDropImplementation>(),
		new BehaviourApplySystem<InputItemUseImplementation>(),
		new BehaviourApplySystem<ApproachImplementation>(),
		new BehaviourApplySystem<FleeImplementation>(),
		new BehaviourApplySystem<MeleeAttackImplementation>(),
		new BehaviourApplySystem<StoneThrowImplementation>(),
		new BehaviourApplySystem<PotionThrowImplementation>(),
		new BehaviourApplySystem<SlimeAttackImplementation>(),
		new BehaviourApplySystem<SlimeSpawnImplementation>()
	    };
	}
    }
}
