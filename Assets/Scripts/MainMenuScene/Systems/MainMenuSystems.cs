using System.Collections.Generic;
using Leopotam.EcsLite;

namespace MainMenu {
    public static class MainMenuSystems {
	public static IEnumerable<IEcsSystem> GetFixedUpdateSystems() {
	    return new IEcsSystem[0];
	}

	public static IEnumerable<IEcsSystem> GetUpdateSystems() {
	    return new IEcsSystem[] {
		new RecipeSwitchSystem(),
		new CraftSystem(),
		new MainMenuUISystem(),
		new GameStartSystem()
	    };
	}
    }
}
