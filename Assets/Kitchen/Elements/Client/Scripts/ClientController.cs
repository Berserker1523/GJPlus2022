using Events;
using System.Collections;
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

        private SpriteRenderer clientSpriteRend;
        [SerializeField] private Sprite happyClientSprite;

        private bool clientServed =false;
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
            clientSpriteRend = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (clientServed)
                return;

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
            pointerEventData.pointerDrag.TryGetComponent(out PotionResultController potionController);
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
            StartCoroutine(ClientServedRoutine());
        }

        public IEnumerator ClientServedRoutine()
        {
            //Display Happy Animation Here!
            clientServed = true;
            clientSpriteRend.sprite = happyClientSprite;
            yield return new WaitForSeconds(3f);

            EventManager.Dispatch(ClientEvent.Served);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Atiende cliente");
            Destroy(gameObject);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(MaxWaitingSeconds * waitingTimer / MaxWaitingSeconds) + 10; //TODO Burned Variable
    }
}
