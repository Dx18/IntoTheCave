using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class DeathSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new DeathTagSystem(),
		new ExperienceOnDeathSystem(),
		new LootOnDeathSystem(),
		new DanglingBehaviourSystem(),
		new DanglingTrackedTargetSystem(),
		new DanglingFollowingCameraSystem(),
		new DeathCleanSystem()
	    };
	}
    }
}
