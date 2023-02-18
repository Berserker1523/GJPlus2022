using System;
using System.Collections.Generic;

namespace Kitchen
{
    [System.Serializable]
    public class GameData
    {
        public int currentLevel;
        public bool[,] stars = new bool[5,3];

        public int[] attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        public GameData(StarsData starsData)
        {
            currentLevel = LevelManager.CurrentLevel;
            stars = starsData.stars;
            attendedClients = GlobalCounter.attendedClients;
        }
    }
}

