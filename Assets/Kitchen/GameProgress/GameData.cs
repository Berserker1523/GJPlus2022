using System;
using System.Linq;
using UnityEngine;

namespace Kitchen
{
    [Serializable]
    public class GameData
    {
        public int currentLevel;
        [HideInInspector] public LevelStars[] stars = NewLevelStarsArray(6);
        [HideInInspector] public bool[] tutorials = new bool[6];

        public int[] attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        public int streaksAmount , frightenedMonkeys; 
        [SerializeField]

        public GameData()
        {
            currentLevel = 0;
            stars = NewLevelStarsArray(6);
            tutorials = new bool[6];
            attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
            streaksAmount = 0;
        }

        public static LevelStars[] NewLevelStarsArray(int lenght)
        {
            LevelStars[] stars = Enumerable.Range(0, lenght)
            .Select(levelIndex => new LevelStars { level = levelIndex })
            .ToArray();

            return stars;
        }
    }

    [Serializable]
    public class LevelStars
    {
        //public (int levelWorld, int levelIndex) level;
        public int level;
        public bool goalStar, timeStar, streakStar;
    }
}