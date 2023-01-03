using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class CookingToolView : ButtonHandler
    {
        [SerializeField] public CookingToolData cookingToolData;
        [SerializeField] public Image image;

        private Sprite initialSprite;

        private IngredientView currentlyCookingIngredient;
        private float currentlyCookingSeconds;

        FMOD.Studio.EventInstance cookingSound;

        protected override void Awake()
        {
            base.Awake();
            initialSprite = image.sprite;
        }

        private void Update()
        {
            if (currentlyCookingIngredient == null)
            {
                cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                return;
            }

            currentlyCookingSeconds += Time.deltaTime;
            if (currentlyCookingIngredient.State == IngredientState.Raw && currentlyCookingSeconds >= cookingToolData.cookingSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Cooked;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Lista", transform.position);
                if(cookingToolData.cookingToolName == CookingToolName.Mortar)
                    cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else if (currentlyCookingIngredient.State == IngredientState.Cooked && currentlyCookingSeconds >= cookingToolData.burningSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Burnt;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Quemada", transform.position);
                cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.SelectedGameObject as IngredientView;
            SelectionManager.SelectedGameObject = null;

            if (currentlyCookingIngredient != null)
                return;

            if (ingredientView == null || !ingredientView.NecessaryCookingTool.HasFlag( cookingToolData.cookingToolName) || ingredientView.usedCookingTool !=0)
                return;
            

            currentlyCookingIngredient = Instantiate(ingredientView, transform.position, transform.rotation, transform);
            currentlyCookingIngredient.CookingToolView = this;
            currentlyCookingIngredient.State = IngredientState.Raw;
            currentlyCookingIngredient.button.targetGraphic = button.targetGraphic;
            button.enabled = false;
            currentlyCookingSeconds = 0;

            if(cookingToolData.cookingToolName == CookingToolName.Stove)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Enciende Sarten");
                cookingSound.start();
                cookingSound.setParameterByName("Cocinando", 1);
            }
            else if(cookingToolData.cookingToolName == CookingToolName.Mortar)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Mortero");
                cookingSound.start();
            }
        }

        public void SetInitialSprite()
        {
            button.enabled = true;
            image.sprite = initialSprite;
        }
    }
}
