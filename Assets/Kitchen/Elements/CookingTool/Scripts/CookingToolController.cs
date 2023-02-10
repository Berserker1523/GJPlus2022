using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(DragView))]
    public class CookingToolController : MonoBehaviour, IReleaseable
    {
        [SerializeField] private CookingToolData cookingToolData;

        private SpriteRenderer spriteRenderer;
        private DragView dragView;

        private Sprite initialSprite;
        private FMOD.Studio.EventInstance cookingSound;

        public CookingIngredient CurrentCookingIngredient { get; private set; }
        public CookingToolData CookingToolData => cookingToolData;

        public Timer timer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            dragView = GetComponent<DragView>();
            initialSprite = spriteRenderer.sprite;
            dragView.OnDropped += HandleDropped;

            timer = GetComponentInChildren<Timer>();
        }

        private void OnDestroy() =>
            dragView.OnDropped -= HandleDropped;

        private void Update()
        {
            if (CurrentCookingIngredient == null)
                return;

            CurrentCookingIngredient.currentCookingSeconds += Time.deltaTime;
            if (CurrentCookingIngredient.state == IngredientState.Raw && CurrentCookingIngredient.currentCookingSeconds >= cookingToolData.cookingSeconds)
            {
                CurrentCookingIngredient.state = IngredientState.Cooked;
                spriteRenderer.sprite = cookingToolData.cookingToolName == CookingToolName.Stove ? CurrentCookingIngredient.data.stoveCookedSprite : CurrentCookingIngredient.data.mortarCrushedSprite;
                EventManager.Dispatch(IngredientState.Cooked);
                if (cookingToolData.cookingToolName == CookingToolName.Mortar)
                    cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                else
                    timer.StartRedTimer(cookingToolData.burningSeconds);
            }
            else if (CurrentCookingIngredient.state == IngredientState.Cooked && CurrentCookingIngredient.currentCookingSeconds >= cookingToolData.burningSeconds)
            {
                CurrentCookingIngredient.state = IngredientState.Burnt;
                spriteRenderer.sprite = CurrentCookingIngredient.data.stoveBurntSprite;
                EventManager.Dispatch(IngredientState.Burnt);
                cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        private void HandleDropped(PointerEventData pointerEventData)
        {
            if (CurrentCookingIngredient != null)
                return;

            if (!pointerEventData.pointerDrag.TryGetComponent(out IngredientController ingredientController))
                return;

            if (!ingredientController.IngredientData.necessaryCookingTool.HasFlag(cookingToolData.cookingToolName))
                return;

            CurrentCookingIngredient = new(ingredientController.IngredientData);
            spriteRenderer.sprite = cookingToolData.cookingToolName == CookingToolName.Stove ? CurrentCookingIngredient.data.stoveRawSprite : CurrentCookingIngredient.data.mortarRawSprite;
            PlayCookingSound();
            timer.StartBlueTimer(cookingToolData.cookingSeconds); 
        }

        private void PlayCookingSound()
        {
            if (cookingToolData.cookingToolName == CookingToolName.Stove)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Enciende Sarten");
                cookingSound.start();
                cookingSound.setParameterByName("Cocinando", 1);
            }
            else if (cookingToolData.cookingToolName == CookingToolName.Mortar)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Mortero");
                cookingSound.start();
            }
        }

        public void Release()
        {
            CurrentCookingIngredient = null;
            spriteRenderer.sprite = initialSprite;
            cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            timer.StopTimer();
        }
    }
}
