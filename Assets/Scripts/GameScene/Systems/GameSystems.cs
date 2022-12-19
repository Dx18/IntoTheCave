using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;

namespace Game {
    public static class GameSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return BuffSystems.GetFixedUpdateSystems()
		.Concat(BehaviourSystems.GetFixedUpdateSystems())
		.Concat(ProjectileSystems.GetFixedUpdateSystems())
		.Concat(ItemSystems.GetFixedUpdateSystems())
		.Concat(MiscSystems.GetFixedUpdateSystems())
		.Concat(DeathSystems.GetFixedUpdateSystems())
		.Concat(GameFinishSystems.GetFixedUpdateSystems())
		.Concat(InputSystems.GetFixedUpdateSystems())
		.Concat(GenericSystems.GetFixedUpdateSystems())
		.Concat(PhysicsSystems.GetFixedUpdateSystems());
	}

	public static IEnumerable<IEcsSystem> GetUpdateSystems() {
	    return InputSystems.GetUpdateSystems()
		.Concat(UISystems.GetUpdateSystems());
	}
    }
}
