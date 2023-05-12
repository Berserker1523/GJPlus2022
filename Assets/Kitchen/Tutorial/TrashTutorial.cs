using Events;
using UnityEngine;

namespace Kitchen.Tutorial
{
    public class TrashTutorial : MonoBehaviour
    {
        private void Awake()
        {
            EventManager.AddListener(PotionEvent.FailedRecipe, ShowPotionTutorial);
        }

        private void ShowPotionTutorial()
        {
            gameObject.SetActive(true);
            EventManager.AddListener(TrashEvent.Throw, EndPotionTutorial);
        }

        private void EndPotionTutorial()
        {
            gameObject.SetActive(false);
            EventManager.Dispatch(GlobalTrigerableTutorialEvent.TrashTutorialTriggered, (int)GlobalTrigerableTutorialEvent.TrashTutorialTriggered);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(PotionEvent.FailedRecipe, ShowPotionTutorial);
            EventManager.RemoveListener(TrashEvent.Throw, EndPotionTutorial);           
        }
    }

}
