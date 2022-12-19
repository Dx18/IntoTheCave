using Leopotam.EcsLite;

namespace Game {
    public sealed class ItemRemoveSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();

	    world.DeleteAllWith<ItemUsedTag>();
	    world.DeleteAllWith<PotionReachedTargetTag>();
	}
    }
}
