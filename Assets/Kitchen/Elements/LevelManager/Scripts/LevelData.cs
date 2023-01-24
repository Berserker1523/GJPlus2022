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
        public List<RecipeData> levelRecipes;
    }
}
