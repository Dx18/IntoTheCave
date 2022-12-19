using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Unity.Ugui;

namespace Game {
    public sealed class HealthUISystem : IEcsRunSystem {
	private EcsUguiEmitter _uguiEmitter;

	[EcsUguiNamed("HealthBarFill")]
	private Image _healthBarFill;

	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var healthBarPool = world.GetPool<HealthBar>();
	    var healthPool = world.GetPool<Health>();

	    _healthBarFill.fillAmount = 0;

	    var players = world.Filter<Player>().End();
	    foreach (int playerEntity in players) {
		ref Health health = ref healthPool.Get(playerEntity);

		_healthBarFill.fillAmount = Mathf.Clamp(
		    health.health, 0, health.maxHealth
		) / health.maxHealth;
	    }

	    var entities = world
		.Filter<HealthBar>()
		.Inc<Health>()
		.End();
	    foreach (int entity in entities) {
		Image fill = healthBarPool.Get(entity).fill;
		ref Health health = ref healthPool.Get(entity);

		fill.fillAmount = Mathf.Clamp(
		    health.health, 0, health.maxHealth
		) / health.maxHealth;
	    }
	}
    }
}
