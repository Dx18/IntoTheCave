using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class ThrownStonesSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    int playerLayer = LayerMask.NameToLayer("Player");

	    var eventPool = world.GetPool<TriggerEnterEvent>();
	    var thrownStonePool = world.GetPool<ThrownStone>();
	    var healthPool = world.GetPool<Health>();
	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();

	    var events = world.Filter<TriggerEnterEvent>().End();
	    foreach (int eventEntity in events) {
		ref var eventData = ref eventPool.Get(eventEntity);

		int stoneEntity = eventData.entity;
		if (!thrownStonePool.Has(stoneEntity)) {
		    continue;
		}

		int? hitEntity = eventData.otherEntity;
		if (hitEntity != null &&
		    healthPool.Has((int) hitEntity)) {
		    float damage = thrownStonePool
			.Get(stoneEntity).damage;

		    ref Health targetHealth = ref healthPool
			.Get((int) hitEntity);

		    targetHealth.health -= damage;
		}

		GameObject.Destroy(
		    rigidbodyPool.Get(stoneEntity).gameObject
		);
		world.DelEntity(stoneEntity);
	    }
	}
    }
}
