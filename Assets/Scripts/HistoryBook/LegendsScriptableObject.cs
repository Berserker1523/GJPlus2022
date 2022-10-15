using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MythsDatabase", menuName = "ScriptableObjects/Book/MythsContainer")]
public class LegendsScriptableObject : ScriptableObject
{
    [SerializeField] public Myths[] myths = new Myths[5];
}

[Serializable]
public class Myths
{
    [SerializeField] public string name;
    [SerializeField] public string description;
}
