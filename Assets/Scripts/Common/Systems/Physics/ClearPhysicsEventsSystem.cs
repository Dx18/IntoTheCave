using Leopotam.EcsLite;

public sealed class ClearPhysicsEventsSystem : IEcsRunSystem {
    public void Run(IEcsSystems systems) {
	EcsWorld world = systems.GetWorld();

	world.DeleteAll<TriggerEnterEvent>();
	world.DeleteAll<TriggerStayEvent>();
	world.DeleteAll<TriggerExitEvent>();
    }
}
