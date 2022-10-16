using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Kitchen
{
    public class ClientSpawner : MonoBehaviour
    {
        [SerializeField] private ClientSpawnData spawnData;
        [SerializeField] private List<SpawnPoint> spawnPoints = new();
        
        private float timer;
        private float nextSpawnTime;

        private void Update()
        {
            timer += Time.deltaTime;

        }

        private SpawnPoint GetRandomSpawnPoint()
        {
            List<SpawnPoint> availableSpawnPoints = spawnPoints.Where(s => s.isOpen).ToList();
            availableSpawnPoints = availableSpawnPoints.OrderBy(s => Guid.NewGuid()).ToList();
            if (availableSpawnPoints.Count > 0)
                return availableSpawnPoints[0];
            return null;
        }
    }

    [Serializable]
    public class SpawnPoint
    {
        public Transform transform;
        public bool isOpen;
    }
}
