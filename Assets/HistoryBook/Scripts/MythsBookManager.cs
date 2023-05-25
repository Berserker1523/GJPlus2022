using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HistoryBook
{
    public class MythsBookManager : MonoBehaviour
    {
        [SerializeField] public LegendsScriptableObject mythsDatabase;
        [SerializeField] public GameObject leftList;
        [SerializeField] public GameObject leftTabPrefab;
    }

}