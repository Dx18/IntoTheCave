using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class CameraFollowSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var followingCameraPool = world.GetPool<FollowingCamera>();
	    var positionPool = world.GetPool<Position>();

	    var cameras = world
		.Filter<FollowingCamera>()
		.Inc<Position>()
		.End();
	    foreach (int cameraEntity in cameras) {
		ref FollowingCamera camera = ref followingCameraPool
		    .Get(cameraEntity);

		ref Position position = ref positionPool
		    .Get(cameraEntity);
		Vector2 targetPosition = positionPool
		    .Get(camera.target).position;

		position.position = Vector2.Lerp(
		    position.position, targetPosition,
		    1 - camera.smoothness
		);
	    }
	}
    }
}
