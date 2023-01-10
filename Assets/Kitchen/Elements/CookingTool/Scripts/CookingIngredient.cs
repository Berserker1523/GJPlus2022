namespace Kitchen
{
    public class CookingIngredient
    {
        public IngredientData data;
        public IngredientState state;
        public float currentCookingSeconds;

        public CookingIngredient(IngredientData data)
        {
            this.data = data;
            state = IngredientState.Raw;
            currentCookingSeconds = 0;
        }
    }
}
