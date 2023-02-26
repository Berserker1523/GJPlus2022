using UnityEngine;

namespace Kitchen
{
    [RequireComponent(typeof(DragView))]
    public class PotionResultController : MonoBehaviour, IReleaseable
    {
        [SerializeField] private SpriteRenderer resultSprite;

        private BoxCollider2D collider;

        public RecipeData CurrentRecipe { get; private set; }

        private void Awake()
        {
            collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;
            resultSprite.enabled = false;
        }

        public void SetPotion(RecipeData data, Sprite recipeSprite)
        {
            CurrentRecipe = data;
            resultSprite.enabled = true;
            collider.enabled = true;
            resultSprite.sprite = recipeSprite;
            collider.size = Vector3.Scale(resultSprite.localBounds.size, resultSprite.transform.localScale);
            collider.offset = Vector3.Scale(resultSprite.localBounds.center, resultSprite.transform.localScale);
        }

        public void Release()
        {
            CurrentRecipe = null;
            collider.enabled = false;
            resultSprite.enabled = false;
        }
    }
}
