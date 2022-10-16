using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace Kitchen
{
    public class PotionView : ButtonHandler
    {
        [SerializeField] private TextMeshProUGUI IngredientsLabel;

        public List<IngredientName> Ingredients { get; private set; } = new();

        public void Clear()
        {
            Ingredients.Clear();
            IngredientsLabel.text = string.Empty;
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.selectedGameObject as IngredientView;
            SelectionManager.selectedGameObject = this;

            if (Ingredients.Count >= 3)
                return;
            
            if (ingredientView == null || ingredientView.State != IngredientState.Cooked)
                return;

            Ingredients.Add(ingredientView.IngredientName);
            IngredientsLabel.text += $"{ingredientView.IngredientName}\n";
            Destroy(ingredientView.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Infusión");
        }
    }
}
