using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
    public class IngredientData : ScriptableObject
    {
        public IngredientName ingredientName;
        public CookingToolName necessaryCookingTool;

        [Header("Stove Sprites")]
        public Sprite rawState;
        public Sprite cookedState;
        public Sprite burntState;

        [Header("Mortar Sprites")]
        public Sprite entireState;
        public Sprite crushedState;
    }
}
