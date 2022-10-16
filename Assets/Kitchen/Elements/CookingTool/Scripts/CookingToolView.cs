﻿using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class CookingToolView : ButtonHandler
    {
        [SerializeField] private CookingToolData cookingToolData;

        private IngredientView currentlyCookingIngredient;
        [SerializeField] private Image image;
        private float currentlyCookingSeconds;

        FMOD.Studio.EventInstance MorteroSound;

        FMOD.Studio.EventInstance CookingSound;


        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.selectedGameObject as IngredientView;
            SelectionManager.selectedGameObject = null;

            if (currentlyCookingIngredient != null)
                return;
            
            if (ingredientView == null || ingredientView.NecessaryCookingTool != cookingToolData.cookingToolName)
                return;

            currentlyCookingIngredient = Instantiate(ingredientView, transform.position, transform.rotation, transform);
            currentlyCookingIngredient.State = IngredientState.Raw;
            currentlyCookingIngredient.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            image.sprite = currentlyCookingIngredient.stateRaw;
            currentlyCookingSeconds = 0;

            CookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Enciende Sarten");
            CookingSound.start();
            CookingSound.setParameterByName("Cocinando", 1);

            //MorteroSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Mortero");
            //MorteroSound.start();
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
            image.sprite = currentlyCookingIngredient.stateCooked;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Lista", transform.position);
            }
                
            else if(currentlyCookingIngredient.State == IngredientState.Cooked && currentlyCookingSeconds >= cookingToolData.burningSeconds)
            {
                currentlyCookingIngredient.State = IngredientState.Burned;
            image.sprite = currentlyCookingIngredient.stateBurnt;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Comida Quemada", transform.position);
                CookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
                
        }
        
    }
}
