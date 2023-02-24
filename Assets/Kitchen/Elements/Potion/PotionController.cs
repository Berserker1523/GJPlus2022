using Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{

    [RequireComponent(typeof(Animator))]
    public class PotionController : MonoBehaviour, IDropHandler, IPointerDownHandler
    {
        public const int MaxAllowedIngredients = 3;

        [SerializeField] private Sprite failedPotionSkin;
        [SerializeField] private Sprite defaultPotionSkin;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer[] potionBranchesSprites;
        [SerializeField] private PotionResultController potionResult;

        [SerializeField] private Animator anim;
        private LevelInstantiator levelInstantiator;
        public readonly List<PotionIngredient> potionIngredients = new();

        public RecipeData CurrentRecipe;

        private CookingTimerView timer;
        private IEnumerator updateTimerRoutine;

        private void Awake() =>
            levelInstantiator = FindObjectOfType<LevelInstantiator>();

        private void Start()
        {
            anim = GetComponent<Animator>();
            timer = GetComponentInChildren<CookingTimerView>();
            timer.gameObject.SetActive(false);
        }


        public void OnDrop(PointerEventData pointerEventData)
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

        public void HandleCookingToolReceived(CookingToolController cookingToolController)
        {
            if (cookingToolController.CurrentCookingIngredient.state != IngredientState.Cooked)
                return;
            AddIngredient(cookingToolController.CurrentCookingIngredient.data, cookingToolController.CookingToolData.cookingToolName);
            cookingToolController.Release();
        }

        public void HandleIngredientReceived(IngredientController ingredientController)
        {
            if (ingredientController.IngredientData.necessaryCookingTool != CookingToolName.None)
                return;
            AddIngredient(ingredientController.IngredientData, CookingToolName.None);
            EventManager.Dispatch(PotionEvent.AddWater);
        }

        private void AddIngredient(IngredientData ingredientData, CookingToolName usedCookingTool)
        {
            PotionIngredient potionIngredient = new(ingredientData, usedCookingTool);
            potionIngredients.Add(potionIngredient);
            potionBranchesSprites[potionIngredients.Count - 1].sprite = potionIngredient.data.rawSprite;
            EventManager.Dispatch(PotionEvent.AddIngredient);
        }


        private void CheckRecipe()
        {
            List<PotionIngredient> recipeIngredients = new();

            foreach (RecipeData recipe in levelInstantiator.LevelData.levelRecipes)
            {
                recipeIngredients.Clear();
                foreach (var ingredient in recipe.ingredients)
                    recipeIngredients.Add(new(ingredient.ingredient, ingredient.cookingToolName));

                if (!recipeIngredients.OrderBy(x => x.data.ingredientName).SequenceEqual(potionIngredients.OrderBy(x => x.data.ingredientName)) || !recipeIngredients.OrderBy(x => x.usedCookingTool).SequenceEqual(potionIngredients.OrderBy(x => x.usedCookingTool)))
                    continue;

                CurrentRecipe = recipe;
                potionResult.SetPotion(CurrentRecipe, recipe.sprite);
                EventManager.Dispatch(PotionEvent.Poof);
                ClearShaker();
                return;
            }
            potionResult.SetPotion(CurrentRecipe, failedPotionSkin);

            EventManager.Dispatch(PotionEvent.FailedRecipe);
            ClearShaker();
        }

        public void ClearShaker()
        {
            potionIngredients.Clear();
            CurrentRecipe = null;
            // spriteRenderer.sprite = defaultPotionSkin;
            foreach (SpriteRenderer renderer in potionBranchesSprites)
                renderer.sprite = null;
        }

        //This method must be called with an animation Event at the end of Shaking Animation
        public void EndAShakingnimationEvent()
        {
            anim.SetBool("Shake", false);
            StopCoroutine(updateTimerRoutine);
            timer.gameObject.SetActive(false);
            CheckRecipe();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (potionResult.CurrentRecipe != null || potionIngredients.Count == 0)
                return;

            anim.SetBool("Shake", true);
            EventManager.Dispatch(PotionEvent.Shake);
            timer.gameObject.SetActive(true);
            timer.SetCooking();
            updateTimerRoutine = ShakingTimerRoutine(2);
            StartCoroutine(updateTimerRoutine);
        }

        private IEnumerator ShakingTimerRoutine(float seconds)
        {
            float currentTime = 0;
            while(true)
            {
                yield return new WaitForEndOfFrame();
                currentTime += Time.deltaTime;
                timer.SetFillAmount(currentTime / seconds);
            }
        }
    }
}
