using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    public class CookingToolController : ClickHandlerBase, IReleaseable
    {
        [SerializeField] private CookingToolData cookingToolData;

        private Sprite initialSprite;
        private FMOD.Studio.EventInstance cookingSound;

        public CookingIngredient CurrentCookingIngredient { get; private set; }
        public CookingToolData CookingToolData => cookingToolData;

        private void Awake() =>
            initialSprite = spriteRenderer.sprite;

        private void Update()
        {
            if (CurrentCookingIngredient == null)
                return;

            CurrentCookingIngredient.currentCookingSeconds += Time.deltaTime;
            if (CurrentCookingIngredient.state == IngredientState.Raw && CurrentCookingIngredient.currentCookingSeconds >= cookingToolData.cookingSeconds)
            {
                CurrentCookingIngredient.state = IngredientState.Cooked;
                spriteRenderer.sprite = cookingToolData.cookingToolName == CookingToolName.Stove ? CurrentCookingIngredient.data.stoveCookedSprite : CurrentCookingIngredient.data.mortarCrushedSprite;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Lista");
                if (cookingToolData.cookingToolName == CookingToolName.Mortar)
                    cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else if (CurrentCookingIngredient.state == IngredientState.Cooked && CurrentCookingIngredient.currentCookingSeconds >= cookingToolData.burningSeconds)
            {
                CurrentCookingIngredient.state = IngredientState.Burnt;
                spriteRenderer.sprite = CurrentCookingIngredient.data.stoveBurntSprite;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Quemada");
                cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            IngredientController ingredientView = SelectionManager.SelectedGameObject as IngredientController;
            SelectionManager.SelectedGameObject = this;

            if (CurrentCookingIngredient != null)
                return;

            if (ingredientView == null || !ingredientView.IngredientData.necessaryCookingTool.HasFlag(cookingToolData.cookingToolName))
                return;

            CurrentCookingIngredient = new(ingredientView.IngredientData);
            spriteRenderer.sprite = cookingToolData.cookingToolName == CookingToolName.Stove ? CurrentCookingIngredient.data.stoveRawSprite : CurrentCookingIngredient.data.mortarRawSprite;
            PlayCookingSound();
        }

        private void PlayCookingSound()
        {
            if (cookingToolData.cookingToolName == CookingToolName.Stove)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Enciende Sarten");
                cookingSound.start();
                cookingSound.setParameterByName("Cocinando", 1);
            }
            else if (cookingToolData.cookingToolName == CookingToolName.Mortar)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Mortero");
                cookingSound.start();
            }
        }

        public void Release()
        {
            CurrentCookingIngredient = null;
            spriteRenderer.sprite = initialSprite;
            cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
