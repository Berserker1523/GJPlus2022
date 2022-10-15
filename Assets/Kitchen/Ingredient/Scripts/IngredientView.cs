using UnityEngine;

namespace Kitchen
{

    public class IngredientView : ButtonHandler
    {
        [SerializeField] private IngredientData ingredientData;

        public CookingToolName NecessaryCookingTool => ingredientData.necessaryCookingTool;
        public IngredientState State 
        {
            get { return state; }
            set
            {
                state = value;
                if (state == IngredientState.Cooked)
                    button.image.color = Color.green;
                else if (state == IngredientState.Burned)
                    button.image.color = Color.red;
            }
        }
        private IngredientState state;

        protected override void OnClick()
        {
            SelectionManager.selectedGameObject = this;
        }
    }
}
