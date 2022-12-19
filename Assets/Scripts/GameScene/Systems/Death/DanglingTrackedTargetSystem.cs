using Leopotam.EcsLite;

namespace Game {
    public sealed class DanglingTrackedTargetSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var trackedTargetPool = world.GetPool<TrackedTarget>();
	    var gameAgentDiedTagPool = world
		.GetPool<GameAgentDiedTag>();

	    var entities = world.Filter<TrackedTarget>().End();
	    foreach (int entity in entities) {
		ref int? target = ref trackedTargetPool
		    .Get(entity).target;

		if (target != null &&
		    gameAgentDiedTagPool.Has((int) target)) {
		    target = null;
		}
	    }
	}
    }
}
