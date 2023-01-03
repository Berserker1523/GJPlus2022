using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{

    public class IngredientView : ButtonHandler
    {
        [SerializeField] private IngredientData ingredientData;
        [SerializeField] public Sprite stateDefault;
        [SerializeField] public CookingToolName usedCookingTool;

        private Image image;
        private IngredientState state;

        public CookingToolView CookingToolView { get; set; }

        public IngredientName IngredientName => ingredientData.ingredientName;
        public CookingToolName NecessaryCookingTool => ingredientData.necessaryCookingTool;

        public IngredientState State
        {
            get { return state; }
            set
            {
                state = value;
                if (state == IngredientState.Raw)
                {
                    image.color = new Color32(0, 0, 0, 0);
                    usedCookingTool = CookingToolView.cookingToolData.cookingToolName;
                    if (usedCookingTool == CookingToolName.Stove)
                        CookingToolView.image.sprite = ingredientData.rawState;
                    else if(usedCookingTool == CookingToolName.Mortar)
                        CookingToolView.image.sprite = ingredientData.entireState;
                }
                else if (state == IngredientState.Cooked)
                {
                    if(usedCookingTool == CookingToolName.Stove)
                        CookingToolView.image.sprite = ingredientData.cookedState;
                    else if(usedCookingTool == CookingToolName.Mortar)
                        CookingToolView.image.sprite = ingredientData.crushedState;
                }
                else if (state == IngredientState.Burnt)
                    CookingToolView.image.sprite = ingredientData.burntState;

                Debug.Log(usedCookingTool);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            image = GetComponent<Image>();
        }

        protected override void OnClick()
        {
            SelectionManager.SelectedGameObject = this;
            if (ingredientData.ingredientName == IngredientName.Water)
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Coge Agua");
        }
            

        public void Release()
        {
            if (ingredientData.ingredientName == IngredientName.Water)
                return;

            if (CookingToolView != null)
                CookingToolView.SetInitialSprite();
            Destroy(gameObject);
        }
    }
}
