using Events;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kitchen
{
    public class EndLevelPopUp : MonoBehaviour
    {
        [SerializeField] private Animator[] stars = new Animator[3];
        [SerializeField] private StarsData starsData;
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

        [SerializeField] private LocalizedString adviceString;
        [SerializeField] private LocalizeStringEvent adviceStringEvent;
        [SerializeField] private GameObject mythsUpdatedGO;

        private MoneyUI moneyUI;
        private LevelInstantiator levelInstantiator;

        //FMOD Parameter
        public PARAMETER_ID starsParameterId;
        public EventInstance starsInstance;

        [EventRef, SerializeField] public string fmodEvent;
        [SerializeField] public string fmodParameterName;

        private int currentlevel;

        private void Awake()
        {
            mythsUpdatedGO.SetActive(false);
            moneyUI = FindObjectOfType<MoneyUI>();
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            currentlevel = LevelManager.CurrentLevel;
            EventManager.AddListener(GameStatus.LevelFinished, HandleLevelFinished);
            SetFmodParameter();

        }

        private void OnDestroy() =>
            EventManager.RemoveListener(GameStatus.LevelFinished, HandleLevelFinished);

        private void HandleLevelFinished()
        {
            EventManager.RemoveListener(GameStatus.LevelFinished, HandleLevelFinished);
            DisplayAdvice();
            if (levelInstantiator.LevelData.stars[0])
            {
                gameObject.SetActive(true);
                StartCoroutine(CheckMythsData());
            }
            else
            {
                HandleLost();
                EventManager.Dispatch(GameStatus.Lost);
            }
        }

        private void HandleWon()
        {
            gameObject.SetActive(true);
            titleText.text = levelCompletedString.GetLocalizedString();
            titleDilateText.text = levelCompletedString.GetLocalizedString();
            titleText.color = wonTitleColor;

            positiveButtonText.text = continueString.GetLocalizedString();
            positiveButton.onClick.AddListener(NextLevel);
            moneyText.text = moneyUI.GetCurrentLevelMoney().ToString();
            StartCoroutine(DisplayStars());
        }

        private void HandleLost()
        {
            gameObject.SetActive(true);
            titleText.text = levelFailedString.GetLocalizedString();
            titleDilateText.text = levelFailedString.GetLocalizedString();
            titleText.color = lostTitleColor;

            positiveButtonText.text = retryString.GetLocalizedString();
            positiveButton.onClick.AddListener(TryAgain);
            moneyText.text = moneyUI.GetCurrentLevelMoney().ToString();
        }

        private IEnumerator CheckMythsData()
        {
            GameData gameData = SaveManager.LoadStarsData();

            if (gameData != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (gameData.stars[LevelManager.CurrentLevel, i] == false && levelInstantiator.LevelData.stars[i] == true)
                    {
                        mythsUpdatedGO.SetActive(true);
                        yield return new WaitForSecondsRealtime(5f);
                        mythsUpdatedGO.SetActive(false);
                        break;
                    }
                }
            }

            HandleWon();
            EventManager.Dispatch(GameStatus.Won);
            if (LevelManager.CurrentLevel == 2) //TODO Burned Variable
            {
                PlayerPrefs.DeleteAll();
                LevelManager.CurrentLevel = 1;
            }
            else
                LevelManager.CurrentLevel++;
        }

        private IEnumerator DisplayStars()
        {
            int starsAmount = 0;
            for (int i = 0; i < 3; i++)
            {
                if (levelInstantiator.LevelData.stars[i])
                {
                    stars[i].SetTrigger("TriggerStar");
                    starsAmount++;
                    yield return new WaitForSecondsRealtime(1f);
                }
            }
            PlayStarsParameter(starsAmount);
        }
        private void DisplayAdvice()
        {
            string reference = "";
            if (!levelInstantiator.LevelData.stars[0])
                reference = "Ad_Goal";
            else if (!levelInstantiator.LevelData.stars[1])
                reference = "Ad_Time";
            else if (!levelInstantiator.LevelData.stars[2])
                reference = "Ad_Combo";
            else
                reference = "Ad_Win";

            adviceString.TableEntryReference = reference;
            adviceStringEvent.StringReference = adviceString;

            adviceString.RefreshString();
        }

        public void NextLevel()
        {
            if (currentlevel == 2)
                SceneManager.LoadScene(SceneName.Credits2.ToString(), LoadSceneMode.Single);
            else
                SceneManager.LoadScene($"{SceneName.Kitchen}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        }

        public void TryAgain()
        {
            LevelManager.CurrentLevel = currentlevel;
            SceneManager.LoadScene($"{SceneName.Kitchen}{currentlevel}", LoadSceneMode.Single);
        }

        void SetFmodParameter()
        {
            fmodEvent = SoundsManager.endlevelStarsMusic;
            fmodParameterName = SoundsManager.endLevelStarsParameter;

            starsInstance = RuntimeManager.CreateInstance(fmodEvent);
            EventDescription pitchEventDescription;
            starsInstance.getDescription(out pitchEventDescription);
            PARAMETER_DESCRIPTION pitchParameterDescription;
            pitchEventDescription.getParameterDescriptionByName(fmodParameterName, out pitchParameterDescription);
            starsParameterId = pitchParameterDescription.id;
        }

        void PlayStarsParameter(int starsAmount)
        {
            starsInstance.setParameterByName(fmodParameterName, starsAmount);
            starsInstance.start();
        }
    }
}
