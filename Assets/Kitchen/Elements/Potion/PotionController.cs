using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(DragView))]
    public class PotionController : MonoBehaviour, IReleaseable
    {
        public const int MaxAllowedIngredients = 3;

        [SerializeField] private Sprite failedPotionSkin;
        [SerializeField] private Sprite defaultPotionSkin;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer[] potionBranchesSprites;

        private LevelInstantiator levelInstantiator;

        private DragView dragView;

        private readonly List<PotionIngredient> potionIngredients = new();

        public RecipeData CurrentRecipe { get; private set; }

        private void Awake()
        {
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            dragView = GetComponent<DragView>();
            dragView.OnDropped += HandleDropped;
        }

        private void OnDestroy()
        {
            dragView.OnDropped -= HandleDropped;
        }

        public void HandleDropped(PointerEventData pointerEventData)
        {
            if (potionIngredients.Count == MaxAllowedIngredients)
                return;

            if (pointerEventData.pointerDrag.TryGetComponent(out CookingToolController cookingToolController))
            {
                HandleCookingToolReceived(cookingToolController);
                return;
            }

            if (pointerEventData.pointerDrag.TryGetComponent(out IngredientController ingredientController))
            {
                HandleIngredientReceived(ingredientController);
                return;
            }
        }

        private void HandleCookingToolReceived(CookingToolController cookingToolController)
        {
            if (cookingToolController.CurrentCookingIngredient.state != IngredientState.Cooked)
                return;
            AddIngredient(cookingToolController.CurrentCookingIngredient.data, cookingToolController.CookingToolData.cookingToolName);
            cookingToolController.Release();
        }

        private void HandleIngredientReceived(IngredientController ingredientController)
        {
            if (ingredientController.IngredientData.necessaryCookingTool != CookingToolName.None)
                return;
            AddIngredient(ingredientController.IngredientData, CookingToolName.None);
        }

        private void AddIngredient(IngredientData ingredientData, CookingToolName usedCookingTool)
        {
            PotionIngredient potionIngredient = new(ingredientData, usedCookingTool);
            potionIngredients.Add(potionIngredient);
            potionBranchesSprites[potionIngredients.Count - 1].sprite = potionIngredient.data.rawSprite;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Infusión");

            if (potionIngredients.Count == MaxAllowedIngredients)
                CheckRecipe();
        }

        private void CheckRecipe()
        {
            if (CurrentRecipe != null)
                return;

            List<PotionIngredient> recipeIngredients = new();

            foreach (RecipeData recipe in levelInstantiator.LevelData.levelRecipes)
            {
                recipeIngredients.Clear();
                foreach (var ingredient in recipe.ingredients)
                    recipeIngredients.Add(new(ingredient.ingredient, ingredient.cookingToolName));

                if (!recipeIngredients.OrderBy(x => x.data.ingredientName).SequenceEqual(potionIngredients.OrderBy(x => x.data.ingredientName)) || !recipeIngredients.OrderBy(x => x.usedCookingTool).SequenceEqual(potionIngredients.OrderBy(x => x.usedCookingTool)))
                    continue;

                CurrentRecipe = recipe;
                spriteRenderer.sprite = recipe.sprite;
                return;
            }

            spriteRenderer.sprite = failedPotionSkin;
        }

        public void Release()
        {
            potionIngredients.Clear();
            CurrentRecipe = null;
            spriteRenderer.sprite = defaultPotionSkin;
            foreach (SpriteRenderer renderer in potionBranchesSprites)
                renderer.sprite = null;
        }
    }
}
