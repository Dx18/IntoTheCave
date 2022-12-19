using UnityEngine;
using Leopotam.EcsLite;

using Random = UnityEngine.Random;

namespace Game {
    public sealed class InitSystem : IEcsInitSystem {
	public void Init(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    SpawnGameState(world);

	    ItemSpawner itemSpawner = new ItemSpawner();
	    GameAgentSpawner gameAgentSpawner =
		new GameAgentSpawner(itemSpawner);

	    MapGenerator generator = new MapGenerator(
		world, gameAgentSpawner, data
	    );
	    Vector2 startPosition = generator.SpawnMap();

	    int playerEntity = gameAgentSpawner.SpawnPlayer(
		world, startPosition, data
	    );

	    EntityBuilder.BeginEmpty(world)
		.With(new TransformInfo(Camera.main.gameObject))
		.With(new Position(startPosition))
		.With(
		    new FollowingCamera(
			playerEntity,
			data.rules.followingCameraSmoothness
		    )
		)
		.Build();
	}


	private void SpawnGameState(EcsWorld world) {
	    EntityBuilder.BeginEmpty(world)
		.With(new GameStats { experienceGained = 0 });
	}
    }
}
