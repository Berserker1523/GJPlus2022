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
        [SerializeField] Button deactivatedButton;

        [SerializeField] float clientsUpgrades;
        [SerializeField] float medicineUpgrades;
        [SerializeField] float kitchenUpgrades;
        [SerializeField] float mortarUpgrades;
        [SerializeField] float painRelieverUpgrades;

        [SerializeField] int clientsCost=100;
        [SerializeField] int medicineCost=300;
        [SerializeField] int kitchenCost=500;
        [SerializeField] int mortarCost=400;
        [SerializeField] int painRelieverCost=600;

        [SerializeField] float maxMedicines = 3;
        [SerializeField] float maxClients = 2;
        [SerializeField] float maxOthers = 1;

        public void Start()
        {
            FillAllBarsInStart();
        }

        public void FillAllBarsInStart()
        {
            clientCapacityBar.fillAmount = (UpgradesManager.clientsUpgrades / maxClients);
            MedicineJarsBar.fillAmount = (UpgradesManager.medicineUpgrades / maxMedicines);
            KitchensBar.fillAmount = (UpgradesManager.kitchenUpgrades / maxOthers);
            MortarBar.fillAmount = (UpgradesManager.mortarsUpgradeds / maxOthers);
            PainRelieverBar.fillAmount = (UpgradesManager.painkillerUpgrades / maxOthers);
            deactivatedButton.interactable = false;
        }

        public void UpgradeClients()
        {
            if (MoneyManager.Money-clientsCost < 0 || UpgradesManager.clientsUpgrades == maxClients)
                return;

            UpgradesManager.clientsUpgrades++;                
            clientCapacityBar.fillAmount = (UpgradesManager.clientsUpgrades / maxClients);
            MoneyManager.Money -= clientsCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradeMedicines()
        {
            if (MoneyManager.Money - medicineCost < 0 || UpgradesManager.medicineUpgrades == maxMedicines)
                return;

            UpgradesManager.medicineUpgrades++;
            MedicineJarsBar.fillAmount = (UpgradesManager.medicineUpgrades / maxMedicines);
            MoneyManager.Money -= medicineCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradeKitchen()
        {
            if (MoneyManager.Money - kitchenCost < 0 || UpgradesManager.kitchenUpgrades == maxOthers)
                return;

            UpgradesManager.kitchenUpgrades++;
            KitchensBar.fillAmount = (UpgradesManager.kitchenUpgrades / maxOthers);
            MoneyManager.Money -= kitchenCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradeMortar()
        {
            if (MoneyManager.Money - mortarCost < 0 || UpgradesManager.mortarsUpgradeds == maxOthers)
                return;

            UpgradesManager.mortarsUpgradeds++;
            MortarBar.fillAmount = (UpgradesManager.mortarsUpgradeds / maxOthers);
            MoneyManager.Money -= mortarCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradePainReliever()
        {
            if (MoneyManager.Money - painRelieverCost < 0 || UpgradesManager.painkillerUpgrades == maxOthers)
                return;

            UpgradesManager.painkillerUpgrades++;
            PainRelieverBar.fillAmount = (UpgradesManager.painkillerUpgrades / maxOthers);
            MoneyManager.Money -= painRelieverCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }

        public void ÇloseUpgrades()
        {
            gameObject.SetActive(false);
        }
    }
}
