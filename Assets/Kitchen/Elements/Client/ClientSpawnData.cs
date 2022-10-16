using UnityEngine;
using System.Collections.Generic;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/ClientSpawnData", order = 3)]
    public class ClientSpawnData : ScriptableObject
    {
        public int level;
        public float minSpawnSeconds;
        public float maxSpawnSeconds;
        public List<List<IngredientName>> levelRecipes;
    }
}
