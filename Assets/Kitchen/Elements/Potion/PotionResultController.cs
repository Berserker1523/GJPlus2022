using UnityEngine;

namespace Kitchen
{
    [RequireComponent(typeof(DragView))]
    public class PotionResultController : MonoBehaviour, IReleaseable
    {
        [SerializeField] private SpriteRenderer resultSprite;

        private Collider2D collider;

        public RecipeData CurrentRecipe { get; private set; }

        private void Awake()
        {
            collider = GetComponent<Collider2D>();
            collider.enabled = false;
            resultSprite.enabled = false;
        }

        public void SetPotion(RecipeData data, Sprite recipeSprite)
        {
            CurrentRecipe = data;
            resultSprite.enabled = true;
            collider.enabled = true;
            resultSprite.sprite = recipeSprite;
        }

        public void Release()
        {
            CurrentRecipe = null;
            collider.enabled = false;
            resultSprite.enabled = false;
        }
    }
}
