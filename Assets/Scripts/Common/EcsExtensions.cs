using System;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public static class EcsExtensions {
    public static IEcsSystems AddAll(
	this IEcsSystems systems, IEnumerable<IEcsSystem> newSystems
    ) {
	foreach (IEcsSystem system in newSystems) {
	    systems.Add(system);
	}

	return systems;
    }

    public static void DeleteAll<T>(this EcsWorld world)
    where T : struct {
	var pool = world.GetPool<T>();
	var transformPool = world.GetPool<TransformInfo>();
	var rigidbodyPool = world.GetPool<RigidbodyInfo>();

	var entities = world.Filter<T>().End();
	foreach (int entity in entities) {
	    pool.Del(entity);
	}
    }

    public static void DeleteAllWith<T>(this EcsWorld world)
    where T : struct {
	var transformPool = world.GetPool<TransformInfo>();
	var rigidbodyPool = world.GetPool<RigidbodyInfo>();

	var entities = world.Filter<T>().End();
	foreach (int entity in entities) {
	    GameObject gameObject = null;
	    if (transformPool.Has(entity)) {
		gameObject = transformPool.Get(entity).gameObject;
	    } else if (rigidbodyPool.Has(entity)) {
		gameObject = rigidbodyPool.Get(entity).gameObject;
	    }

	    if (gameObject != null) {
		GameObject.Destroy(gameObject);
	    }

	    world.DelEntity(entity);
	}
    }

    public static void AddInitialized<T>(
	this EcsPool<T> pool, int entity, T component
    ) where T : struct {
	pool.Add(entity) = component;
    }

    public static int FindFirstEntityWith<T>(this EcsWorld world)
    where T : struct {
	var entities = world.Filter<T>().End();
	foreach (int entity in entities) {
	    return entity;
	}

	throw new Exception(
	    $"No entities with component {typeof(T).FullName}"
	);
    }

    public static ref T FindFirstComponent<T>(this EcsWorld world)
    where T : struct {
	var pool = world.GetPool<T>();
	return ref pool.Get(world.FindFirstEntityWith<T>());
    }
}
