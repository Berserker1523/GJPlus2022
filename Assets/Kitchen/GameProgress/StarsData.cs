using Events;
using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Stars Data", menuName = "ScriptableObjects/GameProgress/StarsData")]
    [System.Serializable]
    public class StarsData : ScriptableObject
    {
        [HideInInspector] public bool[,] stars = new bool[5, 3];
        [HideInInspector] public bool[] tutorials = new bool[4];
        public bool[] trigerrableTutorials = new bool[1];

        private void OnEnable()
        {
            EventManager.AddListener(LevelEvents.Goal, SetGoalStar);
            EventManager.AddListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.AddListener(LevelEvents.Streak, SetStreak);
            EventManager.AddListener(GameStatus.Won, CallSaveData);
            EventManager.AddListener(GlobalTutorialEvent.Tutorial1Completed, Tutorial1Completed);
            EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial2Completed, MarkTutorialAsComplete);
            EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial3Completed, MarkTutorialAsComplete);
            EventManager.AddListener<int>(GlobalTrigerableTutorialEvent.TrashTutorialTriggered, MarkTriggerableTutorialAsComplete);

            LoadStarsDataFileIfExists();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(LevelEvents.Goal, SetGoalStar);
            EventManager.RemoveListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.RemoveListener(LevelEvents.Streak, SetStreak);
            EventManager.RemoveListener(GameStatus.Won, CallSaveData);
            EventManager.RemoveListener(GlobalTutorialEvent.Tutorial1Completed, Tutorial1Completed);
            EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial2Completed, MarkTutorialAsComplete);
            EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial3Completed, MarkTutorialAsComplete);
            EventManager.RemoveListener<int>(GlobalTrigerableTutorialEvent.TrashTutorialTriggered, MarkTriggerableTutorialAsComplete);
        }

        public void CallSaveData() =>
            SaveManager.SavePlayerData(this);

        public void SetGoalStar() =>
            stars[LevelManager.CurrentLevel, 0] = true;

        public void SetSpeedStar() =>
           stars[LevelManager.CurrentLevel, 1] = true;

        public void SetStreak() =>
           stars[LevelManager.CurrentLevel, 2] = true;

        public void Tutorial1Completed()
        {
            SetGoalStar();
            SetSpeedStar();
            SetStreak();
            MarkTutorialAsComplete(0);
            CallSaveData();
        }

        public void MarkTutorialAsComplete(int id)
        {
            tutorials[id] = true;
            if(id == tutorials.Length-1)
                LevelManager.CurrentLevel = 1;

            CallSaveData();
        }

        public void MarkTriggerableTutorialAsComplete(int id)
        {
            trigerrableTutorials[id] = true;
            CallSaveData();
        }

        void LoadStarsDataFileIfExists()
        {
            GameData gameData = SaveManager.LoadStarsData();

            if (gameData != null)
                stars = gameData.stars;
        }
    }

}
