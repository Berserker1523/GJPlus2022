using Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 3)]
    public class LevelData : ScriptableObject
    {
        public static UnityAction assetsChanged;

        public int level;
        [Header("Clients")]
        [Range (1,200)] public int clientNumber;
        [Range(1, 200)] public float minSpawnSeconds;
        [Range(1, 200)] public float maxSpawnSeconds;
        [Header("Kitchen Elements")]
        [Range(1, 4)] public int minNumberOfPotionRecipients;
        [Range(1, 4)] public int minNumberOfMortars;
        [Range(1, 4)] public int minNumberOfStoves;
        [Range(1, 4)] public int minNumberOfPainKillers;

        [Header("LevelGoals")]
        [Range(1, 200)] public int time =200;
        [Range(1, 200)] public int goal =5;

        [Header("Recipes")]
        public List<RecipeData> levelRecipes = new List<RecipeData>();
        public List<RecipePercentage> levelPercentages = new List<RecipePercentage>();       
        
        public bool[] stars = new bool[3];

        private void OnEnable()
        {
            GameData gameData = SaveManager.LoadStarsData();

            if(level == gameData.currentLevel)
            {
                for(int i=0; i<stars.Length; i++)
                {
                    stars[i] = gameData.stars[level-1, i];
                   // Debug.Log(gameData.stars[level-1, i]);

                }
            }
            EventManager.AddListener(LevelEvents.Goal, SetGoalStar);
            EventManager.AddListener(LevelEvents.Speed, SetSpeedStar);
            EventManager.AddListener(LevelEvents.Streak, SetStreak);
        }

        public void SetGoalStar()
        {
            if (LevelManager.CurrentLevel == level)
                stars[0] = true;
        }

        public void SetSpeedStar()
        {
            if (LevelManager.CurrentLevel == level)
                stars[1] = true;
        }

        public void SetStreak()
        {
            if (LevelManager.CurrentLevel == level)
                stars[2] = true;
        }
    }

    public enum RecipePercentage
    {  
        _5 =5,
        _10 =10, 
        _15=15,
        _20=20,
        _25=25,
        _30=30,
        _35=35,
        _40 =40,
        _45=45,
        _50=50,
        _55=55,
        _60=60,
        _65=65,
        _70=70,
        _75=75,
        _80=80,
        _85=85,
        _90=90,
        _95=95,
        _100=100,
        _0 = 0
    }
}
