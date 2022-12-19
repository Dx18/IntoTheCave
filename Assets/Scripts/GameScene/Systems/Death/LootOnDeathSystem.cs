using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class LootOnDeathSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    var lootOnDeathPool = world
		.GetPool<LootOnDeath>();
	    var positionPool = world.GetPool<Position>();

	    var entities = world
		.Filter<GameAgentDiedTag>()
		.Inc<LootOnDeath>()
		.End();
	    foreach (int entity in entities) {
		int itemEntity = lootOnDeathPool.Get(entity).item;
		Vector2 position = positionPool.Get(entity).position;

		ItemUtils.MakeItemDropped(
		    world, itemEntity, position, data
		);
	    }
	}
    }
}
