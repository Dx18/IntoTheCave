using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class DeathTagSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var healthPool = world.GetPool<Health>();
	    var gameAgentDiedTagPool = world
		.GetPool<GameAgentDiedTag>();

	    var entities = world.Filter<Health>().End();
	    foreach (int entity in entities) {
		ref Health health = ref healthPool.Get(entity);
		if (health.health > 0) {
		    continue;
		}

		gameAgentDiedTagPool.Add(entity);
	    }
	}
    }
}
