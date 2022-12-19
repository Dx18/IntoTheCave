using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class DeathCleanSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    var transformPool = world.GetPool<TransformInfo>();

	    var entities = world.Filter<GameAgentDiedTag>().End();
	    foreach (int entity in entities) {
		GameObject.Destroy(
		    rigidbodyPool.Has(entity)
		    ? rigidbodyPool.Get(entity).gameObject
		    : transformPool.Get(entity).gameObject
		);
		world.DelEntity(entity);
	    }
	}
    }
}
