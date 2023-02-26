using Events;
using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Stars Data", menuName = "ScriptableObjects/GameProgress/StarsData")]
    [System.Serializable]
    public class StarsData : ScriptableObject
    {
        [SerializeField] public bool[,] stars = new bool[5, 3];

        private void OnEnable()
        {
            EventManager.AddListener(LevelEvents.Goal, SetGoalStar);
            EventManager.AddListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.AddListener(LevelEvents.Streak, SetStreak);
            EventManager.AddListener(GameStatus.Won, CallSaveData);
            EventManager.AddListener(GlobalEvent.TutorialCompleted, TutorialCompleted);

            LoadStarsDataFileIfExists();
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(LevelEvents.Goal, SetGoalStar);
            EventManager.RemoveListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.RemoveListener(LevelEvents.Streak, SetStreak);
            EventManager.RemoveListener(GameStatus.Won, CallSaveData);
            EventManager.RemoveListener(GlobalEvent.TutorialCompleted, TutorialCompleted);
        }

        public void CallSaveData() =>
            SaveManager.SavePlayerData(this);

        public void SetGoalStar() =>
            stars[LevelManager.CurrentLevel, 0] = true;

        public void SetSpeedStar() =>
           stars[LevelManager.CurrentLevel, 1] = true;

        public void SetStreak() =>
           stars[LevelManager.CurrentLevel, 2] = true;

        public void TutorialCompleted()
        {
            SetGoalStar();
            SetSpeedStar();
            SetStreak();
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
