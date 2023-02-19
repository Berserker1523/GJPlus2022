using Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class GoalsHUD : MonoBehaviour
    {
        private LevelInstantiator levelInstantiator;
        [SerializeField] private TextMeshProUGUI goalText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private Image[] stars = new Image[3];
        [SerializeField] private int currentTime;
        [SerializeField] private int currentGoal;
        [SerializeField] private bool hurryDispatched;

        [Header("StreakSystem")]
        [SerializeField] private bool[] streakStar = new bool[3];
        [SerializeField] private int streakProgress = 0;
        [SerializeField] private int streakCheckpoint = 0;
        [SerializeField] Image streakBar;

        private FMOD.Studio.EventInstance comboSound;

        private void Awake()
        {
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            comboSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Combo");
        }

        private void Start()
        {
            EventManager.AddListener(ClientEvent.Served, HandleClientServed);
            EventManager.AddListener(TrashEvent.Throw, HandleResetStreak);
            currentGoal = levelInstantiator.LevelData.goal;
            SetGoal();

            currentTime = levelInstantiator.LevelData.time;
            StartCoroutine(LevelTimer());

            for (int i = 0; i < stars.Length; i++)
            {
                if (!levelInstantiator.LevelData.stars[i])
                    stars[i].color = Color.black;
            }
        }

        private void SetGoal() =>
            goalText.text = currentGoal.ToString();

        private IEnumerator LevelTimer()
        {
            speedText.text = currentTime.ToString();

            WaitForSeconds wait = new WaitForSeconds(1f);
            while (currentTime > 0)
            {
                currentTime--;
                speedText.text = string.Format("{0:0}:{1:00}", (currentTime / 60) % 60, currentTime % 60);


                if (currentTime < 10)
                    speedText.color = Color.red;
                else if (currentTime < 30 && !hurryDispatched)
                {
                    speedText.color = Color.yellow;
                    EventManager.Dispatch(LevelEvents.Hurry);
                    hurryDispatched = true;
                }

                yield return wait;

            }
            yield return null;
        }

        private void HandleClientServed()
        {
            CheckStreak();


            currentGoal--;
            if (currentGoal < 0)
                return;
            SetGoal();

            //Goal Star
            if (currentGoal <= 0)
            {
                stars[0].color = Color.white;
                EventManager.Dispatch(LevelEvents.Goal);

                //Time Star
                if (currentTime > 0)
                {
                    stars[1].color = Color.white;
                    EventManager.Dispatch(LevelEvents.Speed);
                }
            }

        }

        private void CheckStreak()
        {
            streakProgress++;
            SetBarProgress();
            if (streakProgress % 3 == 0)
            {
                streakCheckpoint = streakProgress;
                streakStar[(streakCheckpoint / 3) - 1] = true;
                EventManager.Dispatch(LevelEvents.StreakCheckpoint); //TODO Sound
                comboSound.setParameterByName("Combo counter", (streakCheckpoint / 3) - 1);
                comboSound.start();
            }
            if (streakStar[2])
            {
                stars[2].color = Color.white;
                EventManager.Dispatch(LevelEvents.Streak);
            }
        }

        private void SetBarProgress()
        {
            streakBar.rectTransform.sizeDelta = new Vector2(26f * streakProgress, 33f);
        }

        private void HandleResetStreak()
        {
            streakProgress = streakCheckpoint;
            SetBarProgress();
        }
    }
}
