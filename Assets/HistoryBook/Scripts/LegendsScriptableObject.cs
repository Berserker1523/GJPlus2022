using Kitchen;
using System;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "MythsDatabase", menuName = "ScriptableObjects/Book/MythsContainer")]
public class LegendsScriptableObject : ScriptableObject
{
    [SerializeField] public Myth[] myths;
    [SerializeField] public IndigenousCommunity[] indigenousCommunities;
    [SerializeField] public Ingredient[] ingredients;
    [SerializeField] public Place[] places;
}

[Serializable]
public class Myth
{
    [SerializeField] public LocalizedString ingredient;
    [SerializeField] public LocalizedString name;
    [SerializeField] public LocalizedString region;
    [SerializeField] public LocalizedString description;
    [SerializeField] public LocalizedString mythP1;
    [SerializeField] public LocalizedString mythP2;
    [SerializeField] public LocalizedString mythP3;
    [SerializeField] public Sprite mythSprite;
    [SerializeField] public IngredientName refIngredient;
    [SerializeField] public LocalizedString[] goals;
}

[Serializable]
public class IndigenousCommunity
{
    [SerializeField] public LocalizedString name;
    [SerializeField] public Sprite indigenousSprite;
    [SerializeField] public LocalizedString[] texts;
    [SerializeField] public LocalizedString[] goals;
}

[Serializable]
public class Ingredient
{
    [SerializeField] public string name;
    [SerializeField] public IngredientName refIngredient;
    [SerializeField] public Sprite ingredientSprite;
    [SerializeField] public LocalizedString[] texts;
    [SerializeField] public LocalizedString[] goals;
}

[Serializable]
public class Place
{
    [SerializeField] public string name;
    [SerializeField] public Sprite placeSprite;
    [SerializeField] public LocalizedString[] texts;
    [SerializeField] public LocalizedString[] goals;
}
