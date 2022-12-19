namespace Game {
    public struct StoneThrowBehaviour {
	public float cooldown;
	public float baseCooldown;

	public StoneThrowBehaviour(float baseCooldown) {
	    this.cooldown = baseCooldown;
	    this.baseCooldown = baseCooldown;
	}
    }
}
