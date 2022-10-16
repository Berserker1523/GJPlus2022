using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Events;

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

        private void Awake() =>
            EventManager.AddListener<int>(SpawnPointEvent.Released, HandleSpawnPointReleased);

        private void OnDestroy() =>
            EventManager.RemoveListener<int>(SpawnPointEvent.Released, HandleSpawnPointReleased);

        private void Start() =>
            TrySpawnClient();

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= nextSpawnTime)
            {
                TrySpawnClient();
                timer = 0;
            }
        }

        private void TrySpawnClient()
        {
            if (clientsSpawned >= spawnData.clientNumber)
                return;
            SpawnPoint spawnPoint = GetRandomSpawnPoint(out int ID);
            if (spawnPoint == null)
                return;
            ClientView client = Instantiate(clientPrefab, spawnPoint.transform);
            client.Initialize(ID, spawnData.levelRecipes[Random.Range(0, spawnData.levelRecipes.Count)].ingredients);
            spawnPoint.isOpen = false;
            SetRandomNextSpawnTime();
            clientsSpawned += 1;
        }

        private SpawnPoint GetRandomSpawnPoint(out int ID)
        {
            List<SpawnPoint> availableSpawnPoints = spawnPoints.Where(s => s.isOpen).ToList();
            availableSpawnPoints = availableSpawnPoints.OrderBy(s => System.Guid.NewGuid()).ToList();
            if (availableSpawnPoints.Count > 0)
            {
                ID = spawnPoints.IndexOf(availableSpawnPoints[0]);
                return availableSpawnPoints[0];
            }
            ID = -1;
            return null;
        }

        private void SetRandomNextSpawnTime() =>
            nextSpawnTime = Random.Range(spawnData.minSpawnSeconds, spawnData.maxSpawnSeconds);

        private void HandleSpawnPointReleased(int ID) =>
            spawnPoints[ID].isOpen = true; 
    }
}
