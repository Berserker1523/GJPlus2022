using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] float maxUpggrades =4;

    public void Awake()
    {
        FillAllBarsToZero();
    }

    public void FillAllBarsToZero()
    {
        clientCapacityBar.fillAmount = 0;
        MedicineJarsBar.fillAmount = 0;
        KitchensBar.fillAmount = 0;
        MortarBar.fillAmount = 0;
        PainRelieverBar.fillAmount = 0;
    }

    public void UpgradeClients()
    {
        clientsUpgrades++;
        clientCapacityBar.fillAmount = (clientsUpgrades / maxUpggrades);
    }
    public void UpgradeMedicines()
    {
        medicineUpgrades++;
        MedicineJarsBar.fillAmount = (medicineUpgrades / maxUpggrades);
    }
     public void UpgradeKitchen()
    {
        medicineUpgrades++;
        MedicineJarsBar.fillAmount = (kitchenUpgrades / maxUpggrades);
    }
     public void UpgradeMortar()
    {
        medicineUpgrades++;
        MedicineJarsBar.fillAmount = (mortarUpgrades / maxUpggrades);
    }
     public void UpgradePainReliever()
    {
        medicineUpgrades++;
        MedicineJarsBar.fillAmount = (painRelieverUpgrades / maxUpggrades);
    }

    public void ÇloseUpgrades()
    {
        gameObject.SetActive(false);
    }
}
