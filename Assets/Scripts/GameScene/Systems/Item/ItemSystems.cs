using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class ItemSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new RegenerationPotionSystem(),
		new SpeedPotionSystem(),
		new ItemRemoveSystem()
	    };
	}
    }
}
