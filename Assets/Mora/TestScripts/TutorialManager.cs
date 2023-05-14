using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Kitchen;
using Events;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

namespace Kitchen.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        private Volume vignetteVolume;
        private Vignette vignetteInstance;
        private HandTutorial handTutorial;

        private enum TutorialActors
        {
            Pequi = 0, Water = 1, Mortar = 2, Shaker = 3, Client = 4, PotionResult = 5,
        }
        public PlayableDirector finalTimeline;
        //Assing Pequi Water an client from inspector
        [SerializeField] private Transform[] tutorialElements = new Transform[5];
        [SerializeField] private StarsData starsData;

        //Post procees Intensity
        private float activeIntensity = 1f, initialIntensity;

        private float waitToShowTimer = 1f, waitToMoveTimer = 3f;

        private float standardWaitStep = 0.1f;
        WaitForSeconds coroutineWaitStep;
        WaitForSeconds waitSecondsToShow;
        WaitForSeconds halfSecond = new WaitForSeconds(0.5f);

        [ContextMenuItem("MoveObject", "Move")]
        [SerializeField] Transform currentPos;

        Vector2 targetResolution = new Vector2(1920f, 1080f);

        [SerializeField] private GameObject updatedMythsGO;

        private void Awake()
        {
            EventManager.AddListener(IngredientState.Cooking, HideHand);
            EventManager.AddListener(IngredientState.Cooked, CallSecondCoroutine);
            EventManager.AddListener(PotionEvent.AddIngredient, CallThirdCoroutine);
           // EventManager.AddListener(PotionEvent.AddIngredient, HideHand);
            EventManager.AddListener(PotionEvent.AddWater, callFourthCoroutine);
            EventManager.AddListener(PotionEvent.Poof, callFifhtCoroutine);
            EventManager.AddListener(ClientEvent.Served, FinalCoroutineStart);

            updatedMythsGO.SetActive(false);
            handTutorial = FindObjectOfType<HandTutorial>();
        }

        void OnDestroy()
        {
            EventManager.RemoveListener(IngredientState.Cooking, HideHand);
            EventManager.RemoveListener(IngredientState.Cooked, CallSecondCoroutine);
            EventManager.RemoveListener(PotionEvent.AddIngredient, CallThirdCoroutine);
            //EventManager.RemoveListener(PotionEvent.AddIngredient, HideHand);
            EventManager.RemoveListener(PotionEvent.AddWater, callFourthCoroutine);
            EventManager.RemoveListener(PotionEvent.Poof, callFifhtCoroutine);
            EventManager.RemoveListener(ClientEvent.Served, FinalCoroutineStart);
        }

        private void Start()
        {
            vignetteVolume = this.gameObject.GetComponent<Volume>();
            vignetteVolume.profile.TryGet<Vignette>(out vignetteInstance);

            vignetteInstance.intensity.value = 0f;
            waitSecondsToShow = new WaitForSeconds(waitToShowTimer);
            LevelManager.CurrentLevel = 0;

            tutorialElements[(int)TutorialActors.Mortar] = GameObject.Find("Mortar(Clone)").GetComponent<Transform>();
            tutorialElements[(int)TutorialActors.Shaker] = GameObject.Find("Potion(Clone)").GetComponent<Transform>();
            //tutorialElements[(int)TutorialActors.Stove] = GameObject.Find("Stove(Clone)").GetComponent<Transform>();
            tutorialElements[(int)TutorialActors.PotionResult] = GameObject.Find("Result").GetComponent<Transform>();

            foreach (var tutorialActor in tutorialElements)
                SwitchObjectCollider(tutorialActor, false);

            targetResolution = new Vector2(Screen.width, Screen.height);
        }

        void SwitchObjectCollider(Transform collider, bool enabled) => collider.GetComponent<BoxCollider2D>().enabled = enabled;
        void UnusefulMethod()
        {
            //TODO Replace Unuseful method to the correspondant listener Method
            Debug.Log(" Replace Unuseful method to the correspondant listener Method");
        }

        private void HideHand()
        {
            handTutorial.SwitchEnableHand(false);
            foreach (var element in tutorialElements)
                SwitchObjectCollider(element, false);
        }

        public void EnableVignetteFirstTime()
        {
            handTutorial.StartNewSequence(new Transform[] { tutorialElements[(int)TutorialActors.Pequi], tutorialElements[(int)TutorialActors.Mortar] });

            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Pequi], true);
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], true);
        }

        private void CallSecondCoroutine()
        {
            handTutorial.StartNewSequence(new Transform[] { tutorialElements[(int)TutorialActors.Mortar], tutorialElements[(int)TutorialActors.Shaker] });
            //activates colliders
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], true);
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], true);
        }
       
        private void CallThirdCoroutine() 
        {
            handTutorial.StartNewSequence(new Transform[] { tutorialElements[(int)TutorialActors.Water], tutorialElements[(int)TutorialActors.Shaker] });

            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Water], true);
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], true);
        }


        private void callFourthCoroutine() 
        {
            handTutorial.StartNewSequence(new Transform[] { tutorialElements[(int)TutorialActors.Shaker] });
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], true);
        }

        private void callFifhtCoroutine()
        {
            handTutorial.StartNewSequence(new Transform[] { tutorialElements[(int)TutorialActors.PotionResult], tutorialElements[(int)TutorialActors.Client] });

            SwitchObjectCollider(tutorialElements[(int)TutorialActors.PotionResult], true);
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Client], true);
        }


        private void FinalCoroutineStart() =>
            StartCoroutine(FinalCoroutine());

        public IEnumerator FinalCoroutine()
        {
            HideHand();
            yield return new WaitForSeconds(0.1f);
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], false);
            SwitchObjectCollider(tutorialElements[(int)TutorialActors.Client], false);
            finalTimeline.Play();
            StartCoroutine(endOfTutorial());
        }

        public IEnumerator endOfTutorial()
        {
            yield return new WaitForSeconds(14f);
            EventManager.Dispatch(GlobalTutorialEvent.Tutorial1Completed);
            yield return StartCoroutine(DisplayMythsUpdatedPopUP());
            LoadKitchen1();

        }
        void LoadKitchen1() => SceneManager.LoadScene("Tutorial1");

        IEnumerator DisplayMythsUpdatedPopUP()
        {
            updatedMythsGO.SetActive(true);
            yield return new WaitForSecondsRealtime(3f);
            updatedMythsGO.SetActive(false);
        }
    }


}