using DG.Tweening;
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
        public const float MaxWaitingSeconds = 70; //TODO burned variable

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderBarImage;
        [SerializeField] private Image potionImage;
        [SerializeField] private List<Image> ingredientsImages;
        [SerializeField] private List<Image> cookingToolsImages;
        [SerializeField] private List<Image> arrowsImages;
        [SerializeField] private ParticleSystem healingVFX;
        [SerializeField] private ParticleSystem SicknessVFX;
        [SerializeField] private ParticleSystem Sickness2VFX;
        [SerializeField] private ParticleSystem SicknessSparkVFX;
        [SerializeField] private ParticleSystem DeathVFX;

        private DropView dropView;

        private bool clientServed = false;
        private RecipeData requiredRecipe;
        private float waitingTimer;

        private Gender clientGender;
        public static bool inTutorial;

        //Animation Params
        [SerializeField] RuntimeAnimatorController[] animators = new RuntimeAnimatorController[2];
        Animator animator;

        private readonly string animatorIllnessParameter = "Illness";
        private readonly string animatorRecuperatedParameter = "Recuperated";
        private readonly string animatorDiedParameter = "Died";
        private readonly string animatorArriveParameter = "Arrive";

        private string[] animsDie = new string[] { "char1_viruelaToMuerte", "char2_viruelaToMuerte", "char1_fiebreToMuerte", "char2_fiebreToMuerte" };
        private bool clientDied;
        private bool gruntDispatched;
        private bool sicknessParticlesActivated;
        private BoxCollider2D bc;

        Tween walkTween, scaleTween;

        protected void Awake()
        {
            dropView = GetComponent<DropView>();

            waitingTimer = MaxWaitingSeconds;
            slider.value = 1;
            EventManager.Dispatch(ClientEvent.Arrived);

            dropView.OnDropped += HandleDropped;
            dropView.IsDraggedObjectInteractableWithMe = IsDraggedObjectInteractableWithMe;

            animator = GetComponentInChildren<Animator>();
            bc = GetComponent<BoxCollider2D>();

            EventManager.AddListener(GameStatus.LevelFinished, StopCounter);
            EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, StopTweens);
        }

        private void StopTweens(bool inTutorial)
        {
            if(inTutorial == true)
            {
                walkTween.Pause();
                scaleTween.Pause();
            }
            else
            {
                walkTween.Play();
                scaleTween.Play();
            }
        }

        private void OnDestroy() => EventManager.RemoveListener(GameStatus.LevelFinished, StopCounter);

        private void StopCounter()
        {
            if (!clientServed)
                Destroy(gameObject);
        }

        public void Initialize(RecipeData requiredRecipe, Sprite potionSprite, Transform finalDestination)
        {
            SetClientGenderAndIllnessAnimations(requiredRecipe);

            transform.localScale = Vector3.one * 0.05f;
            scaleTween = transform.DOScale(0.55f, 15f).SetEase(Ease.InSine);
            walkTween = transform.DOMove(finalDestination.position, 15f).SetEase(Ease.InSine)
            .OnComplete(() => { 
                InitCanvas(requiredRecipe, potionSprite);
                animator.SetTrigger(animatorArriveParameter);
            });
        }

        private void InitCanvas(RecipeData requiredRecipe, Sprite potionSprite)
        {
            canvas.gameObject.SetActive(true);
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
                {
                    cookingToolsImages[i].enabled = false;
                    arrowsImages[i].enabled = false;
                }
                cookingToolsImages[i].GetComponent<ImageResizeInBounds>().ResizeImageToBounds();
                i++;
            }

            for (; i < ingredientsImages.Count; i++)
            {
                ingredientsImages[i].enabled = false;
                cookingToolsImages[i].enabled = false;
                arrowsImages[i].enabled = false;
            }
        }

        private void SetClientGenderAndIllnessAnimations(RecipeData requiredRecipe)
        {
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
            }
            else if (waitingTimer <= MaxWaitingSeconds * 0.5)
            {
                ColorUtility.TryParseHtmlString("#ef7824", out Color color);
                sliderBarImage.color = color;

                if (!gruntDispatched)
                {
                    gruntDispatched = true;
                    EventManager.Dispatch(ClientEvent.Grunt, clientGender);
                }

                if (!sicknessParticlesActivated)
                {
                    sicknessParticlesActivated = true;
                    SicknessVFX.Play();
                }
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

            if (SicknessVFX.isEmitting)
            {
                var emission1 = SicknessVFX.emission;
                emission1.rateOverTime = 1.5f - (waitingTimer / (MaxWaitingSeconds / 2f));

                var emission2 = Sickness2VFX.emission;
                emission2.rateOverTime = 1.5f - (waitingTimer / (MaxWaitingSeconds / 2f));

                var sparkEmission = SicknessSparkVFX.emission;
                sparkEmission.rateOverTime = 2f - waitingTimer / (MaxWaitingSeconds / 2f) * 1.5f;

                spriteRenderer.material.SetFloat("_SicknessColorMultiplier", 1f - waitingTimer / (MaxWaitingSeconds / 2f) * 0.5f);
            }
        }

        IEnumerator DiedClientCoroutine()
        {
            SicknessVFX.Stop();
            spriteRenderer.material.SetFloat("_SicknessColorMultiplier", 0);
            clientDied = true;
            animator.SetBool(animatorDiedParameter, true);
            EventManager.Dispatch(ClientEvent.Dying);
            DeathVFX.Play();
            canvas.enabled = false;
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.material.SetFloat("_DeathColorMultiplier", 1);
            yield return new WaitForSeconds(0.83f - 0.3f);
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(2.17f - 0.3f);
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
            SicknessVFX.Stop();
            spriteRenderer.material.SetFloat("_SicknessColorMultiplier", 0);
            clientServed = true;
            //clientSpriteRenderer.sprite = happyClientSprite[(int)clientGender];
            animator.SetBool(animatorRecuperatedParameter, clientServed);
            EventManager.Dispatch(ClientEvent.Served);
            GetComponent<BoxCollider2D>().enabled = false;
            healingVFX.Play();
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.material.SetFloat("_HealColorMultiplier", 1);
            float green = 5f;
            while(green > 0)
            {
                yield return new WaitForSeconds(0.126f);
                green -= 1f;
                spriteRenderer.material.SetFloat("_HealColorMultiplier", green / 5f);
            }
            yield return new WaitForSeconds(1.31f);
            bc.enabled = false;
            Destroy(gameObject);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)waitingTimer + 10; //TODO Burned Variable
    }
}