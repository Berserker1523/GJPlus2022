using UnityEngine;
using System.Collections.Generic;
using System;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/ClientSpawnData", order = 3)]
    public class ClientSpawnData : ScriptableObject
    {
        public int level;
        public int clientNumber;
        public float minSpawnSeconds;
        public float maxSpawnSeconds;
        public List<IngredientList> levelRecipes;
    }

    [Serializable]
    public class IngredientList
    {
        public List<IngredientName> ingredients;
    }
}
