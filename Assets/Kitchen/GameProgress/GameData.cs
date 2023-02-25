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
        public bool tutorialCompleted;

        public GameData(StarsData starsData)
        {
            currentLevel = LevelManager.CurrentLevel;
            stars = starsData.stars;
            attendedClients = GlobalCounter.attendedClients;
            tutorialCompleted = true;
        }

        public GameData()
        {
            currentLevel = LevelManager.CurrentLevel;
            stars= new bool[5, 3];
            attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        }

        public GameData(bool tutorial)
        {
            currentLevel = LevelManager.CurrentLevel;
            tutorialCompleted = tutorial;
        }
    }
    
}

