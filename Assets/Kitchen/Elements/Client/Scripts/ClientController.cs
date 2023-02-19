using Events;
using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private List<Image> ingredientsImages;
        [SerializeField] private List<Image> cookingToolsImages;
        [SerializeField] private GameObject treePrefab;

        private SpriteRenderer clientSpriteRend;
        [SerializeField] private Sprite happyClientSprite;

        private bool clientServed = false;
        private RecipeData requiredRecipe;
        private float waitingTimer;

        protected void Awake()
        {
            clientSpriteRend = GetComponent<SpriteRenderer>();
            waitingTimer = MaxWaitingSeconds;
            slider.value = 1;
            EventManager.Dispatch(ClientEvent.Arrived);
        }

        public void Initialize(RecipeData requiredRecipe, Sprite potionSprite)
        {
            this.requiredRecipe = requiredRecipe;
            potionImage.sprite = potionSprite;

            int i = 0;
            while (i < requiredRecipe.ingredients.Length)
            {
                ingredientsImages[i].sprite = requiredRecipe.ingredients[i].ingredient.rawSprite;
                if (requiredRecipe.ingredients[i].cookingToolName == CookingToolName.Mortar)
                    cookingToolsImages[i].sprite = requiredRecipe.ingredients[i].ingredient.mortarRawSprite;
                else if (requiredRecipe.ingredients[i].cookingToolName == CookingToolName.Stove)
                    cookingToolsImages[i].sprite = requiredRecipe.ingredients[i].ingredient.stoveRawSprite;
                else
                    cookingToolsImages[i].enabled = false;
                i++;
            }

            for (; i < ingredientsImages.Count; i++)
            {
                ingredientsImages[i].enabled = false;
                cookingToolsImages[i].enabled = false;
            }
            if (requiredRecipe.clientSprite != null)
                clientSpriteRend.sprite = requiredRecipe.clientSprite;
        }

        private void Update()
        {
            if (clientServed)
                return;

            waitingTimer -= Time.deltaTime;
            slider.value = waitingTimer / MaxWaitingSeconds;

            if (waitingTimer <= MaxWaitingSeconds * 0.25)
            {
                ColorUtility.TryParseHtmlString("#ef2424", out Color color);
                sliderBarImage.color = color;
            }
            else if (waitingTimer <= MaxWaitingSeconds * 0.5)
            {
                ColorUtility.TryParseHtmlString("#ef7824", out Color color);
                sliderBarImage.color = color;
            }
            else if (waitingTimer <= MaxWaitingSeconds * 0.75)
            {
                ColorUtility.TryParseHtmlString("#efd924", out Color color);
                sliderBarImage.color = color;
            }
            else if (waitingTimer > MaxWaitingSeconds * 0.75)
            {
                ColorUtility.TryParseHtmlString("#73bd06", out Color color);
                sliderBarImage.color = color;
            }

            if (waitingTimer <= 0)
            {
                Instantiate(treePrefab, transform.parent);
                Destroy(gameObject);
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

            for(int i=0; i< potionController.CurrentRecipe.ingredients.Length;i++)
            {
                for(int j=0; j<Enum.GetValues(typeof(IngredientName)).Length;j++)
                {
                    if ((int)potionController.CurrentRecipe.ingredients[i].ingredient.ingredientName == j)
                        GlobalCounter.attendedClients[j]++;              
                }
            }

            potionController.Release();
            AddMoney();
            StartCoroutine(ClientServedRoutine());
        }

        public IEnumerator ClientServedRoutine()
        {
            //Display Happy Animation Here!
            clientServed = true;
            clientSpriteRend.sprite = happyClientSprite;
            EventManager.Dispatch(ClientEvent.Served);
            yield return new WaitForSeconds(3f);

            Destroy(gameObject);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(MaxWaitingSeconds * waitingTimer / MaxWaitingSeconds) + 10; //TODO Burned Variable
    }


}
