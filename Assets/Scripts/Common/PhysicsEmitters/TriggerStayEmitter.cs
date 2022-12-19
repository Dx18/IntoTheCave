using UnityEngine;
using Leopotam.EcsLite;

public class TriggerStayEmitter : MonoBehaviour {
    public EcsWorld world;

    private EntityInfo _entityInfo;

    void OnTriggerStay2D(Collider2D other) {
	if (_entityInfo == null) {
	    _entityInfo = GetComponent<EntityInfo>();
	}

	int entity = _entityInfo.entity;
	int? otherEntity = other.GetComponent<EntityInfo>()?.entity;

	EntityBuilder.BuildRequest(
	    world, new TriggerStayEvent(entity, otherEntity)
	);
    }
}
