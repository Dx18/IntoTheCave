using UnityEngine;
using Leopotam.EcsLite;

namespace Game {
    public sealed class SpeedBuffSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var speedBuffPool = world.GetPool<SpeedBuff>();
	    var speedPool = world.GetPool<Speed>();

	    var speedBuffs = world.Filter<SpeedBuff>().End();
	    foreach (int buffEntity in speedBuffs) {
		ref SpeedBuff buff = ref speedBuffPool
		    .Get(buffEntity);

		buff.duration -= Time.deltaTime;

		if (buff.duration <= 0) {
		    speedBuffPool.Del(buffEntity);
		}
	    }

	    var validSpeedBuffs = world
		.Filter<SpeedBuff>()
		.Inc<Speed>()
		.End();
	    foreach (int buffEntity in validSpeedBuffs) {
		ref SpeedBuff buff = ref speedBuffPool.Get(buffEntity);
		ref Speed speed = ref speedPool.Get(buffEntity);

		speed.speed *= buff.multiplier;
	    }
	}
    }
}
