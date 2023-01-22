using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kitchen
{
    [RequireComponent(typeof(Collider2D))]
    public class ClientController : MonoBehaviour, IDropHandler
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

        public void OnDrop(PointerEventData pointerEventData)
        {
            pointerEventData.pointerDrag.TryGetComponent(out PotionController potionController);
            pointerEventData.pointerDrag.TryGetComponent(out PainkillerController painkillerController);

            if (painkillerController != null)
            {
                waitingTimer = MaxWaitingSeconds;
                Destroy(painkillerController.gameObject);
                return;
            }

            if (potionController == null)
                return;

            bool acceptedRecipe = potionController.CurrentRecipe == requiredRecipe;

            if (!acceptedRecipe)
                return;

            potionController.Release();
            AddMoney();
            EventManager.Dispatch(ClientEvent.Served);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Atiende cliente");
            Destroy(gameObject);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(MaxWaitingSeconds * waitingTimer / MaxWaitingSeconds) + 10; //TODO Burned Variable
    }
}
