namespace Game {
    public struct PotionThrowBehaviour {
	public float cooldown;
	public float baseCooldown;

	public PotionThrowBehaviour(float baseCooldown) {
	    this.cooldown = baseCooldown;
	    this.baseCooldown = baseCooldown;
	}
    }
}
