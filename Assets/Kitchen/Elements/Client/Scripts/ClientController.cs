using Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kitchen
{
    public enum Gender 
    {
        female = 0, 
        male = 1 
    }

    [RequireComponent(typeof(DropView))]
    public class ClientController : MonoBehaviour
    {
        public const float MaxWaitingSeconds = 70f; //TODO burned variable

        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderBarImage;
        [SerializeField] private Image potionImage;
        [SerializeField] private List<Image> ingredientsImages;
        [SerializeField] private List<Image> cookingToolsImages;
        private ParticleSystem healingVFX;

        private DropView dropView;

        private bool clientServed = false;
        private RecipeData requiredRecipe;
        private float waitingTimer;
        
        private Gender clientGender;
        public static bool inTutorial;

        //Animation Params
        [SerializeField] RuntimeAnimatorController[] animators = new RuntimeAnimatorController[2];
        Animator animator;

        private string animatorIllnessParameter = "Illness";
        private string animatorRecuperatedParameter = "Recuperated";
        private string animatorDiedParameter ="Died";

        private string[] animsDie = new string []{ "char1_viruelaToMuerte", "char2_viruelaToMuerte", "char1_fiebreToMuerte", "char2_fiebreToMuerte" };
        private bool clientDied;
        private bool gruntDispatched;

        protected void Awake()
        {
            dropView = GetComponent<DropView>();

            waitingTimer = MaxWaitingSeconds;
            slider.value = 1;
            EventManager.Dispatch(ClientEvent.Arrived);

            dropView.OnDropped += HandleDropped;
            dropView.IsDraggedObjectInteractableWithMe = IsDraggedObjectInteractableWithMe;

            animator = GetComponentInChildren<Animator>();    
            healingVFX = GetComponentInChildren<ParticleSystem>();

            EventManager.AddListener(GameStatus.LevelFinished, StopCounter);
        }

        private void OnDestroy() =>  EventManager.RemoveListener(GameStatus.LevelFinished, StopCounter);            
        
        private void StopCounter() => clientServed = true;       

        public void Initialize(RecipeData requiredRecipe, Sprite potionSprite)
        {
            this.requiredRecipe = requiredRecipe;
            potionImage.sprite = potionSprite;
            potionImage.GetComponent<ImageResizeInBounds>().ResizeImageToBounds();

            int i = 0;
            while (i < requiredRecipe.ingredients.Length)
            {
                ingredientsImages[i].sprite = requiredRecipe.ingredients[i].ingredient.rawSprite;
                ingredientsImages[i].GetComponent<ImageResizeInBounds>().ResizeImageToBounds();
                if (requiredRecipe.ingredients[i].cookingToolName == CookingToolName.Mortar)
                    cookingToolsImages[i].sprite = requiredRecipe.ingredients[i].ingredient.mortarRawSprite;
                else if (requiredRecipe.ingredients[i].cookingToolName == CookingToolName.Stove)
                    cookingToolsImages[i].sprite = requiredRecipe.ingredients[i].ingredient.stoveRawSprite;
                else
                    cookingToolsImages[i].enabled = false;
                cookingToolsImages[i].GetComponent<ImageResizeInBounds>().ResizeImageToBounds();
                i++;
            }

            for (; i < ingredientsImages.Count; i++)
            {
                ingredientsImages[i].enabled = false;
                cookingToolsImages[i].enabled = false;
            }

            clientGender = (Gender)UnityEngine.Random.Range(0, 2); 
            animator.runtimeAnimatorController = animators[(int)clientGender];

            animator.SetInteger(animatorIllnessParameter, (int)requiredRecipe.diseasesItCures);
        }

        private void Update()
        {
            if (clientServed || inTutorial)
                return;

            waitingTimer -= Time.deltaTime;
            slider.value = waitingTimer / MaxWaitingSeconds;

            //TODO burned colors and variables
            if (waitingTimer <= MaxWaitingSeconds * 0.25)
            {
                ColorUtility.TryParseHtmlString("#ef2424", out Color color);
                sliderBarImage.color = color;

                if (!gruntDispatched)
                {
                    gruntDispatched = true;
                    EventManager.Dispatch(ClientEvent.Grunt, clientGender);
                }
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

            if (waitingTimer <= 0 && !clientDied)          
                StartCoroutine(DiedClientCoroutine());          
        }

        IEnumerator DiedClientCoroutine()
        {
            clientDied = true;
            animator.SetBool(animatorDiedParameter, true);
            foreach(var animation in animsDie)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation)) ;
                    yield return new WaitForSeconds(0.1f);
            }
            Destroy(gameObject);
            EventManager.Dispatch(ClientEvent.Died);
        }

        private bool IsDraggedObjectInteractableWithMe(PointerEventData pointerEventData) =>
            pointerEventData.pointerDrag.TryGetComponent(out PotionResultController _) ||
            pointerEventData.pointerDrag.TryGetComponent(out PainkillerController _);

        public void HandleDropped(PointerEventData pointerEventData)
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

            for (int i = 0; i < potionController.CurrentRecipe.ingredients.Length; i++)
            {
                for (int j = 0; j < Enum.GetValues(typeof(IngredientName)).Length; j++)
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
            //clientSpriteRenderer.sprite = happyClientSprite[(int)clientGender];
            animator.SetBool(animatorRecuperatedParameter, clientServed);
            EventManager.Dispatch(ClientEvent.Served);
            GetComponent<BoxCollider2D>().enabled = false;
            healingVFX.Play();
            yield return new WaitForSeconds(3f);

            Destroy(gameObject);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)waitingTimer + 10; //TODO Burned Variable
    }
}
