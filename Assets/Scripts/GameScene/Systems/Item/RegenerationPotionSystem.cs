using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class RegenerationPotionSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    HandleUsedPotions(world);
	    HandleThrownPotions(world, data);
	}

	private void HandleUsedPotions(EcsWorld world) {
	    var potionPool = world.GetPool<RegenerationPotion>();
	    var buffPool = world.GetPool<RegenerationBuff>();

	    int playerEntity = world.FindFirstEntityWith<Player>();

	    var usedPotions = world
		.Filter<Item>()
		.Inc<RegenerationPotion>()
		.Inc<ItemUsedTag>()
		.End();
	    foreach (int potionEntity in usedPotions) {
		ApplyBuff(world, potionEntity, playerEntity);
	    }
	}

	private void HandleThrownPotions(
	    EcsWorld world, GameSharedData data
	) {
	    var positionPool = world.GetPool<Position>();
	    var potionPool = world.GetPool<RegenerationPotion>();
	    var buffPool = world.GetPool<RegenerationBuff>();

	    float effectSqrDistance = data.rules.potionEffectDistance *
		data.rules.potionEffectDistance;

	    var thrownPotions = world
		.Filter<ThrownPotion>()
		.Inc<RegenerationPotion>()
		.Inc<PotionReachedTargetTag>()
		.End();
	    foreach (int potionEntity in thrownPotions) {
		Vector2 position = positionPool
		    .Get(potionEntity).position;

		var targets = world
		    .Filter<Health>()
		    .Inc<Position>()
		    .End();
		foreach (int targetEntity in targets) {
		    Vector2 targetPosition = positionPool
			.Get(targetEntity).position;

		    float sqrDistance =
			(targetPosition - position).sqrMagnitude;
		    if (sqrDistance < effectSqrDistance) {
			ApplyBuff(world, potionEntity, targetEntity);
		    }
		}
	    }
	}

	private void ApplyBuff(
	    EcsWorld world, int potionEntity, int target
	) {
	    var buffPool = world.GetPool<RegenerationBuff>();
	    var potionPool = world.GetPool<RegenerationPotion>();

	    ref RegenerationPotion potion = ref potionPool
		.Get(potionEntity);

	    if (!buffPool.Has(target)) {
		buffPool.Add(target);
	    }

	    buffPool.Get(target) = new RegenerationBuff(
		potion.rate,
		potion.duration
	    );
	}
    }
}
