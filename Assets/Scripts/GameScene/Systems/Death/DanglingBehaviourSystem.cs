using Leopotam.EcsLite;

namespace Game {
    public sealed class DanglingBehaviourSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var behaviourPool = world.GetPool<AgentBehaviour>();
	    var gameAgentDiedTagPool = world
		.GetPool<GameAgentDiedTag>();

	    var entities = world.Filter<AgentBehaviour>().End();
	    foreach (int behaviourEntity in entities) {
		int entity = behaviourPool.Get(behaviourEntity).entity;

		if (gameAgentDiedTagPool.Has(entity)) {
		    world.DelEntity(behaviourEntity);
		}
	    }
	}
    }
}
