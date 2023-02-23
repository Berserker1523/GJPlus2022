using Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class GoalsHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goalText;
        [SerializeField] private TextMeshProUGUI speedText;

        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] Slider streakBar;

        private LevelInstantiator levelInstantiator;
        private int currentTime;
        private int currentGoal;
        private bool hurryDispatched;

        private int currentStreakProgress;
        private int streakCheckpoint;
        

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

            streakText.text = "0";
            streakBar.value = 0;
            currentTime = levelInstantiator.LevelData.time;
            StartCoroutine(LevelTimer());
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


                if (currentTime < 15)
                    speedText.color = Color.red;
                else if (currentTime < 40 && !hurryDispatched)
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
                EventManager.Dispatch(LevelEvents.Goal);

                //Time Star
                if (currentTime > 0)
                    EventManager.Dispatch(LevelEvents.Speed);
            }
        }

        private void CheckStreak()
        {
            currentStreakProgress++;
           
            if (currentStreakProgress == 3) //TODO burned variable
            {
                streakCheckpoint++;
                currentStreakProgress = 0;
                EventManager.Dispatch(LevelEvents.StreakCheckpoint); //TODO Sound
                comboSound.setParameterByName("Combo counter", streakCheckpoint);
                comboSound.start();
            }

            SetBarProgress();

            if (streakCheckpoint == 2) //TODO burned variable
                EventManager.Dispatch(LevelEvents.Streak);
        }

        private void SetBarProgress()
        {
            streakText.text = $"{streakCheckpoint}";
            streakBar.value = currentStreakProgress == 0 ? 0 : (float)currentStreakProgress / 3; //TODO burned variable
        }

        private void HandleResetStreak()
        {
            currentStreakProgress = 0;
            SetBarProgress();
        }
    }
}
