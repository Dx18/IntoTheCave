using Leopotam.EcsLite;

namespace Game {
    public sealed class ExperienceOnDeathSystem : IEcsRunSystem {
	public void Run(IEcsSystems systems) {
	    EcsWorld world = systems.GetWorld();

	    var experienceOnDeathPool = world
		.GetPool<ExperienceOnDeath>();

	    ref GameStats gameStats = ref world
		.FindFirstComponent<GameStats>();

	    var entities = world
		.Filter<GameAgentDiedTag>()
		.Inc<ExperienceOnDeath>()
		.End();
	    foreach (int entity in entities) {
		ref ExperienceOnDeath experience =
		    ref experienceOnDeathPool.Get(entity);

		gameStats.experienceGained += experience.experience;
	    }
	}
    }
}
