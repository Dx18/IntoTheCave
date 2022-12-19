using UnityEngine;
using UnityEngine.InputSystem;
using Leopotam.EcsLite;

namespace Game {
    public struct InputMovementImplementation
	: IBehaviourImplementation
    {
	public EcsFilter GetBehaviourFilter(EcsWorld world) {
	    return world.Filter<InputMovementBehaviour>().End();
	}

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputMovementBehaviour>()
		.Get(behaviourEntity);

	    InputAction move = behaviour.input.actions["Move"];
	    Vector2 direction = move.ReadValue<Vector2>();
	    return direction != Vector2.zero ? 1 : 0;
	}

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	) {
	    ref var behaviour = ref world
		.GetPool<InputMovementBehaviour>()
		.Get(behaviourEntity);

	    ref Velocity velocity = ref world
		.GetPool<Velocity>().Get(entity);
	    float speed = world
		.GetPool<Speed>().Get(entity).speed;

	    InputAction move = behaviour.input.actions["Move"];
	    Vector2 direction = move.ReadValue<Vector2>();

	    velocity.velocity = direction * speed;
	}
    }
}
