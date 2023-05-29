using Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(DropView))]
    public class PotionController : MonoBehaviour, IPointerDownHandler
    {
        public const int MaxAllowedIngredients = 3;

        [SerializeField] private Sprite failedPotionSkin;
        [SerializeField] private Sprite defaultPotionSkin;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer[] potionBranchesSprites;
        [SerializeField] private ObjectScalerInBounds[] potionBranchesScalers;
        [SerializeField] private PotionResultController potionResult;

        private Animator anim;

        private LevelInstantiator levelInstantiator;
        private CookingTimerView timer;
        private PotionParticles potionParticles;
        private DropView dropView;

        public readonly List<PotionIngredient> potionIngredients = new();
        private RecipeData currentRecipe;
        private IEnumerator updateTimerRoutine;

        private FMOD.Studio.EventInstance shakerSound;
        private bool shakerEnabled = true;
        private bool inTutorial;
        private BoxCollider2D bc;

        private void Awake()
        {
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            anim = GetComponent<Animator>();
            timer = GetComponentInChildren<CookingTimerView>();
            timer.gameObject.SetActive(false);
            potionParticles = GetComponentInChildren<PotionParticles>();
            dropView = GetComponent<DropView>();
            bc = GetComponent<BoxCollider2D>();
            dropView.OnDropped += HandleDropped;
            dropView.IsDraggedObjectInteractableWithMe = IsDraggedObjectInteractableWithMe;
            shakerSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Shaker");
            EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, StopTimer);
        }

        private void StopTimer(bool tutorial)
        {
            inTutorial = tutorial;
            anim.speed = tutorial ? 0 : 1;
        }

        private void OnDestroy()
        {
            dropView.OnDropped -= HandleDropped;
            EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, StopTimer);
        }

        private bool IsDraggedObjectInteractableWithMe(PointerEventData pointerEventData)
        {
            if (potionIngredients.Count == MaxAllowedIngredients)
                return false;

            if (pointerEventData.pointerDrag.TryGetComponent(out CookingToolController cookingToolController) &&
                cookingToolController.CurrentCookingIngredient.state == IngredientState.Cooked ||
                pointerEventData.pointerDrag.TryGetComponent(out IngredientController ingredientController) &&
                ingredientController.IngredientData.necessaryCookingTool == CookingToolName.None)
                return true;

            return false;
        }

        public void HandleDropped(PointerEventData pointerEventData)
        {
            if (!IsDraggedObjectInteractableWithMe(pointerEventData))
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
            AddIngredient(cookingToolController.CurrentCookingIngredient.data, cookingToolController.CookingToolData.cookingToolName);
            cookingToolController.Release();
        }

        public void HandleIngredientReceived(IngredientController ingredientController)
        {
            AddIngredient(ingredientController.IngredientData, CookingToolName.None);
            EventManager.Dispatch(PotionEvent.AddWater);
        }

        private void AddIngredient(IngredientData ingredientData, CookingToolName usedCookingTool)
        {
            PotionIngredient potionIngredient = new(ingredientData, usedCookingTool);
            potionIngredients.Add(potionIngredient);
            potionBranchesSprites[potionIngredients.Count - 1].sprite = potionIngredient.data.rawSprite;
            potionBranchesScalers[potionIngredients.Count - 1].ScaleSpriteToBounds();

            if (ingredientData.ingredientName != IngredientName.Water)
                EventManager.Dispatch(PotionEvent.AddIngredient, ingredientData);
        }

        private void CheckRecipe()
        {
            List<PotionIngredient> recipeIngredients = new();

            foreach (RecipeData recipe in levelInstantiator.LevelData.levelRecipes)
            {
                recipeIngredients.Clear();
                foreach (var ingredient in recipe.ingredients)
                    recipeIngredients.Add(new(ingredient.ingredient, ingredient.cookingToolName));

                if (!recipeIngredients.OrderBy(x => x.data.ingredientName).SequenceEqual(potionIngredients.OrderBy(x => x.data.ingredientName)) ||
                     !recipeIngredients.OrderBy(x => $"{x.usedCookingTool}+{x.data.ingredientName}").SequenceEqual(potionIngredients.OrderBy(x => $"{x.usedCookingTool}+{x.data.ingredientName}")))
                    continue;

                currentRecipe = recipe;
                potionResult.SetPotion(currentRecipe, recipe.sprite);
                EventManager.Dispatch(PotionEvent.Poof);
                potionParticles.SuccesActivator();

                ClearShaker();
                return;
            }
            potionResult.SetPotion(currentRecipe, failedPotionSkin);

            EventManager.Dispatch(PotionEvent.FailedRecipe, potionResult.transform);
            potionParticles.FailureActivator();
            ClearShaker();
        }

        public void ClearShaker()
        {
            potionIngredients.Clear();
            currentRecipe = null;
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
            shakerSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CheckRecipe();
            bc.enabled = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (potionResult.CurrentRecipe != null || potionIngredients.Count <= 1 || !shakerEnabled)
                return;

            anim.SetBool("Shake", true);
            EventManager.Dispatch(PotionEvent.Shake);
            timer.gameObject.SetActive(true);
            timer.SetCooking();
            shakerSound.start();
            updateTimerRoutine = ShakingTimerRoutine(2);
            bc.enabled = false;
            StartCoroutine(updateTimerRoutine);
        }

        private IEnumerator ShakingTimerRoutine(float seconds)
        {
            float currentTime = 0;
            while (true)
            {
                while (inTutorial)
                    yield return new WaitForSeconds(0.1f);
                yield return new WaitForEndOfFrame();
                currentTime += Time.deltaTime;
                timer.SetFillAmount(currentTime / seconds);
            }
        }

        public void SwitchShakerEnabled(bool enabled) => shakerEnabled = enabled;
    }
}
