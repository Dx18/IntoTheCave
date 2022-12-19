namespace Game {
    public struct AgentBehaviour {
	public int entity;
	public float utility;
	public bool isActive;

	public AgentBehaviour(int entity) {
	    this.entity = entity;
	    this.utility = 0;
	    this.isActive = false;
	}
    }
}
