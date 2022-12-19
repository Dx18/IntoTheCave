using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class RegenerationBuffSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var regenerationBuffPool = world
		.GetPool<RegenerationBuff>();
	    var healthPool = world.GetPool<Health>();

	    var regenerationBuffs = world
		.Filter<RegenerationBuff>()
		.End();
	    foreach (int buffEntity in regenerationBuffs) {
		ref RegenerationBuff buff = ref regenerationBuffPool
		    .Get(buffEntity);

		buff.duration -= Time.deltaTime;

		if (buff.duration <= 0) {
		    regenerationBuffPool.Del(buffEntity);
		}
	    }

	    var validRegenerationBuffs = world
		.Filter<RegenerationBuff>()
		.Inc<Health>()
		.End();
	    foreach (int buffEntity in validRegenerationBuffs) {
		ref RegenerationBuff buff = ref regenerationBuffPool
		    .Get(buffEntity);
		ref Health health = ref healthPool.Get(buffEntity);

		health.health += buff.rate * Time.deltaTime;
	    }
	}
    }
}
