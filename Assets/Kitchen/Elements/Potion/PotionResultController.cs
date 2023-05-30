using System.Collections;
using UnityEngine;

namespace Kitchen
{
    [RequireComponent(typeof(DragView))]
    public class PotionResultController : MonoBehaviour, IReleaseable
    {
        [SerializeField] private SpriteRenderer resultSprite;

        private BoxCollider2D col;

        public RecipeData CurrentRecipe { get; private set; }

        private void Awake()
        {
            col = GetComponent<BoxCollider2D>();
            col.enabled = false;
            resultSprite.enabled = false;
        }

        public void SetPotion(RecipeData data, Sprite recipeSprite)
        {
            CurrentRecipe = data;
            resultSprite.enabled = true;
            col.enabled = true;
            resultSprite.sprite = recipeSprite;
            col.size = Vector3.Scale(resultSprite.localBounds.size, resultSprite.transform.localScale);
            col.offset = Vector3.Scale(resultSprite.localBounds.center, resultSprite.transform.localScale);
        }

        public void Release()
        {
            CurrentRecipe = null;
            StartCoroutine(DisableCollider());
            resultSprite.enabled = false;
        }

        IEnumerator DisableCollider()
        {
            yield return new WaitForSeconds(0.1f);
            col.enabled = false;
        }
    }
}
