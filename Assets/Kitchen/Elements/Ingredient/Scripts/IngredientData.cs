using UnityEngine;
using UnityEngine.Events;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
    public class IngredientData : ScriptableObject
    {
        public static UnityAction assetsChanged;

        public IngredientName ingredientName;
        public CookingToolName necessaryCookingTool;

        [Header("Common Sprites")]
        public Sprite rawSprite;
        public Sprite dishSprite;

        [Header("Stove Sprites")]
        public Sprite stoveRawSprite;
        public Sprite stoveCookedSprite;
        public Sprite stoveBurntSprite;

        [Header("Mortar Sprites")]
        public Sprite mortarRawSprite;
        public Sprite mortarCrushedSprite;
        
        public void Awake() =>
            assetsChanged?.Invoke();
    }
}
