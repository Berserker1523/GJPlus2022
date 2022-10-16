using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class CookingToolView : ButtonHandler
    {
        [SerializeField] private CookingToolData cookingToolData;
        [SerializeField] public Image image;

        private Sprite initialSprite;

        private IngredientView currentlyCookingIngredient;
        private float currentlyCookingSeconds;

        FMOD.Studio.EventInstance MorteroSound;
        FMOD.Studio.EventInstance CookingSound;

        protected override void Awake()
        {
            base.Awake();
            initialSprite = image.sprite;
        }

        private void Update()
        {
            if (currentlyCookingIngredient == null)
            {
                CookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                return;
            }

            currentlyCookingSeconds += Time.deltaTime;
            if (currentlyCookingIngredient.State == IngredientState.Raw && currentlyCookingSeconds >= cookingToolData.cookingSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Cooked;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Lista", transform.position);
            }
            else if (currentlyCookingIngredient.State == IngredientState.Cooked && currentlyCookingSeconds >= cookingToolData.burningSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Burnt;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Quemada", transform.position);
                CookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.selectedGameObject as IngredientView;
            SelectionManager.selectedGameObject = null;

            if (currentlyCookingIngredient != null)
                return;

            if (ingredientView == null || ingredientView.NecessaryCookingTool != cookingToolData.cookingToolName)
                return;

            currentlyCookingIngredient = Instantiate(ingredientView, transform.position, transform.rotation, transform);
            currentlyCookingIngredient.CookingToolView = this;
            currentlyCookingIngredient.State = IngredientState.Raw;
            currentlyCookingSeconds = 0;

            CookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Enciende Sarten");
            CookingSound.start();
            CookingSound.setParameterByName("Cocinando", 1);

            //MorteroSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Mortero");
            //MorteroSound.start();
        }

        public void SetInitialSprite() =>
            image.sprite = initialSprite;
    }
}
