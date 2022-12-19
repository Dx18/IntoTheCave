public struct TriggerExitEvent {
    public int entity;
    public int? otherEntity;

    public TriggerExitEvent(int entity, int? otherEntity) {
	this.entity = entity;
	this.otherEntity = otherEntity;
    }
}
