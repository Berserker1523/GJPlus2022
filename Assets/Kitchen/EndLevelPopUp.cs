using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kitchen
{
    public class EndLevelPopUp : MonoBehaviour
    {
        [SerializeField] private Image[] stars = new Image[3];
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI titleDilateText;
        [SerializeField] private Color wonTitleColor;
        [SerializeField] private Color lostTitleColor;
        [SerializeField] private TextMeshProUGUI adviceText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Button positiveButton;
        [SerializeField] private TextMeshProUGUI positiveButtonText;

        [SerializeField] private LocalizedString levelCompletedString;
        [SerializeField] private LocalizedString levelFailedString;
        [SerializeField] private LocalizedString continueString;
        [SerializeField] private LocalizedString retryString;

        private MoneyUI moneyUI;

        private void Awake()
        {
            moneyUI = FindObjectOfType<MoneyUI>();

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
            titleText.text = levelCompletedString.GetLocalizedString();
            titleDilateText.text = levelCompletedString.GetLocalizedString();
            titleText.color = wonTitleColor;

            //TODO set advice text

            positiveButtonText.text = continueString.GetLocalizedString();
            positiveButton.onClick.AddListener(NextLevel);
            moneyText.text = moneyUI.GetCurrentLevelMoney().ToString();
        }

        private void HandleLost()
        {
            gameObject.SetActive(true);
            titleText.text = levelFailedString.GetLocalizedString();
            titleDilateText.text = levelFailedString.GetLocalizedString();
            titleText.color = lostTitleColor;

            //TODO set advice text

            positiveButtonText.text = retryString.GetLocalizedString();
            positiveButton.onClick.AddListener(TryAgain);
            moneyText.text = moneyUI.GetCurrentLevelMoney().ToString();
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
            SceneManager.LoadScene($"{SceneName.Kitchen}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        }

        public void TryAgain()
        {
            SceneManager.LoadScene($"{SceneName.Kitchen}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        }
    }
}
