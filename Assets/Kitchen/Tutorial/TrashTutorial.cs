using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Kitchen.Tutorial
{
    public class TrashTutorial : MonoBehaviour
    {
        [SerializeField] HandTutorial handTutorial;
        [SerializeField] Canvas handCanvas;
        TrashController trashController;
        LocalizeStringEvent stringEvent;

        LocalizedString potionString ;
        LocalizedString burntString;

        bool alreadyFailedPotionDisplayed;
        bool alreadyBurntFoodDisplayed;

        private void Awake()
        {
            EventManager.AddListener<Transform>(PotionEvent.FailedRecipe, ShowPotionTutorial);
            EventManager.AddListener<Transform>(IngredientState.Burnt, ShowBurntTutorial);
            trashController = FindObjectOfType<TrashController>();
            stringEvent = GetComponentInChildren<LocalizeStringEvent>();
            potionString = new LocalizedString("Tutorial", "TrashTutorial1");
            burntString = new LocalizedString("Tutorial", "TrashTutorial2");
            handTutorial = Instantiate(handCanvas).GetComponentInChildren<HandTutorial>();
        }


        private void ShowPotionTutorial(Transform toTrashTranform)
        {
            if (alreadyFailedPotionDisplayed)
                return;
            alreadyFailedPotionDisplayed = true;
            stringEvent.StringReference = potionString;
            TutorialGeneralBehaviour(toTrashTranform);
        }

        private void ShowBurntTutorial(Transform toTrashTranform)
        {
            if (alreadyBurntFoodDisplayed)
                return;

            alreadyBurntFoodDisplayed= true;
            stringEvent.StringReference = burntString;
            TutorialGeneralBehaviour(toTrashTranform);           
        }

        private void TutorialGeneralBehaviour(Transform toTrashTranform)
        {
            gameObject.SetActive(true);
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial, true);
            Transform[] tutorialTargets = { toTrashTranform, trashController.transform };
            handTutorial.StartNewSequence(tutorialTargets);
            EnableColliders(tutorialTargets);
            EventManager.AddListener(TrashEvent.Throw, EndPotionTutorial);
        }

        private void EndPotionTutorial()
        {
            handTutorial.SwitchEnableHand(false);
            gameObject.SetActive(false);
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial,false);
            //EventManager.Dispatch(GlobalTrigerableTutorialEvent.TrashTutorialTriggered, (int)GlobalTrigerableTutorialEvent.TrashTutorialTriggered);
        }

        private void EnableColliders(Transform[] tutorialTargets)
        {
            foreach(var tranform in tutorialTargets)
                tranform.GetComponent<Collider2D>().enabled = true;
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<Transform>(PotionEvent.FailedRecipe, ShowPotionTutorial);
            EventManager.RemoveListener<Transform>(IngredientState.Burnt, ShowBurntTutorial);
        }

        public void ReEnableBurntTrigger() => alreadyBurntFoodDisplayed = true;
    }

}
