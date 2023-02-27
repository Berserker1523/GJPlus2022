using Events;
using System;
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

        Coroutine streakCoroutine;

        private FMOD.Studio.EventInstance comboSound;

        private void Awake()
        {
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            comboSound = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cocina/Combo");
            EventManager.AddListener(ClientEvent.Served, HandleClientServed);
            EventManager.AddListener(GameStatus.LevelFinished, HandleLevelFinished);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(ClientEvent.Served, HandleClientServed);
            EventManager.RemoveListener(GameStatus.LevelFinished, HandleLevelFinished);
        }

        private void Start()
        {

            currentGoal = levelInstantiator.LevelData.goal;
            SetGoal();

            streakText.text = "0";
            streakBar.maxValue = levelInstantiator.LevelData.streakWaitTime;
            streakBar.value = 0;
            currentTime = levelInstantiator.LevelData.time;
            StartCoroutine(LevelTimer());
        }

        private void HandleLevelFinished()
        {
            streakBar.value = 0;
            StopAllCoroutines();
        }

        private void SetGoal() =>
            goalText.text = currentGoal.ToString();

        private IEnumerator LevelTimer()
        {
            speedText.text = string.Format("{0:0}:{1:00}", currentTime / 60 % 60, currentTime % 60);

            while (currentTime > 0)
            {
                yield return new WaitForSeconds(1f);
                currentTime--;
                speedText.text = string.Format("{0:0}:{1:00}", currentTime / 60 % 60, currentTime % 60);

                if (currentTime < 15)
                    speedText.color = Color.red;
                else if (currentTime < 40 && !hurryDispatched)
                {
                    speedText.color = Color.yellow;
                    EventManager.Dispatch(LevelEvents.Hurry);
                    hurryDispatched = true;
                }
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

                EventManager.Dispatch(GameStatus.LevelFinished);
            }
        }

        private IEnumerator StreakBar()
        {
            float currentStreakTime = 0f;
            streakBar.value = levelInstantiator.LevelData.streakWaitTime;
            UpdateStreakText();
            while (currentStreakTime < levelInstantiator.LevelData.streakWaitTime)
            {
                yield return new WaitForSeconds(0.1f);
                currentStreakTime += 0.1f;
                streakBar.value -= 0.1f;
            }

            currentStreakProgress = 0;
            UpdateStreakText();
            yield return null;
        }

        private void CheckStreak()
        {
            currentStreakProgress++;
            EventManager.Dispatch(LevelEvents.StreakCheckpoint);

            int comboParamater = 0;
            if (currentStreakProgress >= levelInstantiator.LevelData.streak * 0.75f)
                comboParamater = 3;
            else if (currentStreakProgress >= levelInstantiator.LevelData.streak * 0.5f)
                comboParamater = 2;
            else if (currentStreakProgress >= levelInstantiator.LevelData.streak * 0.25f)
                comboParamater = 1;

            comboSound.setParameterByName("Combo counter", comboParamater);
            comboSound.start();
            
            SetBarProgress();

            if (currentStreakProgress == levelInstantiator.LevelData.streak)
                EventManager.Dispatch(LevelEvents.Streak);
        }

        private void SetBarProgress()
        {
            streakText.text = $"{streakCheckpoint}";
            if (currentStreakProgress > 1)
                StopCoroutine(streakCoroutine);

            streakCoroutine = StartCoroutine(StreakBar());
        }

        private void UpdateStreakText() => streakText.text = currentStreakProgress.ToString();
    }
}
