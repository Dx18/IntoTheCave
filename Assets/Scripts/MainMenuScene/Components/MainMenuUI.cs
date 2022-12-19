namespace MainMenu {
    public struct MainMenuUI {
	public int[] recipeInputSlots;
	public int[] playerItemsSlots;
	public int selectedRecipe;
	public bool updateRequired;

	public MainMenuUI(
	    int[] recipeInputSlots, int[] playerItemsSlots
	) {
	    this.recipeInputSlots = recipeInputSlots;
	    this.playerItemsSlots = playerItemsSlots;
	    this.selectedRecipe = 0;
	    this.updateRequired = false;
	}
    }
}
