using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Game {
    public class ItemSpawner {
	private delegate void FeatureFunc(
	    EcsWorld world, int itemEntity, GameSharedData data
	);

	private EcsWorld _world;
	private GameSharedData _data;

	private int _level;

	private Dictionary<string, FeatureFunc> _features;

	public ItemSpawner() {
	    _features = new Dictionary<string, FeatureFunc>() {
		{
		    "speedPotion",
		    (world, itemEntity, data) =>
		    PotionUtils.AddSpeedPotion(
			world, itemEntity,
			data.rules.CalculateLevel(
			    data.playerData.experience
			),
			true, data
		    )
		},
		{
		    "regenerationPotion",
		    (world, itemEntity, data) =>
		    PotionUtils.AddRegenerationPotion(
			world, itemEntity,
			data.rules.CalculateLevel(
			    data.playerData.experience
			),
			true, data
		    )
		},
		{
		    "negativeSpeedPotion",
		    (world, itemEntity, data) =>
		    PotionUtils.AddSpeedPotion(
			world, itemEntity,
			data.rules.CalculateLevel(
			    data.playerData.experience
			),
			false, data
		    )
		},
		{
		    "negativeRegenerationPotion",
		    (world, itemEntity, data) =>
		    PotionUtils.AddRegenerationPotion(
			world, itemEntity,
			data.rules.CalculateLevel(
			    data.playerData.experience
			),
			false, data
		    )
		},
		{
		    "usable",
		    (world, itemEntity, data) =>
		    world.GetPool<UsableItem>().Add(itemEntity)
		}
	    };
	}

	public int SpawnItem(
	    EcsWorld world, string id, GameSharedData data
	) {
	    int itemEntity = EntityBuilder.BeginEmpty(world)
		.Build();
	    AddItem(world, itemEntity, id, data);
	    return itemEntity;
	}

	public void AddItem(
	    EcsWorld world, int itemEntity, string id,
	    GameSharedData data
	) {
	    ItemInfo itemInfo = data.rules.FindItemInfo(id);

	    world.GetPool<Item>().AddInitialized(
		itemEntity, new Item(
		    itemInfo.id, itemInfo.name, itemInfo.sprite
		)
	    );

	    foreach (string feature in itemInfo.features) {
		_features[feature](world, itemEntity, data);
	    }
	}
    }
}
