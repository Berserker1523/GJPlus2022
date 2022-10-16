using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Kitchen
{
    public class ClientSpawner : MonoBehaviour
    {
        [SerializeField] private ClientSpawnData spawnData;
        [SerializeField] private ClientView clientPrefab;
        [SerializeField] private List<SpawnPoint> spawnPoints = new();

        private int clientsSpawned;
        private float timer;
        private float nextSpawnTime;

        private void Start()
        {
            SetRandomNextSpawnTime();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= nextSpawnTime)
                TrySpawnClient();
        }

        private void TrySpawnClient()
        {
            if (clientsSpawned >= spawnData.clientNumber)
                return;
            SpawnPoint spawnPoint = GetRandomSpawnPoint();
            if (spawnPoint == null)
                return;
            ClientView client = Instantiate(clientPrefab, spawnPoint.transform);
            client.SetRequiredRecipe(spawnData.levelRecipes[Random.Range(0, spawnData.levelRecipes.Count)].ingredients);
            SetRandomNextSpawnTime();
            clientsSpawned += 1;
        }

        private SpawnPoint GetRandomSpawnPoint()
        {
            List<SpawnPoint> availableSpawnPoints = spawnPoints.Where(s => s.isOpen).ToList();
            availableSpawnPoints = availableSpawnPoints.OrderBy(s => System.Guid.NewGuid()).ToList();
            if (availableSpawnPoints.Count > 0)
                return availableSpawnPoints[0];
            return null;
        }

        private void SetRandomNextSpawnTime() =>
            nextSpawnTime = Random.Range(spawnData.minSpawnSeconds, spawnData.maxSpawnSeconds);
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform transform;
        public bool isOpen;
    }
}
