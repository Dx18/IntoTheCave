using UnityEngine;
using Leopotam.EcsLite;

using Random = UnityEngine.Random;

namespace Game {
    public static class PotionUtils {
	public static string AddRandomPotion(
	    EcsWorld world, int potionEntity, int level,
	    bool positive, GameSharedData data
	) {
	    switch (Random.Range(0, 2)) {
		case 0:
		    AddSpeedPotion(
			world, potionEntity, level, positive, data
		    );
		    return "speedPotion";
		case 1:
		    AddRegenerationPotion(
			world, potionEntity, level, positive, data
		    );
		    return "regenerationPotion";
		default:
		    return null;
	    }
	}

	public static void AddSpeedPotion(
	    EcsWorld world, int potionEntity, int level,
	    bool positive, GameSharedData data
	) {
	    float multiplier = data.rules
		.CalculateSpeedPotionMultiplier(level, positive);
	    float duration = data.rules
		.CalculatePotionDuration(level);

	    world.GetPool<SpeedPotion>().AddInitialized(
		potionEntity,
		new SpeedPotion(multiplier, duration)
	    );
	}

	public static void AddRegenerationPotion(
	    EcsWorld world, int potionEntity, int level,
	    bool positive, GameSharedData data
	) {
	    float rate = data.rules
		.CalculateRegenerationPotionRate(level, positive);
	    float duration = data.rules
		.CalculatePotionDuration(level);

	    world.GetPool<RegenerationPotion>().AddInitialized(
		potionEntity,
		new RegenerationPotion(rate, duration)
	    );
	}
    }
}
