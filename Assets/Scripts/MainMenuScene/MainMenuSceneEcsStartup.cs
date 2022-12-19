using UnityEngine;
using UnityEngine.InputSystem;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

namespace MainMenu {
    public sealed class MainMenuSceneEcsStartup : MonoBehaviour {
	public GamePrefabs prefabs;
	public GameTiles tiles;
	public GameRules rules;
	public PlayerInput input;
	public EcsUguiEmitter uguiEmitter;

        private EcsWorld _world;        
	private IEcsSystems _fixedUpdateSystems;
	private IEcsSystems _updateSystems;

        void Start() {
            _world = new EcsWorld();

	    GameSharedData data = new GameSharedData {
		prefabs = prefabs,
		tiles = tiles,
		rules = rules,
		input = input,
		playerData = PlayerData.Load(rules)
	    };

            _fixedUpdateSystems = new EcsSystems(_world, data);
            _fixedUpdateSystems
		.AddAll(MainMenuSystems.GetFixedUpdateSystems())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();

            _updateSystems = new EcsSystems(_world, data);
            _updateSystems
		.AddAll(MainMenuSystems.GetUpdateSystems())
		.InjectUgui(uguiEmitter)
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Init();
        }

        void Update() {
            _updateSystems?.Run();
        }

        void FixedUpdate() {
            _fixedUpdateSystems?.Run();
        }

        void OnDestroy() {
            if (_fixedUpdateSystems != null) {
                _fixedUpdateSystems.Destroy();
                _fixedUpdateSystems = null;
            }

            if (_updateSystems != null) {
                _updateSystems.Destroy();
                _updateSystems = null;
            }

            if (_world != null) {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
