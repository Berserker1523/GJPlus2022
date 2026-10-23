using Events;
using System.Collections;
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
            Transform[] tutorialTargets = { toTrashTranform.parent, trashController.transform };
            handTutorial.StartNewSequence(tutorialTargets);
            StartCoroutine(AllowDragOnTutorialTarget(toTrashTranform));
            EventManager.AddListener(TrashEvent.Throw, EndPotionTutorial);
        }

        IEnumerator AllowDragOnTutorialTarget(Transform toTrashTranform)
        {
            yield return new WaitForSeconds(0.5f);
            toTrashTranform.GetComponent<DragView>().ForceAllowDragging();
        }

        private void EndPotionTutorial()
        {
            handTutorial.SwitchEnableHand(false);
            gameObject.SetActive(false);
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial,false);
            EventManager.RemoveListener(TrashEvent.Throw, EndPotionTutorial);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<Transform>(PotionEvent.FailedRecipe, ShowPotionTutorial);
            EventManager.RemoveListener<Transform>(IngredientState.Burnt, ShowBurntTutorial);
            EventManager.RemoveListener(TrashEvent.Throw, EndPotionTutorial);
        }

        public void ReEnableBurntTrigger() => alreadyBurntFoodDisplayed = true;
    }
}