using Kitchen;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "MythsDatabase", menuName = "ScriptableObjects/Book/MythsContainer")]
public class LegendsScriptableObject : ScriptableObject
{
    [SerializeField] public Myth[] myths ;
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
    [SerializeField] public Sprite ingredientSprite;
}
