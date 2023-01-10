using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 3)]
    public class LevelData : ScriptableObject
    {
        public int level;
        public int clientNumber;
        public float minSpawnSeconds;
        public float maxSpawnSeconds;
        public int minNumberOfPotionRecipients;
        public int minNumberOfMortars;
        public int minNumberOfStoves;
        public int minNumberOfPainKillers;
        public List<RecipeData> levelRecipes;
    }
}
