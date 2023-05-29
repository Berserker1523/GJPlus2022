using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    [System.Serializable]
    public class GameData
    {
        public int currentLevel;
        [HideInInspector] public bool[,] stars = new bool[5,3];
        [HideInInspector] public bool[] tutorials = new bool[6];

        public int[] attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
        public int streaksAmount;
        [SerializeField]

        public GameData(StarsData starsData)
        {
            currentLevel = LevelManager.CurrentLevel;
            stars = starsData.stars;
            tutorials = starsData.tutorials;
            attendedClients = GlobalCounter.attendedClients;
            streaksAmount = GlobalCounter.streaksAmount;         
        }

        public GameData()
        {
            currentLevel = 0;
            stars= new bool[5, 3];
            tutorials = new bool[6];
            attendedClients = new int[Enum.GetValues((typeof(IngredientName))).Length];
            streaksAmount = 0;
        }

    }
    
}

