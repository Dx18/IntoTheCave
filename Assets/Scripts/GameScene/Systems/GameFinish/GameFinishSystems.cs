using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class GameFinishSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new GameFinishSystem()
	    };
	}
    }
}
