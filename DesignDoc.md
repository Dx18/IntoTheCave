# Design doc: "Into the Cave!"

## Game

**Brief description:** rogue-like game about a person called Sam
exploring the caves.

### Player

Player has experience which affects the levels of max health, base
damage and speed.  Also, all ingridients collected by player are saved
and can be used to brew potions.

Player can brew potions before entering the dungeon.

During the game player has inventory with items which can be
collected by killing monsters.  Possible types of items:

1. Potions: can be used;
2. Potion ingridients: can be used to brew potions for the next
   session.

### Potions

1. Speed.  Ingridients: slime ball, slime nest core;
2. Regeneration.  Ingridients: rat tail, stonethrower scales.

### Monster types

1. Rat
   
   Short-range enemy.  Approaches and attacks.
   
   **Drops:** rat tail.

2. Stonethrower

   Long-range enemy throwing stones.  Keeps distance and attacks.
    
   **Drops:** stonethrower scales.

3. Alchemist

   Long-enemy enemy throwing potions.  Keeps distance and attacks.
    
   **Drops:** potions.

4. Slime

   Short-range enemy.  Approaches and attacks.  Slows player down.
    
   **Drops:** slime ball.

5. Slime nest

   Spawning enemy.  Does not move.  Spawns slimes if sees player.
    
   **Drops:** slime nest core.

### Controls

1.  Main menu

    **Switch recipe:** `A`, `D`.
    
    **Craft:** `Space`.
    
    **Start game:**
    
    **Movement:** `W`, `A`, `S`, `D`.
    
    **Attack:** `Enter`.
    
    **Item pickup:** `Space`.
    
    **Item drop:** `Shift` + `0`-`9`.
    
    **Item use:** `0`-`9`.

## Architecture / technical information

### `MonoBehaviour` components

**EntityInfo:** `entity`.

**TriggerEnterEmitter:** `world`.

**TriggerStayEmitter:** `world`.

**TriggerExitEmitter:** `world`.

### Common components

Unity generic data:

1. **TransformInfo:** `gameObject`, `transform`;
2. **RigidbodyInfo:** `gameObject`, `rigidbody`.

Generic data:

1. **Position:** `position` (requires either `TransformInfo` or
   `RigidbodyInfo` component).
2. **Velocity:** `velocity` (requires `Position` component).

Physics:

1. **TriggerEnterEvent:** `entity`, `otherEntity`;
2. **TriggerStayEvent:** `entity`, `otherEntity`;
3. **TriggerExitEvent:** `entity`, `otherEntity`.

UI:

1. **SlotUI:** `item`.

### Main menu scene components

UI:

1. **MainMenuUI:** `recipeInputSlots`, `playerItemsSlots`,
   `selectedRecipe`, `updateRequired`.

### Main game scene components

Game agent generic data:

1.  **Health:** `health`, `maxHealth`;
2.  **Speed:** `speed`, `baseSpeed`;
3.  **Damage:** `damage`, `baseDamage`;
4.  **Vision:** `distance`;
5.  **Team:** `team`.

Helper game agent data:

1.  **TrackedTarget:** `target` (requires `Vision`, `Position`
    components).

Agent behaviour (all require `AgentBehaviour` on current entity):

1. **AgentBehaviour:** `entity`, `utility`, `isActive`;
2. **IdleBehaviour:** none.
    
   Component requirements (applied entity):
    
   -   `Velocity`;

3. **InputMovementBehaviour:** `input`.
    
   Component requirements (applied entity):
    
   -   `Velocity`;
   -   `Speed`;

4. **InputMeleeAttackBehaviour:** none.
    
   Component requirements (applied entity):
    
   -   `Damage`;
   -   `Team`;

5. **InputItemPickupBehaviour:** none.
    
   Component requirements (applied entity):
    
   -   `Inventory`;

6. **InputItemDropBehaviour:** none.
    
   Component requirements (applied entity):
    
   -   `Inventory`;

7. **InputItemUseBehaviour:** none.
    
   Component requirements (applied entity):
    
   -   `Inventory`;

8. **ApproachBehaviour:** `distance`.
    
   Component requirements (applied entity):
    
   -   `Velocity`;
   -   `Speed`;
   -   `Vision`;
   -   `Team`;

9. **FleeBehaviour:** `distance`.
    
   Component requirements (applied entity):
    
   -   `Velocity`;
   -   `Speed`;
   -   `Vision`;
   -   `Team`;

10. **MeleeAttackBehaviour:** `attackDistance`, `cooldown`,
    `baseCooldown`.
    
    Component requirements:
    
    -   `Damage`;
    -   `Vision`;
    -   `Team`;

11. **StoneThrowBehaviour:** `cooldown`, `baseCooldown`.
    
    Component requirements:
    
    -   `Damage`;
    -   `Vision`;
    -   `Team`;

12. **PotionThrowBehaviour:** `cooldown`, `baseCooldown`.
    
    Component requirements:
    
    -   `Vision`;
    -   `Team`;

13. **SlimeAttackBehaviour:** `attackDistance`, `speedMultiplier`,
    `duration`, `cooldown`, `baseCooldown`.
    
    Component requirements:
    
    -   `Damage`;
    -   `Vision`;
    -   `Team`;

14. **SlimeSpawnBehaviour:** `cooldown`, `baseCooldown`, `spawner`.
    
    Component requirements:
    
    -   `Vision`;
    -   `Team`.

Player-specific data:

1.  **Player:** none.

Death-related data and tags:

1.  **LootOnDeath:** `item`;
2.  **ExperienceOnDeath:** `experience`;
3.  **GameAgentDiedTag:** none.

Projectile-related data and tags:

1.  **ThrownStone:** `damage`;
2.  **ThrownPotion:** `targetPosition`;
3.  **PotionReachedTargetTag:** none.

Items and inventory:

1.  **Item:** `id`, `name`, `sprite`;
2.  **Inventory:** `items`;
3.  **DroppedItem:** none;
4.  **UsableItem:** none.
5.  **ItemUsedTag:** none;

Singletone game state:

1.  **GameStats:** `experienceGained`.

Buffs:

1.  **RegenerationBuff:** `rate`, `duration`;
2.  **SpeedBuff:** `multiplier`, `duration`.

Potions:

1.  **RegenerationPotion:** `rate`, `duration`;
2.  **SpeedPotion:** `multiplier`, `duration`.

Other data:

1. **FollowingCamera:** `target`, `smoothness`.

Data related to game finishing:

1. **ExitZone:** `rectangle`.

Request:

1. **MeleeAttackInputRequest:** none;
2. **ItemPickupInputRequest:** none;
3. **ItemDropInputRequest:** `slot`;
4. **ItemUseInputRequest:** `slot`.

UI:

1. **HealthBar:** `fill`;
2. **InventoryUI:** `slots`.

### Main game scene entity structure

Entities on game level:

1.  Game agents: player and monsters;
2.  Behaviours of game agents;
3.  Dropped items: potions and ingridients. Can be picked up by
    player;
4.  Thrown potions. Affect player on certain radius;
5.  Thrown stones. Damage player on hit.

Additional game entities:

1.  Singletone game state;
2.  Requests.

Game agents are entities with sets of components:

1.  **Player:** `RigidbodyInfo`, `Position`, `Velocity`, `Speed`,
    `Damage`, `Team`, `Inventory`, `Health`, `Player`, buffs;
2.  **Rat:** `RigidbodyInfo`, `Position`, `Velocity`, `Speed`,
    `Damage`, `Vision`, `Team`, `TrackedTarget`, `Health`,
    `ExperienceOnDeath`, `LootOnDeath` (optional), buffs;
3.  **Stonethrower:** `RigidbodyInfo`, `Position`, `Velocity`,
    `Speed`, `Damage`, `Vision`, `Team`, `TrackedTarget`, `Health`,
    `ExperienceOnDeath`, `LootOnDeath` (optional), buffs;
4.  **Alchemist:** `RigidbodyInfo`, `Position`, `Velocity`, `Speed`,
    `Vision`, `Team`, `TrackedTarget`, `Health`, `ExperienceOnDeath`,
    `LootOnDeath` (optional), buffs;
5.  **Slime:** `RigidbodyInfo`, `Position`, `Velocity`, `Speed`,
    `Damage`, `Vision`, `Team`, `TrackedTarget`, `Health`,
    `ExperienceOnDeath`, `LootOnDeath` (optional), buffs;
6.  **Slime nest:** `RigidbodyInfo`, `Position`, `Vision`, `Team`,
    `TrackedTarget`, `Health`, `ExperienceOnDeath`, `LootOnDeath`
    (optional), buffs.

One or more game agent behaviours correspond to one game agent.
Each **Agent behaviour** entity has `AgentBehaviour` component and
exactly one actual behaviour component.  Possible behaviours:

1.  **Player:** `InputMovementBehaviour`, `InputMeleeAttackBehaviour`,
    `InputItemPickupBehaviour`, `InputItemDropBehaviour`,
    `InputItemUseBehaviour`, `IdleBehaviour`;
2.  **Rat:** `ApproachBehaviour`, `MeleeAttackBehaviour`,
    `IdleBehaviour`;
3.  **Stonethrower:** `ApproachBehaviour`, `FleeBehaviour`,
    `StoneThrowBehaviour`, `IdleBehaviour`;
4.  **Alchemist:** `ApproachBehaviour`, `FleeBehaviour`,
    `PotionThrowBehaviour`, `IdleBehaviour`;
5.  **Slime:** `ApproachBehaviour`, `SlimeAttackBehaviour`,
    `IdleBehaviour`;
6.  **Slime nest:** `SlimeSpawnBehaviour`.

Player's inventory references items.  Item can be in exactly one of
two states:

1. **Inventory item:** `Item`, item-specific components
   (e. g. potions), `ItemUsedTag` (optional). Additionally, item
   is referenced either by inventory or by `LootOnDeath`;
2. **Dropped item:** `Item`, item-specific components
   (e. g. potions), `TransformInfo`, `Position`, `DroppedItem`.

Thrown objects:

1. **Thrown potion:** `ThrownPotion`, potion component,
   `PotionReachedTargetTag` (optional, attached when potion reaches
   its target), `RigidbodyInfo`, `Position`, `Velocity`, `Team`;
2. **Thrown stone:** `ThrownStone`, `RigidbodyInfo`, `Position`,
   `Velocity`.

Singletone game state is entity with the following components:

1. `GameStats.`

Requests: entities with single request component.

### Main game scene systems

Initialization is done in the following systems:

1. **GameSceneInitSystem:** generates map, places agents.

Regular update steps:

1.  Updating input;
2.  Updating UI.

Fixed update steps:

1.  Updating buffs;
2.  Updating behaviours;
3.  Updating projectiles;
4.  Updating used items;
5.  Updating following cameras;
6.  Updating deaths;
7.  Updating dangling components;
8.  Removing dead game agents;
9.  Updating game finish conditions;
10. Clearing input requests;
11. Syncing generic data.

#### Updating input

Running `PlayerInputSystem` to emit input requests
(`MeleeAttackInputRequest`, `ItemPickupInputRequest`,
`ItemDropInputRequest`, `ItemUseInputRequest`).

#### Updating UI

Running system for each UI element (`InventoryUISystem`,
`HealthUISystem`).

#### Updating buffs

Running `BuffsResetSystem` to reset all affected game agent statistics
with base values:
    
1.  `Speed`;
2.  `Damage`.
    
Running system for each buff (`RegenerationBuffSystem`,
`SpeedBuffSystem`).
    
Running `BuffsNormalizeSystem` to clamp all affected game agent
statistics with limited range of values:
    
1.  `Health`.

#### Updating behaviours

Each behaviour has own dedicated `\*Implementation` class implementing
`IBehaviourImplementation` interface. There are two generic:
`BehaviourEvaluateSystem<T>` (for utility evaluation) and
`BehaviourApplySystem<T>` (for behaviour application), where `T`
implements `IBehaviourimplementation`.
    
Game agents updating:
    
1. Running `TargetTrackingSystem` for target tracking handling;
2. Running `BehaviourEvaluateSystem<T>`s for all possible `T`;
3. Running `BehaviourResolveSystem` (to activate and deactivate
   certain behaviours);
4. Running `BehaviourApplySystem`s for all possible `T`;

#### Updating projectiles

Running `ThrownPotionsSystem` for setting `PotionReachedTargetTag`
when potions reach their targets or destroying them on collision;
    
Running `ThrownStonesSystem` for handling stone hits.

#### Updating used items

Running systems dedicated for each type of item.  Then running
`ItemRemoveSystem` to remove remaining used items and potions reached
their targets.

#### Updating following cameras

Running `CameraFollowSystem` to update all following cameras (should
be only one).

#### Updating deaths

Running `DeathTagSystem` to mark all game agents with negative health
as dead (adding `GameAgentDiedTag`).
    
Running `ExperienceOnDeathSystem` to handle experience-dropping
monsters' deaths.
    
Running `LootOnDeathSystem` to handle loot-dropping monsters' deaths.

#### Updating dangling components

Running `DanglingBehaviourSystem` to remove behaviours of game agents
marked as dead.
    
Running `DanglingTrackedTargetSystem` to reset all `TrackedTarget`
components pointing to game agents marked as dead.
    
Running `DanglingFollowingCameraSystem` to remove all cameras
following game agents marked as dead.

#### Removing dead game agents

Running `DeathCleanSystem` remove all game agents marked as dead.

#### Updating game finish conditions

Running `GameFinishSystem` to handle either player reaching the exit
or player dying.

#### Input request clearing

Running `PlayerInputClearSystem` to clear all player input requests.

#### Syncing generic data

Running `PositionSystem` to update all `Position`s according to
`Velocity`s and then `Transform`s and `Rigidbody2D`s according to
`Position`s.

