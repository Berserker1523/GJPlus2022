using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kitchen
{
    public class LevelInstantiator : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;

        [Header("Potions")]
        [SerializeField] private GameObject potionLockPrefab;
        [SerializeField] private GameObject potionPrefab;
        [SerializeField] private List<Transform> potionsPositions;

        private void Awake()
        {
            InstantiatePotions(levelData.minNumberOfPotionRecipients);
        }

        private void InstantiatePotions(int numberOfPotions)
        {
            potionsPositions = potionsPositions.OrderBy(potionTransform => int.Parse(potionTransform.gameObject.name)).ToList();

            for (int i = 0; i < potionsPositions.Count; i++)
                Instantiate(i < numberOfPotions ? potionPrefab : potionLockPrefab, potionsPositions[i]);
        }
    }
}
