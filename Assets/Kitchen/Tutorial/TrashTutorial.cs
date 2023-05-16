using Events;
using UnityEngine;

namespace Kitchen.Tutorial
{
    public class TrashTutorial : MonoBehaviour
    {
        [SerializeField] HandTutorial handTutorial;
        TrashController trashController;
        private void Awake()
        {
            EventManager.AddListener<Transform>(PotionEvent.FailedRecipe, ShowPotionTutorial);
            trashController = FindObjectOfType<TrashController>();
        }

        private void ShowPotionTutorial(Transform failedRecipeTransform)
        {
            gameObject.SetActive(true);
            handTutorial.StartNewSequence( new Transform[] { failedRecipeTransform, trashController.transform });
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial,true);
            EventManager.AddListener(TrashEvent.Throw, EndPotionTutorial);
        }

        private void EndPotionTutorial()
        {
            gameObject.SetActive(false);
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial,false);
            //EventManager.Dispatch(GlobalTrigerableTutorialEvent.TrashTutorialTriggered, (int)GlobalTrigerableTutorialEvent.TrashTutorialTriggered);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<Transform>(PotionEvent.FailedRecipe, ShowPotionTutorial);
            EventManager.RemoveListener(TrashEvent.Throw, EndPotionTutorial);           
        }
    }

}
