using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class PositionSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var velocityPool = world.GetPool<Velocity>();
	    var positionPool = world.GetPool<Position>();
	    var rigidbodyPool = world.GetPool<RigidbodyInfo>();
	    var transformPool = world.GetPool<TransformInfo>();

	    var velocities = world.Filter<Velocity>().End();
	    foreach (int entity in velocities) {
		Vector2 velocity = velocityPool.Get(entity).velocity;
		ref Position position = ref positionPool.Get(entity);

		if (rigidbodyPool.Has(entity)) {
		    Rigidbody2D rigidbody = rigidbodyPool
			.Get(entity).rigidbody;

		    if (rigidbody.bodyType != RigidbodyType2D.Static) {
			rigidbody.velocity = velocity;
		    }

		    position.position = rigidbody.position;
		} else {
		    Transform transform = transformPool
			.Get(entity).transform;

		    position.position += velocity * Time.deltaTime;

		    Vector3 finalPosition = new Vector3(
			position.position.x,
			position.position.y,
			transform.position.z
		    );
		    transform.position = finalPosition;
		}
	    }

	    var positions = world
		.Filter<Position>()
		.Exc<Velocity>()
		.End();
	    foreach (int entity in positions) {
		ref Position position = ref positionPool.Get(entity);

		if (rigidbodyPool.Has(entity)) {
		    Rigidbody2D rigidbody = rigidbodyPool
			.Get(entity).rigidbody;

		    if (rigidbody.bodyType != RigidbodyType2D.Static) {
			rigidbody.MovePosition(position.position);
		    }

		    position.position = rigidbody.position;
		} else {
		    Transform transform = transformPool
			.Get(entity).transform;

		    Vector3 finalPosition = new Vector3(
			position.position.x,
			position.position.y,
			transform.position.z
		    );
		    transform.position = finalPosition;
		}
	    }
	}
    }
}
