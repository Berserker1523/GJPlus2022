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
        }

        public void CallSaveData()
        {
           
            SaveManager.SavePlayerData(this);
        }


        public void SetGoalStar()
        {
            stars[LevelManager.CurrentLevel-1, 0] =true;
           
        }

         public void SetSpeedStar()=>       
            stars[LevelManager.CurrentLevel-1, 1] =true;

         public void SetStreak()=>       
            stars[LevelManager.CurrentLevel-1, 2] =true;

        
    }

}
