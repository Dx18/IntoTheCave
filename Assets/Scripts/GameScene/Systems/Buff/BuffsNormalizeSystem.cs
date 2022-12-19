using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class BuffsNormalizeSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var healthPool = world.GetPool<Health>();
	    var damagePool = world.GetPool<Damage>();

	    var healthEntities = world.Filter<Health>().End();
	    foreach (int entity in healthEntities) {
		ref Health health = ref healthPool.Get(entity);
		health.health = Mathf.Clamp(
		    health.health, 0, health.maxHealth
		);
	    }
	}
    }
}
