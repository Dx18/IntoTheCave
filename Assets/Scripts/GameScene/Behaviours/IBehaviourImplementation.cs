using Leopotam.EcsLite;

namespace Game {
    public interface IBehaviourImplementation {
	public EcsFilter GetBehaviourFilter(EcsWorld world);

	public float Evaluate(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	);

	public void Apply(
	    EcsWorld world, int entity, int behaviourEntity,
	    GameSharedData data
	);
    }
}
