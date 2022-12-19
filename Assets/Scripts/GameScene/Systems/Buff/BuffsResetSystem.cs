using Leopotam.EcsLite;

namespace Game {
    public sealed class BuffsResetSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var speedPool = world.GetPool<Speed>();
	    var damagePool = world.GetPool<Damage>();

	    var speedEntities = world.Filter<Speed>().End();
	    foreach (int entity in speedEntities) {
		ref Speed speed = ref speedPool.Get(entity);
		speed.speed = speed.baseSpeed;
	    }

	    var damageEntities = world.Filter<Damage>().End();
	    foreach (int entity in damageEntities) {
		ref Damage damage = ref damagePool.Get(entity);
		damage.damage = damage.baseDamage;
	    }
	}
    }
}
