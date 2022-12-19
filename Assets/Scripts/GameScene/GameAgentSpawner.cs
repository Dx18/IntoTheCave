using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;

using Random = UnityEngine.Random;

namespace Game {
    public class GameAgentSpawner {
	private ItemSpawner _itemSpawner;

	public GameAgentSpawner(ItemSpawner itemSpawner) {
	    _itemSpawner = itemSpawner;
	}

	public int SpawnPlayer(
	    EcsWorld world, Vector2 position, GameSharedData data
	) {
	    int level = data.rules.CalculateLevel(
		data.playerData.experience
	    );
	    int?[] items = new int?[data.rules.inventoryCapacity];
	    for (int i = 0; i < items.Length; ++i) {
		if (data.playerData.items[i] != null) {
		    items[i] = _itemSpawner.SpawnItem(
			world, data.playerData.items[i], data
		    );
		}
	    }

	    int entity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.playerPrefab, position
		)
		.With(new Position(position))
		.With(new Velocity(Vector2.zero))
		.With(new Speed(data.rules.CalculateBaseSpeed(level)))
		.With(new Damage(data.rules.CalculateBaseDamage(level)))
		.With(new Team(AgentTeam.Player))
		.With(new Inventory(items))
		.With(new Health(data.rules.CalculateMaxHealth(level)))
		.With(new Player())
		.Build();

	    InstantiateBehaviour(
		world, entity, new InputMovementBehaviour(data.input)
	    );
	    InstantiateBehaviour(
		world, entity, new InputMeleeAttackBehaviour()
	    );
	    InstantiateBehaviour(
		world, entity, new InputItemPickupBehaviour()
	    );
	    InstantiateBehaviour(
		world, entity, new InputItemDropBehaviour()
	    );
	    InstantiateBehaviour(
		world, entity, new InputItemUseBehaviour()
	    );
	    InstantiateBehaviour(
		world, entity, new IdleBehaviour()
	    );

	    return entity;
	}

	public int SpawnRat(
	    EcsWorld world, Vector2 position, GameSharedData data
	) {
	    int entity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.ratPrefab, position
		)
		.With(new Position(position))
		.With(new Velocity(Vector2.zero))
		.With(new Speed(data.rules.ratBaseSpeed))
		.With(new Damage(data.rules.ratBaseDamage))
		.With(new Vision(data.rules.visionDistance))
		.With(new Team(AgentTeam.Monster))
		.With(new TrackedTarget(null))
		.With(new Health(data.rules.ratMaxHealth))
		.With(new ExperienceOnDeath(data.rules.ratExperience))
		.Build();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    ConfigureHealthBar(
		world, entity,
		rigidbodyPool.Get(entity).gameObject.transform
	    );

	    if (Random.value < data.rules.ratTailDropRate) {
		world.GetPool<LootOnDeath>().AddInitialized(
		    entity, new LootOnDeath(
			_itemSpawner.SpawnItem(
			    world, "ratTail", data
			)
		    )
		);
	    }

	    InstantiateBehaviour(
		world, entity, new ApproachBehaviour(
		    data.rules.ratAttackDistance
		)
	    );
	    InstantiateBehaviour(
		world, entity, new MeleeAttackBehaviour(
		    data.rules.ratAttackDistance,
		    data.rules.ratAttackCooldown
		)
	    );
	    InstantiateBehaviour(
		world, entity, new IdleBehaviour()
	    );

	    return entity;
	}

	public int SpawnStonethrower(
	    EcsWorld world, Vector2 position, GameSharedData data
	) {
	    int entity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.stonethrowerPrefab, position
		)
		.With(new Position(position))
		.With(new Velocity(Vector2.zero))
		.With(new Speed(data.rules.stonethrowerBaseSpeed))
		.With(new Damage(data.rules.stonethrowerBaseDamage))
		.With(new Vision(data.rules.visionDistance))
		.With(new Team(AgentTeam.Monster))
		.With(new TrackedTarget(null))
		.With(new Health(data.rules.stonethrowerMaxHealth))
		.With(new ExperienceOnDeath(data.rules.stonethrowerExperience))
		.Build();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    ConfigureHealthBar(
		world, entity,
		rigidbodyPool.Get(entity).gameObject.transform
	    );

	    if (Random.value < data.rules.stonethrowerScalesDropRate) {
		world.GetPool<LootOnDeath>().AddInitialized(
		    entity, new LootOnDeath(
			_itemSpawner.SpawnItem(
			    world, "stonethrowerScales", data
			)
		    )
		);
	    }

	    InstantiateBehaviour(
		world, entity, new ApproachBehaviour(
		    data.rules.stonethrowerMaxDistance
		)
	    );
	    InstantiateBehaviour(
		world, entity, new FleeBehaviour(
		    data.rules.stonethrowerMinDistance
		)
	    );
	    InstantiateBehaviour(
		world, entity, new StoneThrowBehaviour(
		    data.rules.stonethrowerAttackCooldown
		)
	    );
	    InstantiateBehaviour(
		world, entity, new IdleBehaviour()
	    );

	    return entity;
	}

	public int SpawnAlchemist(
	    EcsWorld world, Vector2 position, GameSharedData data
	) {
	    int entity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.alchemistPrefab, position
		)
		.With(new Position(position))
		.With(new Velocity(Vector2.zero))
		.With(new Speed(data.rules.alchemistBaseSpeed))
		.With(new Vision(data.rules.visionDistance))
		.With(new Team(AgentTeam.Monster))
		.With(new TrackedTarget(null))
		.With(new Health(data.rules.alchemistMaxHealth))
		.With(new ExperienceOnDeath(data.rules.alchemistExperience))
		.Build();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    ConfigureHealthBar(
		world, entity,
		rigidbodyPool.Get(entity).gameObject.transform
	    );

	    if (Random.value < data.rules.alchemistPotionDropRate) {
		string[] potions = new string[] {
		    "speedPotion", "regenerationPotion"
		};
		string id = potions[Random.Range(0, potions.Length)];

		world.GetPool<LootOnDeath>().AddInitialized(
		    entity, new LootOnDeath(
			_itemSpawner.SpawnItem(world, id, data)
		    )
		);
	    }

	    InstantiateBehaviour(
		world, entity, new ApproachBehaviour(
		    data.rules.alchemistMaxDistance
		)
	    );
	    InstantiateBehaviour(
		world, entity, new FleeBehaviour(
		    data.rules.alchemistMinDistance
		)
	    );
	    InstantiateBehaviour(
		world, entity, new PotionThrowBehaviour(
		    data.rules.alchemistAttackCooldown
		)
	    );
	    InstantiateBehaviour(
		world, entity, new IdleBehaviour()
	    );

	    return entity;
	}

	public int SpawnSlime(
	    EcsWorld world, Vector2 position, GameSharedData data
	) {
	    int entity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.slimePrefab, position
		)
		.With(new Position(position))
		.With(new Velocity(Vector2.zero))
		.With(new Speed(data.rules.slimeBaseSpeed))
		.With(new Damage(data.rules.slimeBaseDamage))
		.With(new Vision(data.rules.visionDistance))
		.With(new Team(AgentTeam.Monster))
		.With(new TrackedTarget(null))
		.With(new Health(data.rules.slimeMaxHealth))
		.With(new ExperienceOnDeath(data.rules.slimeExperience))
		.Build();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    ConfigureHealthBar(
		world, entity,
		rigidbodyPool.Get(entity).gameObject.transform
	    );

	    if (Random.value < data.rules.slimeBallDropRate) {
		world.GetPool<LootOnDeath>().AddInitialized(
		    entity, new LootOnDeath(
			_itemSpawner.SpawnItem(
			    world, "slimeBall", data
			)
		    )
		);
	    }

	    InstantiateBehaviour(
		world, entity, new ApproachBehaviour(
		    data.rules.slimeAttackDistance
		)
	    );
	    InstantiateBehaviour(
		world, entity, new SlimeAttackBehaviour(
		    data.rules.slimeAttackDistance,
		    data.rules.slimeSpeedMultiplier,
		    data.rules.slimeSpeedReduceDuration,
		    data.rules.slimeAttackCooldown
		)
	    );
	    InstantiateBehaviour(
		world, entity, new IdleBehaviour()
	    );

	    return entity;
	}

	public int SpawnSlimeNest(
	    EcsWorld world, Vector2 position,
	    GameAgentSpawner gameAgentSpawner, GameSharedData data
	) {
	    int entity = EntityBuilder
		.BeginInstantiated(
		    world, data.prefabs.slimeNestPrefab, position
		)
		.With(new Position(position))
		.With(new Vision(data.rules.visionDistance))
		.With(new Team(AgentTeam.Monster))
		.With(new TrackedTarget(null))
		.With(new Health(data.rules.slimeNestMaxHealth))
		.With(new ExperienceOnDeath(data.rules.slimeNestExperience))
		.Build();

	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    ConfigureHealthBar(
		world, entity,
		rigidbodyPool.Get(entity).gameObject.transform
	    );

	    if (Random.value < data.rules.slimeNestCoreDropRate) {
		world.GetPool<LootOnDeath>().AddInitialized(
		    entity, new LootOnDeath(
			_itemSpawner.SpawnItem(
			    world, "slimeNestCore", data
			)
		    )
		);
	    }

	    InstantiateBehaviour(
		world, entity, new SlimeSpawnBehaviour(
		    data.rules.slimeNestSpawnCooldown,
		    gameAgentSpawner
		)
	    );

	    return entity;
	}

	private void ConfigureHealthBar(
	    EcsWorld world, int entity, Transform transform
	) {
	    Image healthBarFill = transform
		.Find("HealthUI/HealthBar/HealthBarFill")
		.GetComponent<Image>();
	    world.GetPool<HealthBar>().AddInitialized(
		entity, new HealthBar(healthBarFill)
	    );
	}

	private void InstantiateBehaviour<T>(
	    EcsWorld world, int entity, T behaviour
	) where T : struct {
	    EntityBuilder.BeginEmpty(world)
		.With(new AgentBehaviour(entity))
		.With(behaviour);
	}
    }
}
