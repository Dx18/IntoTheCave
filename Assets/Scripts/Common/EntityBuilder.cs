using System;
using UnityEngine;
using Leopotam.EcsLite;

public struct EntityBuilder {
    private EcsWorld _world;
    private int _entity;

    private GameObject _gameObject;

    private EntityBuilder(
	EcsWorld world, int entity, GameObject gameObject
    ) {
	_world = world;
	_entity = entity;
	_gameObject = gameObject;
    }

    public static EntityBuilder BeginEmpty(EcsWorld world) {
	return new EntityBuilder(world, world.NewEntity(), null);
    }

    public static EntityBuilder BeginInstantiated(
	EcsWorld world, GameObject prefab, Vector2 position
    ) {
	GameObject instance = GameObject.Instantiate(
	    prefab, new Vector3(position.x, position.y, 0),
	    Quaternion.identity
	);

	EntityBuilder builder = new EntityBuilder(
	    world, world.NewEntity(), instance
	);

	EntityInfo entityInfo = instance.AddComponent<EntityInfo>();
	entityInfo.entity = builder._entity;

	Rigidbody2D rigidbody = instance.GetComponent<Rigidbody2D>();
	if (rigidbody != null) {
	    builder.With(new RigidbodyInfo {
		    gameObject = instance,
		    rigidbody = rigidbody
		});
	} else {
	    builder.With(new TransformInfo {
		    gameObject = instance,
		    transform = instance.transform
		});
	}

	return builder;
    }

    public static int BuildRequest<T>(EcsWorld world, T component)
    where T : struct {
	return EntityBuilder.BeginEmpty(world)
	    .With(component)
	    .Build();
    }

    public EntityBuilder With<T>(T component)
    where T : struct {
	_world.GetPool<T>().Add(_entity) = component;
	return this;
    }

    public EntityBuilder With<T>()
    where T : struct {
	_world.GetPool<T>().Add(_entity);
	return this;
    }

    public EntityBuilder WithMonoBehaviour<T>(Action<T> init)
    where T : MonoBehaviour {
	init(_gameObject.AddComponent<T>());
	return this;
    }

    public EntityBuilder WithMonoBehaviour<T>()
    where T : MonoBehaviour {
	_gameObject.AddComponent<T>();
	return this;
    }

    public int Build() {
	return _entity;
    }
}
