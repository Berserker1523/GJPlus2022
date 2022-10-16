using System;
using UnityEngine;

namespace Kitchen
{
    public enum SpawnPointEvent
    {
        Released
    }

    [Serializable]
    public class SpawnPoint
    {
        public Transform transform;
        [NonSerialized] public bool isOpen = true;
    }
}
