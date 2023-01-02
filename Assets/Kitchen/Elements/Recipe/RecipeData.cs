using Kitchen;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]

public class RecipeData : ScriptableObject
{
    [SerializeField] public new string name;
    [SerializeField] public DiseaseName diseasesItCures;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Ingredient[] ingredients;


    [System.Serializable]
    public struct Ingredient 
    {
        [HideInInspector] public  string name;
        public IngredientData ingredient;
        [HideInInspector] public CookingToolOnlyMortar cookingToolOnlyMortar;
        [HideInInspector] public CookingToolOnlyStove cookingToolOnlyStove;
        [HideInInspector] public CookingToolOnlyNone cookingToolOnlyNone;
        [HideInInspector] public CookingToolBoth cookingToolBoth;
    }

    private void OnValidate()
    {
       /* if(ingredients != null)
        {
            for (int i = 0; i < ingredients.Length; i++)
                ingredients[i].name = "Ingredient " + (i+1);
        }*/
    }
}


