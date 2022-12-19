using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class UISystems {
	public static IEnumerable<IEcsSystem> GetUpdateSystems() {
	    return new IEcsSystem[] {
		new InventoryUISystem(),
		new HealthUISystem()
	    };
	}
    }
}
