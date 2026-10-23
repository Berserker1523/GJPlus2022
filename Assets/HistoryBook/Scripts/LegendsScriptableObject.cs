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
    [SerializeField] public Animal[] animals;
}

public abstract class BookEntry
{
    public abstract ENUM_bookTabs bookEntryType { get; }
    [SerializeField] public LocalizedString name;
    [SerializeField] public LocalizedString title;
    [SerializeField] public Sprite sprite;
    [SerializeField] public LocalizedString[] texts;
    [SerializeField] public abstract LocalizedString goal { get; }
}

[Serializable]
public class Myth : BookEntry
{
    public override ENUM_bookTabs bookEntryType => ENUM_bookTabs.Myths;
    public override LocalizedString goal => new LocalizedString("MythsBookV2", "Goal_Stars");   
    [SerializeField] public IngredientName refIngredient;
}

[Serializable]
public class IndigenousCommunity : BookEntry
{
    public override ENUM_bookTabs bookEntryType => ENUM_bookTabs.Indigenous;
    public override LocalizedString goal => new LocalizedString("MythsBookV2", "Goal_Streaks");
    public int[] goalsInt;
}

[Serializable]
public class Ingredient : BookEntry
{
    public override ENUM_bookTabs bookEntryType => ENUM_bookTabs.Ingredients;
    public override LocalizedString goal => new LocalizedString("MythsBookV2", "Goal_Ingredients");
    [SerializeField] public IngredientName refIngredient;    
    public int[] goalsInt;
    public LocalizedString ingredientName;
}

[Serializable]
public class Place : BookEntry
{
    public override ENUM_bookTabs bookEntryType => ENUM_bookTabs.Places;
    public override LocalizedString goal => new LocalizedString("MythsBookV2", "Goal_Places");
}


[Serializable]
public class Animal : BookEntry
{
    public override ENUM_bookTabs bookEntryType => ENUM_bookTabs.Animals;
    public override LocalizedString goal => new LocalizedString("MythsBookV2", "Goal_Animals");
    public int[] goalsInt;
}