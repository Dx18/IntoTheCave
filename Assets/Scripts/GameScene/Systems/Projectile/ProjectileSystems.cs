using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class ProjectileSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new ThrownPotionsSystem(),
		new ThrownStonesSystem()
	    };
	}
    }
}
