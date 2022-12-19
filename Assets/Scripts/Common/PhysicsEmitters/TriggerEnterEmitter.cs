using UnityEngine;
using Leopotam.EcsLite;

public class TriggerEnterEmitter : MonoBehaviour {
    public EcsWorld world;

    private EntityInfo _entityInfo;

    void OnTriggerEnter2D(Collider2D other) {
	if (_entityInfo == null) {
	    _entityInfo = GetComponent<EntityInfo>();
	}

	int entity = _entityInfo.entity;
	int? otherEntity = other.GetComponent<EntityInfo>()?.entity;

	EntityBuilder.BuildRequest(
	    world, new TriggerEnterEvent(entity, otherEntity)
	);
    }
}
