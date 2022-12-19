using UnityEngine;
using UnityEngine.InputSystem;
using Leopotam.EcsLite;

namespace Game {
    public sealed class PlayerInputSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();
	    GameSharedData data = systems.GetShared<GameSharedData>();

	    InputAction meleeAttack = data.input.actions["Attack"];
	    if (meleeAttack.WasPressedThisFrame()) {
		EntityBuilder.BuildRequest(
		    world, new MeleeAttackInputRequest {}
		);
	    }

	    InputAction itemPickup = data.input.actions["ItemPickup"];
	    if (itemPickup.WasPressedThisFrame()) {
		EntityBuilder.BuildRequest(
		    world, new ItemPickupInputRequest {}
		);
	    }

	    InputAction itemDrop = data.input.actions["ItemDrop"];
	    InputAction itemUse = data.input.actions["ItemUse"];
	    if (itemDrop.WasPressedThisFrame()) {
		EntityBuilder.BuildRequest(
		    world, new ItemDropInputRequest {
			slot = (int) itemDrop.ReadValue<float>()
		    }
		);
	    } else if (itemUse.WasPressedThisFrame()) {
		EntityBuilder.BuildRequest(
		    world, new ItemUseInputRequest {
			slot = (int) itemUse.ReadValue<float>()
		    }
		);
	    }
	}
    }
}
