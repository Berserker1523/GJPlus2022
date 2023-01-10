using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Kitchen
{
    public class EndLevelPopUp : MonoBehaviour
    {
        [SerializeField] GameObject upgradesPopUp;
        [SerializeField] private GameObject continueButton;
        [SerializeField] string mainMenuSceneName = "MainMenu";
        [SerializeField] string kitchenScene = "Kitchen";
        [SerializeField] TextMeshProUGUI victoryDefeatText;

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
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Gana Nivel");
            gameObject.SetActive(true);
            if (LevelManager.CurrentLevel == 5) //TODO Burned Variable
            {
                PlayerPrefs.DeleteAll();
                LevelManager.CurrentLevel = 1;
                SceneManager.LoadScene("Credits", LoadSceneMode.Single);
                victoryDefeatText.text = "Victory";
            }
            else
                LevelManager.CurrentLevel++;
        }

        private void HandleLost()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Pierde Nivel");
            gameObject.SetActive(true);
            continueButton.SetActive(false);
            victoryDefeatText.text = "Defeat";
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
