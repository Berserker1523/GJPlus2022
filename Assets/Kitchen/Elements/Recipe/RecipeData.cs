using Kitchen;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]

public class RecipeData : ScriptableObject
{
    [SerializeField] public new string name;
    [SerializeField] public DiseaseName diseasesItCures;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Ingredient[] ingredients;
    [SerializeField] public List<int> popUp = new List<int>();


    [System.Serializable]
    public struct Ingredient 
    {
        [HideInInspector] public  string name;
        public IngredientData ingredient;
        [HideInInspector] public CookingToolName cookingToolName;
    }
}


