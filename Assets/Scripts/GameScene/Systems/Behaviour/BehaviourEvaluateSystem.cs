using Leopotam.EcsLite;

namespace Game {
    public sealed class BehaviourEvaluateSystem<T>
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

		agentBehaviour.utility = impl.Evaluate(
		    world, agentBehaviour.entity, behaviourEntity,
		    data
		);
	    }
	}
    }
}
