using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public static class InputSystems {
	public static IEnumerable<IEcsSystem> GetUpdateSystems() {
	    return new IEcsSystem[] {
		new PlayerInputSystem()
	    };
	}

	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[] {
		new PlayerInputClearSystem()
	    };
	}
    }
}
