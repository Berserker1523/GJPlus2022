using UnityEngine;

namespace Kitchen
{

    public class IngredientView : ButtonHandler
    {
        [SerializeField] private IngredientData ingredientData;
        [SerializeField] private Sprite stateRaw;
        [SerializeField] private Sprite stateCooked;
        [SerializeField] private Sprite stateBurnt;

        public IngredientName IngredientName => ingredientData.ingredientName;
        public CookingToolName NecessaryCookingTool => ingredientData.necessaryCookingTool;
        public IngredientState State 
        {
            get { return state; }
            set
            {
                state = value;
                if (state == IngredientState.Cooked)
                    button.image.sprite = stateCooked;
                else if (state == IngredientState.Burned)
                    button.image.sprite = stateBurnt;
                else if (state == IngredientState.Raw)
                    button.image.sprite = stateRaw;
            }
        }
        private IngredientState state;

        protected override void OnClick()
        {
            SelectionManager.selectedGameObject = this;
        }
    }
}
