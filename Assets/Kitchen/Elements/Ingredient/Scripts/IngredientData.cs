using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
    public class IngredientData : ScriptableObject
    {
        public IngredientName ingredientName;
        public CookingToolName necessaryCookingTool;
    }
}
