public struct TriggerStayEvent {
    public int entity;
    public int? otherEntity;

    public TriggerStayEvent(int entity, int? otherEntity) {
	this.entity = entity;
	this.otherEntity = otherEntity;
    }
}
