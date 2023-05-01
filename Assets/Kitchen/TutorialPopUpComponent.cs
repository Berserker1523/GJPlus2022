using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
      public class TutorialPopUpComponent : MonoBehaviour
    {
        [SerializeField] public int tutorialId;
        [SerializeField] Button closeButton;
        // Start is called before the first frame update

        private void Awake()
        {
            EventManager.AddListener(GameStatus.Won, DispatchTutorial);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(GameStatus.Won, DispatchTutorial);
        }

        void Start()
        {
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial, true);
            closeButton = GetComponentInChildren<Button>();
            closeButton.onClick.AddListener(StartGame);
        }

        public void StartGame()
        {
            EventManager.Dispatch(GlobalTutorialEvent.inTutorial, false);
            gameObject.SetActive(false);
        }

        public void DispatchTutorial()
        {
            EventManager.Dispatch((GlobalTutorialEvent)tutorialId, tutorialId);
        }

    }
}