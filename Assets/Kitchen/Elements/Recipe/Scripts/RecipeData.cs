using Kitchen;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]

    public class RecipeData : ScriptableObject
    {
        [HideInInspector] public new string name;
        [SerializeField] public RecipeName recipeName;
        [SerializeField] public DiseaseName diseasesItCures;
        [SerializeField] public Sprite sprite;
        [SerializeField] public Ingredient[] ingredients;
        [SerializeField] public List<int> popUp = new List<int>();

        public static UnityAction assetsChanged;

        public void Awake() =>
            assetsChanged?.Invoke();

        [System.Serializable]
        public struct Ingredient
        {
            [HideInInspector] public string name;
            public IngredientData ingredient;
            [HideInInspector] public CookingToolName cookingToolName;
        }
    }
}

