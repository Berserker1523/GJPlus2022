using UnityEngine;
using UnityEngine.Events;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
    public class IngredientData : ScriptableObject
    {
        public IngredientName ingredientName;
        public CookingToolName necessaryCookingTool;

        public static UnityAction assetsChanged;
        public void Awake() =>
            assetsChanged?.Invoke();

        [Header("Stove Sprites")]
        public Sprite rawState;
        public Sprite cookedState;
        public Sprite burntState;

        [Header("Mortar Sprites")]
        public Sprite entireState;
        public Sprite crushedState;
    }
}
