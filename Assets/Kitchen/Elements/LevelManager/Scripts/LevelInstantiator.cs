using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Kitchen
{
    public class LevelInstantiator : MonoBehaviour
    {
        public static LevelData levelDataGlobal;
        [SerializeField] private LevelData levelData;

        [Header("Potions")]
        [SerializeField] private GameObject potionLockPrefab;
        [SerializeField] private GameObject potionPrefab;
        [SerializeField] private List<Transform> potionsPositions;

        [Header("Mortars")]
        [SerializeField] private GameObject mortarPrefab;
        [SerializeField] private List<Transform> mortarsPositions;

        [Header("Stoves")]
        [SerializeField] private GameObject stovePrefab;
        [SerializeField] private List<Transform> stovePositions;

        [Header("Painkillers")]
        [SerializeField] private GameObject painkillerPrefab;
        [SerializeField] private List<Transform> painkillersPositions;

        private void Awake()
        {
            levelDataGlobal = levelData;
            InstantiatePotions(levelData.minNumberOfPotionRecipients + UpgradesManager.medicineUpgrades);
            InstantiateMortars(levelData.minNumberOfMortars + UpgradesManager.mortarsUpgradeds);
            InstantiateStoves(levelData.minNumberOfStoves + UpgradesManager.kitchenUpgrades);
            InstantiatePainKillers(levelData.minNumberOfPainKillers + UpgradesManager.painkillerUpgrades);
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

        private void InstantiateStoves(int quantity)
        {
            stovePositions = stovePositions.OrderBy(transform => int.Parse(transform.gameObject.name)).ToList();

            for (int i = 0; i < stovePositions.Count; i++)
            {
                if (i < quantity)
                    Instantiate(stovePrefab, stovePositions[i]);
            }
        }

        private void InstantiatePainKillers(int quantity)
        {
            painkillersPositions = painkillersPositions.OrderBy(transform => int.Parse(transform.gameObject.name)).ToList();

            for (int i = 0; i < painkillersPositions.Count; i++)
            {
                if (i < quantity)
                    Instantiate(painkillerPrefab, painkillersPositions[i]);
            }
        }
    }
}
