using Leopotam.EcsLite;

namespace Game {
    public sealed class DanglingFollowingCameraSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var followingCameraPool = world.GetPool<FollowingCamera>();
	    var gameAgentDiedTagPool = world
		.GetPool<GameAgentDiedTag>();

	    var entities = world.Filter<FollowingCamera>().End();
	    foreach (int entity in entities) {
		int target = followingCameraPool.Get(entity).target;

		if (gameAgentDiedTagPool.Has(target)) {
		    world.DelEntity(entity);
		}
	    }
	}
    }
}
