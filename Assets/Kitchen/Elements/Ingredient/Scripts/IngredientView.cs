using UnityEngine;

namespace Kitchen
{

    public class IngredientView : ButtonHandler
    {
        [SerializeField] private IngredientData ingredientData;
        [SerializeField] public Sprite stateRaw;
        [SerializeField] public Sprite stateCooked;
        [SerializeField] public Sprite stateBurnt;

        public IngredientName IngredientName => ingredientData.ingredientName;
        public CookingToolName NecessaryCookingTool => ingredientData.necessaryCookingTool;
        public IngredientState State 
        {
            get { return state; }
            set
            {
                state = value;               
            }
        }
        private IngredientState state;

        protected override void OnClick()
        {
            SelectionManager.selectedGameObject = this;
        }
    }
}
