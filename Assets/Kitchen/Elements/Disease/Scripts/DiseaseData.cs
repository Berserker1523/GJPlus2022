using Kitchen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Disease", menuName = "ScriptableObjects/Disease", order = 1)]

public class DiseaseData : ScriptableObject
{
    [HideInInspector] public new string name;
    [SerializeField] public DiseaseName disease;
    [SerializeField] public Sprite sprite;

    public static UnityAction assetsChanged;

    public void Awake() =>
        assetsChanged?.Invoke();
}

   
