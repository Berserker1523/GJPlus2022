using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class PotionView : ButtonHandler
    {
        public List<IngredientName> Ingredients { get; private set; } = new();

        public Image[] ingredientsPlaces;
        public Image potionSkin;
        public Sprite failedPotionSkin;
        public Sprite defaultPotionSkin;

        public void Clear()
        {
            Ingredients.Clear();
            foreach (Image img in ingredientsPlaces)
                img.sprite = null;
            potionSkin.sprite = defaultPotionSkin;
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.SelectedGameObject as IngredientView;
            SelectionManager.SelectedGameObject = this;

            if (Ingredients.Count >= 3)
            {
                changePotionView();
                return;
            }

            if (ingredientView == null || (ingredientView.State != IngredientState.Cooked && ingredientView.NecessaryCookingTool != CookingToolName.None))
                return;

            ingredientsPlaces[Ingredients.Count].sprite = ingredientView.stateDefault;
            Ingredients.Add(ingredientView.IngredientName);
            ingredientView.Release();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Infusi�n");
        }

        private void changePotionView()
        {
            bool acceptedRecipe = false;
            foreach (IngredientList ingredientList in LevelInstantiator.levelDataGlobal.levelRecipes)
            {
                acceptedRecipe = ingredientList.ingredients.OrderBy(x => x).SequenceEqual(Ingredients.OrderBy(x => x));
                if (acceptedRecipe)
                {
                    potionSkin.sprite = ingredientList.potionSkin;
                    return;
                }
            }

            if (!acceptedRecipe)
                potionSkin.sprite = failedPotionSkin;
        }

        private void Update()
        {
            if (Ingredients.Count < 3 || Ingredients.Count > 3)
                return;

            changePotionView();
        }
    }
}
