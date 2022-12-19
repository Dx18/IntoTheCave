using System.Collections.Generic;
using Leopotam.EcsLite;

public static class PhysicsSystems {
    public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	return new IEcsSystem[] {
	    new ClearPhysicsEventsSystem()
	};
    }
}
