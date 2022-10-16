using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace Kitchen
{
    public class PotionView : ButtonHandler
    {
        [SerializeField] private TextMeshProUGUI IngredientsLabel;

        [HideInInspector] public List<IngredientName> ingredients = new();

        public void Clear()
        {
            ingredients.Clear();
            IngredientsLabel.text = string.Empty;
        }

        protected override void OnClick()
        {
            IngredientView ingredientView = SelectionManager.selectedGameObject as IngredientView;
            SelectionManager.selectedGameObject = this;

            if (ingredients.Count >= 3)
                return;
            
            if (ingredientView == null || ingredientView.State != IngredientState.Cooked)
                return;

            ingredients.Add(ingredientView.IngredientName);
            IngredientsLabel.text += $"{ingredientView.IngredientName}\n";
            Destroy(ingredientView.gameObject);
        }
    }
}
