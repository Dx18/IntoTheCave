using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class ThrownPotionsSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    int playerLayer = LayerMask.NameToLayer("Player");

	    var eventPool = world.GetPool<TriggerEnterEvent>();
	    var thrownPotionPool = world.GetPool<ThrownPotion>();
	    var positionPool = world.GetPool<Position>();
	    var potionReachedTargetTagPool = world
		.GetPool<PotionReachedTargetTag>();

	    float targetSqrDistance =
		data.rules.potionTargetDistance *
		data.rules.potionTargetDistance;

	    var potions = world.Filter<ThrownPotion>().End();
	    foreach (int potionEntity in potions) {
		Vector2 targetPosition = thrownPotionPool
		    .Get(potionEntity).targetPosition;

		Vector2 position = positionPool
		    .Get(potionEntity).position;

		float sqrDistance =
		    (targetPosition - position).sqrMagnitude;

		if (sqrDistance < targetSqrDistance) {
		    potionReachedTargetTagPool.Add(potionEntity);
		}
	    }

	    var events = world.Filter<TriggerEnterEvent>().End();
	    foreach (int eventEntity in events) {
		ref var eventData = ref eventPool.Get(eventEntity);

		int potionEntity = eventData.entity;
		if (!thrownPotionPool.Has(potionEntity) ||
		    potionReachedTargetTagPool.Has(potionEntity)) {
		    continue;
		}

		potionReachedTargetTagPool.Add(potionEntity);
	    }
	}
    }
}
