using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Events;

namespace Kitchen
{
    public enum GameStatus
    {
        Lost,
        Won
    }

    public class ClientSpawner : MonoBehaviour
    {
        [SerializeField] private LevelData spawnData;
        [SerializeField] private GameObject clientPrefab;
        [SerializeField] private GameObject endScreen;
        [SerializeField] private List<SpawnPoint> spawnPoints = new();

        private int clientsSpawned;
        private float timer;
        private float nextSpawnTime;
        private int clientsDied;
        private int clientsGood;

        private void Awake()
        {
            EventManager.AddListener<int>(SpawnPointEvent.Released, HandleSpawnPointReleased);
            EventManager.AddListener(ClientEvent.Died, HandleClientDied);
            EventManager.AddListener(ClientEvent.Served, HandleClientServed);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<int>(SpawnPointEvent.Released, HandleSpawnPointReleased);
            EventManager.RemoveListener(ClientEvent.Died, HandleClientDied);
            EventManager.RemoveListener(ClientEvent.Served, HandleClientServed);
        }

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
            {
                EventManager.Dispatch(GameStatus.Won);
                return;
            }

            SpawnPoint spawnPoint = GetRandomSpawnPoint(out int ID);
            if (spawnPoint == null)
                return;

            ClientView client = Instantiate(clientPrefab, spawnPoint.transform).GetComponentInChildren<ClientView>();
            int randomRecipeNumber = Random.Range(0, spawnData.levelRecipes.Count);
            client.Initialize(ID, spawnData.levelRecipes[randomRecipeNumber].ingredients, spawnData.levelRecipes[randomRecipeNumber].potionSkin);
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

        private void HandleClientDied()
        {
            clientsDied++;
            if (clientsDied >= 5)
                EventManager.Dispatch(GameStatus.Lost);
        }

        private void HandleClientServed()
        {
            clientsGood++;
            if (clientsGood >= spawnData.clientNumber)
                EventManager.Dispatch(GameStatus.Won);
        }
    }
}
