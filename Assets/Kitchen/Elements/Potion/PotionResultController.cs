using Kitchen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(DragView))]
    public class PotionResultController : MonoBehaviour, IReleaseable
    {
        [SerializeField] public RecipeData CurrentRecipe;
        [SerializeField] private SpriteRenderer resultSprite;

        private void Start()=>     
            resultSprite.enabled = false;
        

        public void SetPotion(RecipeData data, Sprite recipeSprite)
        {
            CurrentRecipe = data;
            resultSprite.enabled = true;
            resultSprite.sprite = recipeSprite; 
        }

        public void Release()
        {
              CurrentRecipe = null;
              resultSprite.enabled = false;
        }

    }
}
