using Events;
using UnityEngine;

namespace Kitchen
{
    public static class UpgradesManager
    {
        private const string UpgradeClientsPressKey = "UPGRADECLIENTS";
        public static int clientsUpgrades
        {
            get { return PlayerPrefs.GetInt(UpgradeClientsPressKey, 0); }
            set
            {
                PlayerPrefs.SetInt(UpgradeClientsPressKey, value);
                EventManager.Dispatch(UpgradeEvents.clientsChanged, value);
            }
        }
        private const string UpgradeMedicinePressKey = "UPGRADEMEDICINE";
        public static int medicineUpgrades
        {
            get { return PlayerPrefs.GetInt(UpgradeMedicinePressKey, 0); }
            set
            {
                PlayerPrefs.SetInt(UpgradeMedicinePressKey, value);
                EventManager.Dispatch(UpgradeEvents.medicineChanged, value);
            }
        }
        private const string UpgradeKitchenPressKey = "UPGRADEKITCHEN";
        public static int kitchenUpgrades
        {
            get { return PlayerPrefs.GetInt(UpgradeKitchenPressKey, 0); }
            set
            {
                PlayerPrefs.SetInt(UpgradeKitchenPressKey, value);
                EventManager.Dispatch(UpgradeEvents.KitchenChanged, value);
            }
        }
        private const string UpgradeMortarsPressKey = "UPGRADEMORTARS";
        public static int mortarsUpgradeds
        {
            get { return PlayerPrefs.GetInt(UpgradeMortarsPressKey, 0); }
            set
            {
                PlayerPrefs.SetInt(UpgradeMortarsPressKey, value);
                EventManager.Dispatch(UpgradeEvents.MortarChanged, value);
            }
        }
        private const string UpgradePainkillersPressKey = "UPGRADEPAINKILLER";
        public static int painkillerUpgrades
        {
            get { return PlayerPrefs.GetInt(UpgradePainkillersPressKey, 0); }
            set
            {
                PlayerPrefs.SetInt(UpgradePainkillersPressKey, value);
                EventManager.Dispatch(UpgradeEvents.PainRelievedChanged, value);
            }
        }

    }
}
