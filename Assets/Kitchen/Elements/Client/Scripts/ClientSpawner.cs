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
        [SerializeField] private ClientController clientPrefab;
        [SerializeField] private List<Transform> spawnPoints = new();

        private LevelInstantiator levelInstantiator;

        private int clientsSpawned;
        private float spawnTimer;
        private float nextSpawnTime;
        private int clientsDied;
        private int clientsGood;

        private bool lastClientSpawned;

        private void Awake()
        {
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            EventManager.AddListener(ClientEvent.Died, HandleClientDied);
            EventManager.AddListener(ClientEvent.Served, HandleClientServed);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(ClientEvent.Died, HandleClientDied);
            EventManager.RemoveListener(ClientEvent.Served, HandleClientServed);
        }

        private void Start() =>
            TrySpawnClient();

        private void Update()
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= nextSpawnTime)
            {
                TrySpawnClient();
                spawnTimer = 0;
            }
        }

        private void TrySpawnClient()
        {
            if (clientsSpawned >= levelInstantiator.LevelData.clientNumber)
                return;

            Transform spawnPoint = GetRandomSpawnPoint();
            if (spawnPoint == null)
                return;

            ClientController client = Instantiate(clientPrefab, spawnPoint.transform);

            int recipeNumber = SetRandomRecipe(Random.Range(0, 101));
            client.Initialize(levelInstantiator.LevelData.levelRecipes[recipeNumber], levelInstantiator.LevelData.levelRecipes[recipeNumber].sprite);
            SetRandomNextSpawnTime();
            clientsSpawned += 1;
            Debug.Log(clientsSpawned);
        }

        private int SetRandomRecipe(int randomNum)
        {
            List<RecipePercentage> probabilities = levelInstantiator.LevelData.levelPercentages;
            int currentProbability = 0;
            for (int i = 0; i < levelInstantiator.LevelData.levelRecipes.Count; i++)
            {
                currentProbability += (int)probabilities[i];
                if (randomNum <= currentProbability)
                {
                    Debug.Log("Seted recipe " + i + " With a probability of " + ((int)probabilities[i]) + "\n % random num=" + randomNum + " | Cummulative % = " + currentProbability+"%");
                    return i;
                }
            }
            return 0;
        }

        private Transform GetRandomSpawnPoint()
        {
            List<Transform> availableSpawnPoints = spawnPoints.Where(s => s.childCount == 0).OrderBy(s => System.Guid.NewGuid()).ToList();
            return availableSpawnPoints.Count > 0 ? availableSpawnPoints[0] : null;
        }

        private void SetRandomNextSpawnTime() =>
            nextSpawnTime = Random.Range(levelInstantiator.LevelData.minSpawnSeconds, levelInstantiator.LevelData.maxSpawnSeconds);

        private void HandleClientDied()
        {
            clientsDied++;
            if (clientsDied >= spawnPoints.Count)
                EventManager.Dispatch(GameStatus.Lost);
            //Call Victory if no more clients will arrive
            else if (clientsGood + clientsDied >= levelInstantiator.LevelData.clientNumber)
                EventManager.Dispatch(GameStatus.Won);
        }

        private void HandleClientServed()
        {
            clientsGood++;
            if (clientsGood >= levelInstantiator.LevelData.clientNumber || clientsSpawned >= levelInstantiator.LevelData.clientNumber)
                EventManager.Dispatch(GameStatus.Won);
        }
    }
}
