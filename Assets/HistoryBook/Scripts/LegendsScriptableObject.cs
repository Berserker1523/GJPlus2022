using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MythsDatabase", menuName = "ScriptableObjects/Book/MythsContainer")]
public class LegendsScriptableObject : ScriptableObject
{
    [SerializeField] public Myth[] myths ;
}

[Serializable]
public class Myth
{
    [SerializeField] public string name;
    [SerializeField] public string description;
    [SerializeField] public Sprite ingredientSprite;
}
