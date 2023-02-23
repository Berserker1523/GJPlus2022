using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Kitchen
{
    public class EndLevelPopUp : MonoBehaviour
    {
        [SerializeField] GameObject upgradesPopUp;
        [SerializeField] string mainMenuSceneName = "MainMenu";
        [SerializeField] string kitchenScene = "Kitchen";
        [SerializeField] LocalizeStringEvent victoryDefeatText;
        [SerializeField] LocalizeStringEvent victoryDefeatTextDilate;
        [SerializeField] Button button;
        [SerializeField] LocalizeStringEvent buttonText;

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
            gameObject.SetActive(true);
            victoryDefeatText.SetEntry("Title_VictoryText");
            victoryDefeatTextDilate.SetEntry("Title_VictoryText");
            buttonText.SetEntry("Button_Continue");
            button.onClick.AddListener(NextLevel);
        }

        private void HandleLost()
        {
            gameObject.SetActive(true);
            victoryDefeatText.SetEntry("Title_DefeatText");
            victoryDefeatTextDilate.SetEntry("Title_DefeatText");
            buttonText.SetEntry("Button_TryAgain");
            button.onClick.AddListener(TryAgain); 
        }

        private void Start()
        {
            currentLevel = LevelManager.CurrentLevel;
            UpgradesPopUp(false);
            gameObject.SetActive(false);
        }

        public void NextLevel()
        {
            if (LevelManager.CurrentLevel == 5) //TODO Burned Variable
            {
                PlayerPrefs.DeleteAll();
                LevelManager.CurrentLevel = 1;
                SceneManager.LoadScene("Credits", LoadSceneMode.Single);
               
            }
            else
                LevelManager.CurrentLevel++;
            SceneManager.LoadScene($"{kitchenScene}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        }

        public void TryAgain()
        {
            LevelManager.CurrentLevel = currentLevel;
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
