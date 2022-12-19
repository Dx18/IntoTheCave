namespace Game {
    public struct Damage {
	public float damage;
	public float baseDamage;

	public Damage(float baseDamage) {
	    this.damage = baseDamage;
	    this.baseDamage = baseDamage;
	}
    }
}
