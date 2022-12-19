namespace Game {
    public struct SlimeAttackBehaviour {
	public float attackDistance;
	public float speedMultiplier;
	public float duration;
	public float cooldown;
	public float baseCooldown;

	public SlimeAttackBehaviour(
	    float attackDistance, float speedMultiplier,
	    float duration, float baseCooldown
	) {
	    this.attackDistance = attackDistance;
	    this.speedMultiplier = speedMultiplier;
	    this.duration = duration;
	    this.cooldown = baseCooldown;
	    this.baseCooldown = baseCooldown;
	}
    }
}
