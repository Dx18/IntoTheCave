using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;

namespace Game {
    public static class BehaviourSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] { new TargetTrackingSystem() }
		.Concat(BehaviourEvaluateSystems.GetFixedUpdateSystems())
		.Concat(new IEcsSystem[] { new BehaviourResolveSystem() })
		.Concat(BehaviourApplySystems.GetFixedUpdateSystems());
	}
    }
}
