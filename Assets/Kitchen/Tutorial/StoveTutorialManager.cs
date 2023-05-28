using System.Collections;
using UnityEngine;
using Events;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

namespace Kitchen.Tutorial
{
    public class StoveTutorialManager : MonoBehaviour
    {
        private HandTutorial handTutorial;

        [SerializeField] Animator playerTextObject;
        const string animParamBoolDisplay = "Display";

        [SerializeField] LocalizeStringEvent playerText;
        [SerializeField] TutorialTextScriptableObject textsDatabase;

        private enum TutorialActors
        {
            Pequi = 0, Water = 1, Mortar = 2, Shaker = 3, Client = 4, PotionResult = 5, Carnauba =6, Stove=7, Trash =8
        }
        public PlayableDirector finalTimeline;
        //Assing Pequi Water an client from inspector
        [SerializeField] private Transform[] tutorialElements = new Transform[7];
        [SerializeField] private StarsData starsData;

        [ContextMenuItem("MoveObject", "Move")]
        [SerializeField] Transform currentPos;

        [SerializeField] IngredientData pequi;
        [SerializeField] IngredientData carnauba;
        [SerializeField] TrashTutorial trashTutorialPopUp;
        private PotionController tutorialShaker;

        //[SerializeField] private GameObject updatedMythsGO;

        private void Awake()
        {
            EventManager.AddListener(IngredientState.Cooking, HideHand);
            EventManager.AddListener<IngredientData>(IngredientState.Cooked, CallAfterCookIngredient);
            EventManager.AddListener<IngredientData>(PotionEvent.AddIngredient, CallAfterAddIngredientToShaker);
            EventManager.AddListener(PotionEvent.Shake, HideHand);
            EventManager.AddListener(PotionEvent.AddWater, CallShakeIngredients);
            EventManager.AddListener(PotionEvent.Poof, CallServeToClient);
            EventManager.AddListener(ClientEvent.Served, FinalCoroutineStart);
            EventManager.AddListener(IngredientState.Burnt, CallThrowBurntIngredient);
            EventManager.AddListener(TrashEvent.Throw, CallTryAgain);

            //updatedMythsGO.SetActive(false);
            handTutorial = FindObjectOfType<HandTutorial>();
        }

        void OnDestroy()
        {
            EventManager.RemoveListener(IngredientState.Cooking, HideHand);
            EventManager.RemoveListener<IngredientData>(IngredientState.Cooked, CallAfterCookIngredient);
            EventManager.RemoveListener<IngredientData>(PotionEvent.AddIngredient, CallAfterAddIngredientToShaker);
            EventManager.RemoveListener(PotionEvent.Shake, HideHand);
            EventManager.RemoveListener(PotionEvent.AddWater, CallShakeIngredients);
            EventManager.RemoveListener(PotionEvent.Poof, CallServeToClient);
            EventManager.RemoveListener(ClientEvent.Served, FinalCoroutineStart);
            EventManager.RemoveListener(IngredientState.Burnt, CallThrowBurntIngredient);
            EventManager.RemoveListener(TrashEvent.Throw, CallTryAgain);
        }

        private void Start()
        {
            LevelManager.CurrentLevel = 1;
            tutorialElements[(int)TutorialActors.Mortar] = GameObject.Find("Mortar(Clone)").GetComponent<Transform>().parent;
            tutorialElements[(int)TutorialActors.Shaker] = GameObject.Find("Potion(Clone)").GetComponent<Transform>();
            tutorialElements[(int)TutorialActors.Stove] = GameObject.Find("Stove(Clone)").GetComponent<Transform>().parent;
            tutorialElements[(int)TutorialActors.PotionResult] = GameObject.Find("Result").GetComponent<Transform>();

            tutorialShaker = tutorialElements[(int)TutorialActors.Shaker].GetComponent<PotionController>();

            DisableAllColliders();
        }

        void SwitchObjectCollider(Transform collider, bool enabled) => collider.GetComponentInChildren<BoxCollider2D>().enabled = enabled;

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
            foreach(var element in tutorialElements)
                SwitchObjectCollider(element, false);
        }

        public void EnableVignetteFirstTime()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Carnauba], tutorialElements[(int)TutorialActors.Stove] }));
            DisplayDialogueBox(textsDatabase.tutorial2Texts[1]);
        }

        private void CallAfterCookIngredient(IngredientData ingredientData)
        {
            Transform[] elements = { };
           if (ingredientData == carnauba)
           {
                elements = new Transform[] { tutorialElements[(int)TutorialActors.Stove], tutorialElements[(int)TutorialActors.Shaker] };   
                DisplayDialogueBox(textsDatabase.tutorial2Texts[2]);
           }                  
           else if(ingredientData == pequi)
           {
                elements = new Transform[] { tutorialElements[(int)TutorialActors.Mortar], tutorialElements[(int)TutorialActors.Shaker] };
                tutorialShaker.SwitchShakerEnabled(false);
           }
            StartCoroutine(StartTutorialSequence(elements));
        }

        private void CallAfterAddIngredientToShaker(IngredientData ingredientData) 
        {
            Transform[] elements = { };
            if (ingredientData == carnauba)
            {
                elements = new Transform[] { tutorialElements[(int)TutorialActors.Pequi], tutorialElements[(int)TutorialActors.Mortar] };
                DisplayDialogueBox(textsDatabase.tutorial2Texts[4]);
            }
            else if (ingredientData == pequi)
            {
                elements = new Transform[] { tutorialElements[(int)TutorialActors.Water], tutorialElements[(int)TutorialActors.Shaker] };
                DisplayDialogueBox(textsDatabase.tutorial2Texts[5]);
            }
            StartCoroutine(StartTutorialSequence(elements));
        }


        private void CallShakeIngredients() 
        {
            tutorialShaker.SwitchShakerEnabled(true);
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Shaker] }));
            DisplayDialogueBox(textsDatabase.tutorial2Texts[6]);
        }

        private void CallServeToClient()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.PotionResult], tutorialElements[(int)TutorialActors.Client] }));
            DisplayDialogueBox(textsDatabase.tutorial2Texts[7]);
        }

        private void CallThrowBurntIngredient()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Stove], tutorialElements[(int)TutorialActors.Trash] }));
            DisplayDialogueBox(textsDatabase.tutorial2Texts[8]);
        }

        private void CallTryAgain()
        {
            StartCoroutine(StartTutorialSequence(new Transform[] { tutorialElements[(int)TutorialActors.Carnauba], tutorialElements[(int)TutorialActors.Stove]  }));
            DisplayDialogueBox(textsDatabase.tutorial2Texts[3]);
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
            //yield return StartCoroutine(DisplayMythsUpdatedPopUP());
            EventManager.Dispatch(GlobalTutorialEvent.Tutorial5Completed, 5);
            LoadKitchen1();
        }

        //IEnumerator DisplayMythsUpdatedPopUP()
        //{
        //    updatedMythsGO.SetActive(true);
        //    yield return new WaitForSecondsRealtime(3f);
        //    updatedMythsGO.SetActive(false);
        //}
        void LoadKitchen1() => SceneManager.LoadScene("Kitchen1");

        void DisplayDialogueBox(LocalizedString text)
        {
            playerTextObject.SetBool(animParamBoolDisplay, true);
            playerText.StringReference = text;
        }
    }
}