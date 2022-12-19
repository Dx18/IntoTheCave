public struct TriggerEnterEvent {
    public int entity;
    public int? otherEntity;

    public TriggerEnterEvent(int entity, int? otherEntity) {
	this.entity = entity;
	this.otherEntity = otherEntity;
    }
}
