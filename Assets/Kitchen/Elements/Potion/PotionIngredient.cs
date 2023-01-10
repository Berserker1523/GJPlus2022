namespace Kitchen
{
    public struct PotionIngredient
    {
        public IngredientData data;
        public CookingToolName usedCookingTool;

        public PotionIngredient(IngredientData data, CookingToolName usedCookingTool)
        {
            this.data = data;
            this.usedCookingTool = usedCookingTool;
        }
    }
}
