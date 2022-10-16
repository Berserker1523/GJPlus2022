using UnityEngine;

namespace Kitchen
{
    public class CookingToolView : ButtonHandler
    {
        [SerializeField] private CookingToolData cookingToolData;

        private IngredientView currentlyCookingIngredient;
        private float currentlyCookingSeconds;

        FMOD.Studio.EventInstance CookingSound;

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.selectedGameObject as IngredientView;
            SelectionManager.selectedGameObject = null;

            if (ingredientView == null || ingredientView.NecessaryCookingTool != cookingToolData.cookingToolName)
                return;

            currentlyCookingIngredient = Instantiate(ingredientView, transform.position, transform.rotation, transform);
            currentlyCookingSeconds = 0;

            CookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Enciende Sarten");
            CookingSound.start();
        }

        private void Update()
        {
            if (currentlyCookingIngredient == null)
                return;

            currentlyCookingSeconds += Time.deltaTime;
            if (currentlyCookingIngredient.State == IngredientState.Raw && currentlyCookingSeconds >= cookingToolData.cookingSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Cooked;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Comida Lista", transform.position);
            }
                
            else if(currentlyCookingIngredient.State == IngredientState.Cooked && currentlyCookingSeconds >= cookingToolData.burningSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Burned;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Comida Quemada", transform.position);
            }
                
        }
    }
}
