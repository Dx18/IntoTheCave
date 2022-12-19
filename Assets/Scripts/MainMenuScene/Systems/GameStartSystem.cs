using UnityEngine.SceneManagement;
using Leopotam.EcsLite;

namespace MainMenu {
    public sealed class GameStartSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    ref MainMenuUI mainMenuUi = ref world
		.FindFirstComponent<MainMenuUI>();

	    if (data.input.actions["Start"].WasPressedThisFrame()) {
		data.playerData.Save();
		SceneManager.LoadScene("Scenes/GameScene");
	    }
	}
    }
}
