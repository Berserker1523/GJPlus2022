using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{

    public class IngredientView : ButtonHandler
    {
        [SerializeField] private IngredientData ingredientData;
        [SerializeField] public Sprite stateRaw;
        [SerializeField] public Sprite stateCooked;
        [SerializeField] public Sprite stateBurnt;

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
                    CookingToolView.image.sprite = stateRaw;
                }
                else if (state == IngredientState.Cooked)
                    CookingToolView.image.sprite = stateCooked;
                else if (state == IngredientState.Burnt)
                    CookingToolView.image.sprite = stateBurnt;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            image = GetComponent<Image>();
        }

        protected override void OnClick() =>
            SelectionManager.selectedGameObject = this;

        public void Release()
        {
            CookingToolView.SetInitialSprite();
            Destroy(gameObject);
        }
    }
}
