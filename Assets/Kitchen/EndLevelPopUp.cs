using Events;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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

        private MoneyUI moneyUI;
        private LevelInstantiator levelInstantiator;

        //FMOD Parameter
        public PARAMETER_ID starsParameterId;
        public EventInstance starsInstance;

        [EventRef, SerializeField] public string fmodEvent;
        [SerializeField] public string fmodParameterName;

        private void Awake()
        {
            moneyUI = FindObjectOfType<MoneyUI>();
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            EventManager.AddListener(GameStatus.LevelFinished, HandleLevelFinished);
            SetFmodParameter();
        }

        private void OnDestroy() =>
            EventManager.RemoveListener(GameStatus.LevelFinished, HandleLevelFinished);

        private void HandleLevelFinished()
        {
            if (levelInstantiator.LevelData.stars[0])
            {
                HandleWon();
                EventManager.Dispatch(GameStatus.Won);
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

            //TODO set advice text

            positiveButtonText.text = continueString.GetLocalizedString();
            positiveButton.onClick.AddListener(NextLevel);
            moneyText.text = moneyUI.GetCurrentLevelMoney().ToString();
            StartCoroutine(DisplayStars());

            LevelManager.CurrentLevel = 1;
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

        private IEnumerator DisplayStars()
        {
            int starsAmount = 0;
            for (int i = 0; i < 3; i++)
            {
                if (starsData.stars[LevelManager.CurrentLevel, i])
                {
                    stars[i].SetTrigger("TriggerStar");
                    starsAmount++;
                    yield return new WaitForSeconds(1f);
                }
            }
            PlayStarsParameter(starsAmount);
        }

        public void NextLevel()
        {
            if (LevelManager.CurrentLevel == 2) //TODO Burned Variable
            {
                PlayerPrefs.DeleteAll();

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
