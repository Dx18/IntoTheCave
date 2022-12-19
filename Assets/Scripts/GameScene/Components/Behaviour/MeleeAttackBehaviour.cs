namespace Game {
    public struct MeleeAttackBehaviour {
	public float attackDistance;
	public float cooldown;
	public float baseCooldown;

	public MeleeAttackBehaviour(
	    float attackDistance, float baseCooldown
	) {
	    this.attackDistance = attackDistance;
	    this.cooldown = baseCooldown;
	    this.baseCooldown = baseCooldown;
	}
    }
}
