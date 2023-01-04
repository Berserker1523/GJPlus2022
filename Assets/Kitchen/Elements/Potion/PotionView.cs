using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class PotionView : ButtonHandler
    {
        public List<IngredientName> Ingredients { get; private set; } = new();
        public List<CookingToolName> IngredientsMethod { get; private set; } = new();

        public Image[] ingredientsPlaces;
        public Image potionSkin;
        public Sprite failedPotionSkin;
        public Sprite defaultPotionSkin;
        public RecipeData currentRecipe;
        public bool failedRecipe=false;

        public void Clear()
        {
            Ingredients.Clear();
            IngredientsMethod.Clear();
            foreach (Image img in ingredientsPlaces)
                img.sprite = null;
            potionSkin.sprite = defaultPotionSkin;
            currentRecipe = null;
            failedRecipe = false;
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.SelectedGameObject as IngredientView;
            SelectionManager.SelectedGameObject = this;

            if (Ingredients.Count >= 3 && currentRecipe==null)
            {
                changePotionView();
                return;
            }

            if (ingredientView == null || (ingredientView.State != IngredientState.Cooked && ingredientView.NecessaryCookingTool != CookingToolName.None))
                return;

            ingredientsPlaces[Ingredients.Count].sprite = ingredientView.stateDefault;
            Ingredients.Add(ingredientView.IngredientName);
            IngredientsMethod.Add(ingredientView.usedCookingTool);

            ingredientView.Release();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Infusión");
        }

        private void changePotionView()
        {
            if (currentRecipe != null || failedRecipe)
                return;

            bool acceptedRecipe = false;
            List<IngredientName> tempList = new List<IngredientName>();
            List<CookingToolName> tempList2= new List<CookingToolName>();

            foreach (RecipeData recipe in LevelInstantiator.levelDataGlobal.levelRecipes)
            {
                foreach (var ingredient in recipe.ingredients)
                {
                    tempList.Add(ingredient.ingredient.ingredientName);
                    tempList2.Add(ingredient.cookingToolName);
                }
                
                if(tempList.OrderBy(x => x).SequenceEqual(Ingredients.OrderBy(x => x)) && tempList2.OrderBy(x => x).SequenceEqual(IngredientsMethod.OrderBy(x => x)))
                    acceptedRecipe = true;

                if (acceptedRecipe)
                {
                    currentRecipe = recipe;
                    potionSkin.sprite = recipe.sprite;
                    return;
                }
            }

            if (!acceptedRecipe)
            {
                potionSkin.sprite = failedPotionSkin;
                failedRecipe = true;
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
