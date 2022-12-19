using Leopotam.EcsLite;

namespace Game {
    public sealed class PlayerInputClearSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    world.DeleteAll<MeleeAttackInputRequest>();
	    world.DeleteAll<ItemPickupInputRequest>();
	    world.DeleteAll<ItemDropInputRequest>();
	    world.DeleteAll<ItemUseInputRequest>();
	}
    }
}
