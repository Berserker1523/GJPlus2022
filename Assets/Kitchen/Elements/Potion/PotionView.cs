using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

namespace Kitchen
{
    public class PotionView : ButtonHandler
    {
        [SerializeField] private TextMeshProUGUI IngredientsLabel;

        public List<IngredientName> Ingredients { get; private set; } = new();
        [SerializeField] private LevelData recipesData;
        public Image[] ingredientsPlaces;
        public Image potionSkin;
        public Sprite failedPotionSkin;
        public Sprite defaultPotionSkin;

        public void Clear()
        {
            Ingredients.Clear();
            IngredientsLabel.text = string.Empty;
            foreach(Image img in ingredientsPlaces)
            {
                img.sprite = null;
            }
            potionSkin.sprite = defaultPotionSkin;
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.selectedGameObject as IngredientView;
            SelectionManager.selectedGameObject = this;

            if (Ingredients.Count >= 3)
            {
                changePotionView();
                return;
            }

            if (ingredientView == null || ( ingredientView.State != IngredientState.Cooked && ingredientView.NecessaryCookingTool != CookingToolName.None ))
                return;

            ingredientsPlaces[Ingredients.Count].sprite = ingredientView.stateDefault;

            Ingredients.Add(ingredientView.IngredientName);
            //IngredientsLabel.text += $"{ingredientView.IngredientName}\n";
            ingredientView.Release();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Infusión");
        }

        private void changePotionView()
        {
            bool acceptedRecipe =false;
            foreach (IngredientList ingredientList in recipesData.levelRecipes) 
            {
                acceptedRecipe = ingredientList.ingredients.OrderBy(x => x).SequenceEqual(Ingredients.OrderBy(x => x));
                if (acceptedRecipe)
                {
                    potionSkin.sprite = ingredientList.potionSkin;
                    return;
                }
            }
            if (!acceptedRecipe)
            {

                potionSkin.sprite = failedPotionSkin;
            }
        }

        private void Update()
        {
            if (Ingredients.Count < 3 || Ingredients.Count > 3)
                return;

            changePotionView();
        }
    }
}
