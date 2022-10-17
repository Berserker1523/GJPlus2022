using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Events;

namespace Kitchen
{
    public class UpgradesPopUp : MonoBehaviour
    {
        [SerializeField] Image clientCapacityBar;
        [SerializeField] Image MedicineJarsBar;
        [SerializeField] Image KitchensBar;
        [SerializeField] Image MortarBar;
        [SerializeField] Image PainRelieverBar;

        [SerializeField] float clientsUpgrades;
        [SerializeField] float medicineUpgrades;
        [SerializeField] float kitchenUpgrades;
        [SerializeField] float mortarUpgrades;
        [SerializeField] float painRelieverUpgrades;

        [SerializeField] int clientsCost=60;
        [SerializeField] int medicineCost=300;
        [SerializeField] int kitchenCost=500;
        [SerializeField] int mortarCost=400;
        [SerializeField] int painRelieverCost=600;

        [SerializeField] float maxUpggrades = 4;

        public void Start()
        {
            FillAllBarsInStart();
        }

        public void FillAllBarsInStart()
        {
            clientCapacityBar.fillAmount = (UpgradesManager.clientsUpgrades / maxUpggrades);
            MedicineJarsBar.fillAmount = (UpgradesManager.medicineUpgrades / maxUpggrades);
            KitchensBar.fillAmount = (UpgradesManager.kitchenUpgrades / maxUpggrades);
            MortarBar.fillAmount = (UpgradesManager.mortarsUpgradeds / maxUpggrades);
            PainRelieverBar.fillAmount = (UpgradesManager.painkillerUpgrades / maxUpggrades);         
        }

        public void UpgradeClients()
        {
            if (MoneyManager.Money-clientsCost < 0 || UpgradesManager.clientsUpgrades == maxUpggrades)
                return;

            UpgradesManager.clientsUpgrades++;                
            clientCapacityBar.fillAmount = (UpgradesManager.clientsUpgrades / maxUpggrades);
            MoneyManager.Money -= clientsCost;
        }
        public void UpgradeMedicines()
        {
            if (MoneyManager.Money - medicineCost < 0 || UpgradesManager.medicineUpgrades == maxUpggrades)
                return;

            UpgradesManager.medicineUpgrades++;
            MedicineJarsBar.fillAmount = (UpgradesManager.medicineUpgrades / maxUpggrades);
            MoneyManager.Money -= medicineCost;
        }
        public void UpgradeKitchen()
        {
            if (MoneyManager.Money - kitchenCost < 0 || UpgradesManager.kitchenUpgrades == maxUpggrades)
                return;

            UpgradesManager.kitchenUpgrades++;
            KitchensBar.fillAmount = (UpgradesManager.kitchenUpgrades / maxUpggrades);
            MoneyManager.Money -= kitchenCost;
        }
        public void UpgradeMortar()
        {
            if (MoneyManager.Money - mortarCost < 0 || UpgradesManager.mortarsUpgradeds == maxUpggrades)
                return;

            UpgradesManager.mortarsUpgradeds++;
            MortarBar.fillAmount = (UpgradesManager.mortarsUpgradeds / maxUpggrades);
            MoneyManager.Money -= mortarCost;
        }
        public void UpgradePainReliever()
        {
            if (MoneyManager.Money - painRelieverCost < 0 || UpgradesManager.painkillerUpgrades == maxUpggrades)
                return;

            UpgradesManager.painkillerUpgrades++;
            PainRelieverBar.fillAmount = (UpgradesManager.painkillerUpgrades / maxUpggrades);
            MoneyManager.Money -= painRelieverCost;
        }

        public void ÇloseUpgrades()
        {
            gameObject.SetActive(false);
        }
    }
}
