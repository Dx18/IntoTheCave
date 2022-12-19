using Leopotam.EcsLite;

namespace Game {
    public sealed class BehaviourApplySystem<T>
	: IEcsRunSystem
    where T : struct, IBehaviourImplementation {
	public void Run(IEcsSystems systems) {
	    T impl = new T();

	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    var agentBehaviourPool = world.GetPool<AgentBehaviour>();

	    var behaviours = impl.GetBehaviourFilter(world);
	    foreach (int behaviourEntity in behaviours) {
		ref AgentBehaviour agentBehaviour =
		    ref agentBehaviourPool.Get(behaviourEntity);
		if (!agentBehaviour.isActive) {
		    continue;
		}

		impl.Apply(
		    world, agentBehaviour.entity, behaviourEntity,
		    data
		);
	    }
	}
    }
}
