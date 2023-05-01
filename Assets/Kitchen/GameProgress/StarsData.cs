using Events;
using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Stars Data", menuName = "ScriptableObjects/GameProgress/StarsData")]
    [System.Serializable]
    public class StarsData : ScriptableObject
    {
        [SerializeField] public bool[,] stars = new bool[5, 3];
        [SerializeField] public bool[] tutorials = new bool[6];

        private void OnEnable()
        {
            EventManager.AddListener(LevelEvents.Goal, SetGoalStar);
            EventManager.AddListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.AddListener(LevelEvents.Streak, SetStreak);
            EventManager.AddListener(GameStatus.Won, CallSaveData);
            EventManager.AddListener(GlobalEvent.Tutorial1Completed, Tutorial1Completed);
            EventManager.AddListener<int>(GlobalEvent.Tutorial2Completed, MarkTutorialAsComplete);
            EventManager.AddListener<int>(GlobalEvent.Tutorial3Completed, MarkTutorialAsComplete);
            EventManager.AddListener<int>(GlobalEvent.Tutorial4Completed, MarkTutorialAsComplete);
            EventManager.AddListener<int>(GlobalEvent.Tutorial5Completed, MarkTutorialAsComplete);
            EventManager.AddListener<int>(GlobalEvent.Tutorial6Completed, MarkTutorialAsComplete);

            LoadStarsDataFileIfExists();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(LevelEvents.Goal, SetGoalStar);
            EventManager.RemoveListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.RemoveListener(LevelEvents.Streak, SetStreak);
            EventManager.RemoveListener(GameStatus.Won, CallSaveData);
            EventManager.RemoveListener(GlobalEvent.Tutorial1Completed, Tutorial1Completed);
            EventManager.RemoveListener<int>(GlobalEvent.Tutorial2Completed, MarkTutorialAsComplete);
            EventManager.RemoveListener<int>(GlobalEvent.Tutorial3Completed, MarkTutorialAsComplete);
            EventManager.RemoveListener<int>(GlobalEvent.Tutorial4Completed, MarkTutorialAsComplete);
            EventManager.RemoveListener<int>(GlobalEvent.Tutorial5Completed, MarkTutorialAsComplete);
            EventManager.RemoveListener<int>(GlobalEvent.Tutorial6Completed, MarkTutorialAsComplete);
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
            if(id == tutorials.Length+1)
                LevelManager.CurrentLevel = 1;

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
