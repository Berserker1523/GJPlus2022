using System.Collections;
using UnityEngine;
using Events;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

namespace Kitchen.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        private HandTutorial handTutorial;

        [SerializeField] Animator playerTextObject;
        const string animParamBoolDisplay = "Display";

        [SerializeField] LocalizeStringEvent playerText;
        [SerializeField] TutorialTextScriptableObject textsDatabase;

        private enum TutorialActors
        {
            Pequi = 0, Water = 1, Mortar = 2, Shaker = 3, Client = 4, PotionResult = 5,
        }
        public PlayableDirector finalTimeline;
        //Assing Pequi Water an client from inspector
        [SerializeField] private Transform[] tutorialElements = new Transform[5];
        [SerializeField] private GameData starsData;

        [ContextMenuItem("MoveObject", "Move")]
        [SerializeField] Transform currentPos;

        //[SerializeField] private GameObject updatedMythsGO;

        [SerializeField] private bool replaying;
        [SerializeField] GameObject pauseMenu;

        private void Awake()
        {
            EventManager.AddListener(IngredientState.Cooking, HideHand);
            EventManager.AddListener(IngredientState.Cooked, CallSecondCoroutine);
            EventManager.AddListener(PotionEvent.AddIngredient, CallThirdCoroutine);
            EventManager.AddListener(PotionEvent.Shake, HideHand);
            EventManager.AddListener(PotionEvent.AddWater, callFourthCoroutine);
            EventManager.AddListener(PotionEvent.Poof, callFifhtCoroutine);
            EventManager.AddListener(ClientEvent.Served, FinalCoroutineStart);
            EventManager.AddListener(GlobalTutorialEvent.replayingTutorial, SetTutorialReplay);

            //updatedMythsGO.SetActive(false);
            handTutorial = FindObjectOfType<HandTutorial>();
        }

        void OnDestroy()
        {
            EventManager.RemoveListener(IngredientState.Cooking, HideHand);
            EventManager.RemoveListener(IngredientState.Cooked, CallSecondCoroutine);
            EventManager.RemoveListener(PotionEvent.AddIngredient, CallThirdCoroutine);
            EventManager.RemoveListener(PotionEvent.Shake, HideHand);
            EventManager.RemoveListener(PotionEvent.AddWater, callFourthCoroutine);
            EventManager.RemoveListener(PotionEvent.Poof, callFifhtCoroutine);
            EventManager.RemoveListener(ClientEvent.Served, FinalCoroutineStart);
            EventManager.RemoveListener(GlobalTutorialEvent.replayingTutorial, SetTutorialReplay);
        }

        private void Start()
        {
            LevelManager.CurrentLevel = 0;
            tutorialElements[(int)TutorialActors.Mortar] = GameObject.Find("Mortar(Clone)").GetComponent<Transform>();
            tutorialElements[(int)TutorialActors.Shaker] = GameObject.Find("Potion(Clone)").GetComponent<Transform>();
            tutorialElements[(int)TutorialActors.PotionResult] = GameObject.Find("Result").GetComponent<Transform>();

            DisableAllColliders();
        }

        void SwitchObjectCollider(Transform collider, bool enabled) => collider.GetComponent<BoxCollider2D>().enabled = enabled;

        private void HideHand()
        {
            handTutorial.SwitchEnableHand(false);
            DisableAllColliders();
            playerTextObject.SetBool(animParamBoolDisplay, false);
        }

        private IEnumerator StartTutorialSequence(Transform[] elements)
        {
            yield return new WaitForSeconds(0.1f);
            DisableAllColliders();
            handTutorial.StartNewSequence(elements);
            foreach (Transform element in elements)
                SwitchObjectCollider(element, true);
        }
        private void DisableAllColliders()
        {
            foreach (var element in tutorialElements)
                SwitchObjectCollider(element, false);
        }

        public void EnableVignetteFirstTime()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Pequi], tutorialElements[(int)TutorialActors.Mortar] }));
            DisplayDialogueBox(textsDatabase.tutorial1Texts[0]);
        }

        private void CallSecondCoroutine()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Mortar], tutorialElements[(int)TutorialActors.Shaker] }));
            DisplayDialogueBox(textsDatabase.tutorial1Texts[1]);
        }
       
        private void CallThirdCoroutine() 
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Water], tutorialElements[(int)TutorialActors.Shaker] }));
            DisplayDialogueBox(textsDatabase.tutorial1Texts[2]);
        }


        private void callFourthCoroutine() 
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Shaker] }));
            DisplayDialogueBox(textsDatabase.tutorial1Texts[3]);
        }

        private void callFifhtCoroutine()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.PotionResult], tutorialElements[(int)TutorialActors.Client] }));
            DisplayDialogueBox(textsDatabase.tutorial1Texts[4]);
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
            EventManager.Dispatch(GlobalTutorialEvent.Tutorial0Completed,0);
            //yield return StartCoroutine(DisplayMythsUpdatedPopUP());
            LoadKitchen1();

        }
        void LoadKitchen1()
        {
            SceneName scene;
            if (replaying)
                scene = SceneName.TutorialsSelector;
            else
                scene = SceneName.Tutorial1;

            SceneManager.LoadScene(scene.ToString());
        }

       /* IEnumerator DisplayMythsUpdatedPopUP()
        {
            updatedMythsGO.SetActive(true);
            yield return new WaitForSecondsRealtime(3f);
            updatedMythsGO.SetActive(false);
        }*/

        void DisplayDialogueBox(LocalizedString text)
        {
            playerTextObject.SetBool(animParamBoolDisplay, true);
            playerText.StringReference = text;
        }

        void SetTutorialReplay()
        {
            replaying = true;
            pauseMenu.SetActive(true);
        }
    }
}