namespace Game {
    public struct FollowingCamera {
	public int target;
	public float smoothness;

	public FollowingCamera(int target, float smoothness) {
	    this.target = target;
	    this.smoothness = smoothness;
	}
    }
}
