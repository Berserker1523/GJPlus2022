using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kitchen
{
    public class ClientController : ClickHandlerBase
    {
        public const float MaxWaitingSeconds = 70f;

        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderBarImage;
        [SerializeField] private Image potionImage;
        [SerializeField] private GameObject treePrefab;


        private RecipeData requiredRecipe;
        private float waitingTimer;

        protected void Awake()
        {
            waitingTimer = MaxWaitingSeconds;
            slider.value = 1;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Llega Cliente");
        }

        public void Initialize(RecipeData requiredRecipe, Sprite potionSprite)
        {
            this.requiredRecipe = requiredRecipe;
            potionImage.sprite = potionSprite;
        }

        private void Update()
        {
            waitingTimer -= Time.deltaTime;
            slider.value = waitingTimer / MaxWaitingSeconds;

            if (waitingTimer <= MaxWaitingSeconds * 0.3)
                sliderBarImage.color = Color.red;
            else if (waitingTimer <= MaxWaitingSeconds * 0.6)
                sliderBarImage.color = Color.yellow;
            else if (waitingTimer > MaxWaitingSeconds * 0.6)
                sliderBarImage.color = Color.white;

            if (waitingTimer <= 0)
            {
                Instantiate(treePrefab, transform.parent);
                Destroy(gameObject);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Muere Cliente");
                EventManager.Dispatch(ClientEvent.Died);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            PotionView potionView = SelectionManager.SelectedGameObject as PotionView;
            PainkillerController painkillerView = SelectionManager.SelectedGameObject as PainkillerController;
            SelectionManager.SelectedGameObject = null;

            if (painkillerView != null)
            {
                waitingTimer = MaxWaitingSeconds;
                Destroy(painkillerView.gameObject);
                return;
            }

            if (potionView == null)
                return;

            bool acceptedRecipe = potionView.CurrentRecipe == requiredRecipe;

            if (!acceptedRecipe)
                return;

            potionView.Release();
            AddMoney();
            EventManager.Dispatch(ClientEvent.Served);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Atiende cliente");
            Destroy(gameObject);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(MaxWaitingSeconds * waitingTimer / MaxWaitingSeconds) + 10; //TODO Burned Variable
    }
}
