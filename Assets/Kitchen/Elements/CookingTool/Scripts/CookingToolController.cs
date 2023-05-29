using Events;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Kitchen
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(DragView))]
    [RequireComponent(typeof(DropView))]
    public class CookingToolController : MonoBehaviour, IReleaseable
    {
        [SerializeField] private CookingToolData cookingToolData;
        [SerializeField] private ParticleSystem shakingMortarParticle;
        [SerializeField] private ParticleSystem cookingParticle;
        [SerializeField] private ParticleSystem overcookingParticle;
        [SerializeField] private ParticleSystem overcookedParticle;
        private bool inTutorial;

        private SpriteRenderer spriteRenderer;
        private DropView dropView;
        private CookingTimerView timer;

        private Sprite initialSprite;
        private FMOD.Studio.EventInstance cookingSound;
        private FMOD.Studio.EventInstance burningSound;
        private IEnumerator startBurningSoundRoutine;

        public CookingIngredient CurrentCookingIngredient { get; private set; }
        public CookingToolData CookingToolData => cookingToolData;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            dropView = GetComponent<DropView>();
            timer = GetComponentInChildren<CookingTimerView>();
            timer.gameObject.SetActive(false);

            initialSprite = spriteRenderer.sprite;
            dropView.OnDropped += HandleDropped;
            dropView.IsDraggedObjectInteractableWithMe = IsDraggedObjectInteractableWithMe;

            if (shakingMortarParticle != null)
                shakingMortarParticle.Stop();

            SceneManager.sceneLoaded += StopCookingSoundsOnNewSceneLoaded;
            EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, StopTimers);
        }

        private void StopTimers(bool tutorial) => inTutorial = tutorial;

        private void OnDestroy()
        {
            dropView.OnDropped -= HandleDropped;
            EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, StopTimers);
            SceneManager.sceneLoaded -= StopCookingSoundsOnNewSceneLoaded;
            StopCookingSoundsOnNewSceneLoaded(new(), new());
        }


        private void Update()
        {
            if (CurrentCookingIngredient == null || inTutorial)
                return;

            CurrentCookingIngredient.currentCookingSeconds += Time.deltaTime;
            if (CurrentCookingIngredient.state == IngredientState.Cooked)
            {
                timer.SetFillAmount((CurrentCookingIngredient.currentCookingSeconds - cookingToolData.cookingSeconds) / (cookingToolData.burningSeconds - cookingToolData.cookingSeconds));
                spriteRenderer.material.SetFloat("_Temperature", ((CurrentCookingIngredient.currentCookingSeconds - cookingToolData.cookingSeconds) / (cookingToolData.burningSeconds - cookingToolData.cookingSeconds)));
            }
            else
                timer.SetFillAmount(CurrentCookingIngredient.currentCookingSeconds / cookingToolData.cookingSeconds);

            if (CurrentCookingIngredient.state == IngredientState.Raw && CurrentCookingIngredient.currentCookingSeconds >= cookingToolData.cookingSeconds)
            {
                CurrentCookingIngredient.state = IngredientState.Cooked;
                spriteRenderer.sprite = cookingToolData.cookingToolName == CookingToolName.Stove ? CurrentCookingIngredient.data.stoveCookedSprite : CurrentCookingIngredient.data.mortarCrushedSprite;
                EventManager.Dispatch(IngredientState.Cooked, CurrentCookingIngredient.data);
                if (cookingToolData.cookingToolName == CookingToolName.Mortar)
                {
                    shakingMortarParticle.Stop();
                    timer.gameObject.SetActive(false);
                    cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
                else
                {
                    burningSound = FMODUnity.RuntimeManager.CreateInstance(SoundsManager.BurningFood);
                    startBurningSoundRoutine = StartBurningSoundRoutine();
                    StartCoroutine(startBurningSoundRoutine);
                    timer.SetBurning();
                    if (overcookedParticle != null)
                        overcookingParticle.Play();
                }
            }
            else if (CurrentCookingIngredient.state == IngredientState.Cooked && CurrentCookingIngredient.currentCookingSeconds >= cookingToolData.burningSeconds)
            {
                CurrentCookingIngredient.state = IngredientState.Burnt;
                overcookedParticle.Play();
                spriteRenderer.material.SetFloat("_Temperature", 0f);
                spriteRenderer.sprite = CurrentCookingIngredient.data.stoveBurntSprite;
                timer.gameObject.SetActive(false);
                EventManager.Dispatch(IngredientState.Burnt, transform);
                cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                burningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

        private bool IsDraggedObjectInteractableWithMe(PointerEventData pointerEventData)
        {
            if (CurrentCookingIngredient != null)
                return false;

            if (!pointerEventData.pointerDrag.TryGetComponent(out IngredientController ingredientController))
                return false;

            if (!ingredientController.IngredientData.necessaryCookingTool.HasFlag(cookingToolData.cookingToolName))
                return false;

            return true;
        }

        private void HandleDropped(PointerEventData pointerEventData)
        {
            if (!IsDraggedObjectInteractableWithMe(pointerEventData))
                return;

            IngredientController ingredientController = pointerEventData.pointerDrag.GetComponent<IngredientController>();
            CurrentCookingIngredient = new(ingredientController.IngredientData);
            spriteRenderer.sprite = cookingToolData.cookingToolName == CookingToolName.Stove ? CurrentCookingIngredient.data.stoveRawSprite : CurrentCookingIngredient.data.mortarRawSprite;
            PlayCookingSound();
            timer.gameObject.SetActive(true);
            timer.SetCooking();
            EventManager.Dispatch(IngredientState.Cooking);
        }

        private void PlayCookingSound()
        {
            if (cookingToolData.cookingToolName == CookingToolName.Stove)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Enciende Sarten");
                cookingSound.start();
                cookingSound.setParameterByName("Cocinando", 1);
                cookingParticle.Play();
            }
            else if (cookingToolData.cookingToolName == CookingToolName.Mortar)
            {
                cookingSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Mortero");
                cookingSound.start();
                shakingMortarParticle.Play();
            }
        }

        public void Release()
        {
            CurrentCookingIngredient = null;
            spriteRenderer.sprite = initialSprite;
            if (startBurningSoundRoutine != null)
                StopCoroutine(startBurningSoundRoutine);
            cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            burningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            spriteRenderer.material.SetFloat("_Temperature", 0f);
            timer.gameObject.SetActive(false);

            if (shakingMortarParticle != null)
                shakingMortarParticle.Stop();
            if (cookingParticle != null)
                cookingParticle.Stop();
            if (overcookingParticle != null)
                overcookingParticle.Stop();
            if (overcookedParticle != null)
                overcookedParticle.Stop();
        }

        public void StopCookingSoundsOnNewSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            cookingSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            burningSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        private IEnumerator StartBurningSoundRoutine()
        {
            yield return new WaitForSeconds(1f);
            burningSound.start();
        }
    }

}
