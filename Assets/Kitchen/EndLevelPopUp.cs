using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kitchen
{
    public class EndLevelPopUp : MonoBehaviour
    {
        [SerializeField] GameObject upgradesPopUp;
        [SerializeField] private GameObject continueButton;
        [SerializeField] string mainMenuSceneName = "MainMenu";
        [SerializeField] string kitchenScene = "Kitchen";

        private int currentLevel;

        private void Awake()
        {
            EventManager.AddListener(GameStatus.Lost, HandleLost);
            EventManager.AddListener(GameStatus.Won, HandleWon);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(GameStatus.Lost, HandleLost);
            EventManager.RemoveListener(GameStatus.Won, HandleWon);
        }

        private void HandleWon()
        {
            if (LevelManager.CurrentLevel == 5)
            {
                PlayerPrefs.DeleteAll();
                LevelManager.CurrentLevel = 1;
            }
            else
                LevelManager.CurrentLevel++;
        }

        private void HandleLost()
        {
            continueButton.SetActive(false);
        }

        private void Start()
        {
            currentLevel = LevelManager.CurrentLevel;
            UpgradesPopUp(false);
            gameObject.SetActive(false);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene($"{kitchenScene}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        }

        public void TryAgain()
        {
            SceneManager.LoadScene($"{kitchenScene}{currentLevel}", LoadSceneMode.Single);
        }

        public void UpgradesPopUp(bool showPopUP)
        {
            upgradesPopUp.SetActive(showPopUP);
        }

        public void ExitGame()
        {
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
