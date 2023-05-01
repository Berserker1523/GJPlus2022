using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    [System.Serializable]
    public class GameData
    {
        public int currentLevel;
        public bool[,] stars = new bool[5,3];
        public bool[] tutorials = new bool[6];

        public int[] attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        [SerializeField]

        public GameData(StarsData starsData)
        {
            currentLevel = LevelManager.CurrentLevel;
            stars = starsData.stars;
            tutorials = starsData.tutorials;
            attendedClients = GlobalCounter.attendedClients;         
        }

        public GameData()
        {
            currentLevel = LevelManager.CurrentLevel;
            stars= new bool[5, 3];
            tutorials = new bool[6];
            attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        }

       /* public GameData(bool tutorial)
        {
            currentLevel = LevelManager.CurrentLevel;
            tutorialCompleted = tutorial;
        }   */
    }
    
}

