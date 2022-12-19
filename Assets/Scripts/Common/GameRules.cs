using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRules")]
public class GameRules : ScriptableObject {
    public float itemPickupDistance;
    public float potionEffectDistance;
    public float potionTargetDistance;
    public float meleeAttackDistance;
    public float stoneSpeed;
    public float potionSpeed;
    public int inventoryCapacity;

    public int roomWidth;
    public int roomHeightMin;
    public int roomHeightMax;

    public float visionDistance;

    public float ratMaxHealth;
    public float ratBaseSpeed;
    public float ratBaseDamage;
    public int ratExperience;
    public float ratAttackDistance;
    public float ratAttackCooldown;
    public float ratSpawnPotential;
    public float ratTailDropRate;

    public float stonethrowerMaxHealth;
    public float stonethrowerBaseSpeed;
    public float stonethrowerBaseDamage;
    public int stonethrowerExperience;
    public float stonethrowerMinDistance;
    public float stonethrowerMaxDistance;
    public float stonethrowerAttackCooldown;
    public float stonethrowerSpawnPotential;
    public float stonethrowerScalesDropRate;

    public float alchemistMaxHealth;
    public float alchemistBaseSpeed;
    public int alchemistExperience;
    public float alchemistMinDistance;
    public float alchemistMaxDistance;
    public float alchemistAttackCooldown;
    public float alchemistSpawnPotential;
    public float alchemistPotionDropRate;

    public float slimeMaxHealth;
    public float slimeBaseSpeed;
    public float slimeBaseDamage;
    public int slimeExperience;
    public float slimeAttackDistance;
    public float slimeSpeedMultiplier;
    public float slimeSpeedReduceDuration;
    public float slimeAttackCooldown;
    public float slimeSpawnPotential;
    public float slimeBallDropRate;

    public float slimeNestMaxHealth;
    public int slimeNestExperience;
    public float slimeNestSpawnCooldown;
    public float slimeNestSpawnDistance;
    public float slimeNestSpawnPotential;
    public float slimeNestCoreDropRate;

    public float idleUtility;

    public ItemInfo[] items;
    public RecipeInfo[] recipes;

    public float followingCameraSmoothness;

    public int CalculateLevel(int experience) {
	return (int) Mathf.Log(experience / 10 + 1, 2) + 1;
    }

    public float CalculateMaxHealth(int level) {
	return 10 + 2.5f * level;
    }

    public float CalculateBaseSpeed(int level) {
	return 6 - 1.5f / level;
    }

    public float CalculateBaseDamage(int level) {
	return 5 - 4f / level;
    }

    public int CalculateRoomCount(int level) {
	return 3 + (int) Mathf.Sqrt(level) * 2;
    }

    public int CalculateRoomSpawnPotential(int level) {
	return (int) (10 * Mathf.Sqrt(level));
    }

    public float CalculateSpeedPotionMultiplier(
	int level, bool positive
    ) {
	float result = 2.5f - 1.3f / Mathf.Sqrt(level + 1);
	if (!positive) {
	    result = 1 / result;
	}
	return result;
    }

    public float CalculateRegenerationPotionRate(
	int level, bool positive
    ) {
	float result = 5 - 4f / (1 + level);
	if (!positive) {
	    result *= -1;
	}
	return result;
    }

    public float CalculatePotionDuration(int level) {
	return 5 - 4f / (1 + level);
    }

    public ItemInfo FindItemInfo(string id) {
	foreach (ItemInfo itemInfo in items) {
	    if (itemInfo.id == id) {
		return itemInfo;
	    }
	}

	throw new Exception($"No item with ID \"{id}\"");
    }
}
