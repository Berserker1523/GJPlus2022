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

        [Header("Mortars")]
        [SerializeField] private GameObject mortarPrefab;
        [SerializeField] private List<Transform> mortarsPositions;

        private void Awake()
        {
            InstantiatePotions(levelData.minNumberOfPotionRecipients);
            InstantiateMortars(levelData.minNumberOfMortars);
        }

        private void InstantiatePotions(int quantity)
        {
            potionsPositions = potionsPositions.OrderBy(transform => int.Parse(transform.gameObject.name)).ToList();

            for (int i = 0; i < potionsPositions.Count; i++)
                Instantiate(i < quantity ? potionPrefab : potionLockPrefab, potionsPositions[i]);
        }

        private void InstantiateMortars(int quantity)
        {
            mortarsPositions = mortarsPositions.OrderBy(transform => int.Parse(transform.gameObject.name)).ToList();

            for (int i = 0; i < mortarsPositions.Count; i++)
            {
                if (i < quantity)
                    Instantiate(mortarPrefab, mortarsPositions[i]);
            }
        }
    }
}
