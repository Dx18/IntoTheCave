namespace Game {
    public struct SlimeSpawnBehaviour {
	public float cooldown;
	public float baseCooldown;
	public GameAgentSpawner spawner;

	public SlimeSpawnBehaviour(
	    float baseCooldown, GameAgentSpawner spawner
	) {
	    this.cooldown = baseCooldown;
	    this.baseCooldown = baseCooldown;
	    this.spawner = spawner;
	}
    }
}
