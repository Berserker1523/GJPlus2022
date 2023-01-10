using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] int clientsCost = 100;
        [SerializeField] int medicineCost = 300;
        [SerializeField] int kitchenCost = 500;
        [SerializeField] int mortarCost = 400;
        [SerializeField] int painRelieverCost = 600;

        [SerializeField] float maxMedicines = 3;
        [SerializeField] float maxClients = 2;
        [SerializeField] float maxOthers = 1;

        public void Start()
        {
            FillAllBarsInStart();
        }

        public void FillAllBarsInStart()
        {
            clientCapacityBar.fillAmount = (UpgradesManager.ClientsUpgrades / maxClients);
            MedicineJarsBar.fillAmount = (UpgradesManager.MedicineUpgrades / maxMedicines);
            KitchensBar.fillAmount = (UpgradesManager.KitchenUpgrades / maxOthers);
            MortarBar.fillAmount = (UpgradesManager.MortarsUpgrades / maxOthers);
            PainRelieverBar.fillAmount = (UpgradesManager.PainkillerUpgrades / maxOthers);
            deactivatedButton.interactable = false;
        }

        public void UpgradeClients()
        {
            if (MoneyManager.Money - clientsCost < 0 || UpgradesManager.ClientsUpgrades == maxClients)
                return;

            UpgradesManager.ClientsUpgrades++;
            clientCapacityBar.fillAmount = (UpgradesManager.ClientsUpgrades / maxClients);
            MoneyManager.Money -= clientsCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradeMedicines()
        {
            if (MoneyManager.Money - medicineCost < 0 || UpgradesManager.MedicineUpgrades == maxMedicines)
                return;

            UpgradesManager.MedicineUpgrades++;
            MedicineJarsBar.fillAmount = (UpgradesManager.MedicineUpgrades / maxMedicines);
            MoneyManager.Money -= medicineCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradeKitchen()
        {
            if (MoneyManager.Money - kitchenCost < 0 || UpgradesManager.KitchenUpgrades == maxOthers)
                return;

            UpgradesManager.KitchenUpgrades++;
            KitchensBar.fillAmount = (UpgradesManager.KitchenUpgrades / maxOthers);
            MoneyManager.Money -= kitchenCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradeMortar()
        {
            if (MoneyManager.Money - mortarCost < 0 || UpgradesManager.MortarsUpgrades == maxOthers)
                return;

            UpgradesManager.MortarsUpgrades++;
            MortarBar.fillAmount = (UpgradesManager.MortarsUpgrades / maxOthers);
            MoneyManager.Money -= mortarCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }
        public void UpgradePainReliever()
        {
            if (MoneyManager.Money - painRelieverCost < 0 || UpgradesManager.PainkillerUpgrades == maxOthers)
                return;

            UpgradesManager.PainkillerUpgrades++;
            PainRelieverBar.fillAmount = (UpgradesManager.PainkillerUpgrades / maxOthers);
            MoneyManager.Money -= painRelieverCost;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Jugabilidad/Upgrade");
        }

        public void ÇloseUpgrades()
        {
            gameObject.SetActive(false);
        }
    }
}
