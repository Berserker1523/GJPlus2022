using UnityEngine;

namespace Kitchen
{
    public static class UpgradesManager
    {
        private const string ClientsUpgradesPlayerPrefsKey = "UPGRADECLIENTS1";
        private const string MedicineUpgradesPlayerPrefsKey = "UPGRADEMEDICINE1";
        private const string KitchenUpgradesPlayerPrefsKey = "UPGRADEKITCHEN1";
        private const string MortarsUpgradesPlayerPrefsKey = "UPGRADEMORTARS1";
        private const string PainkillerUpgradesPlayerPrefsKey = "UPGRADEPAINKILLER1";

        public static int ClientsUpgrades
        {
            get => PlayerPrefs.GetInt(ClientsUpgradesPlayerPrefsKey, 0);
            set => PlayerPrefs.SetInt(ClientsUpgradesPlayerPrefsKey, value);
        }
        
        public static int MedicineUpgrades
        {
            get => PlayerPrefs.GetInt(MedicineUpgradesPlayerPrefsKey, 0);
            set => PlayerPrefs.SetInt(MedicineUpgradesPlayerPrefsKey, value);
        }
        
        public static int KitchenUpgrades
        {
            get => PlayerPrefs.GetInt(KitchenUpgradesPlayerPrefsKey, 0);
            set => PlayerPrefs.SetInt(KitchenUpgradesPlayerPrefsKey, value);
        }
        
        public static int MortarsUpgrades
        {
            get => PlayerPrefs.GetInt(MortarsUpgradesPlayerPrefsKey, 0);
            set => PlayerPrefs.SetInt(MortarsUpgradesPlayerPrefsKey, value);
        }
        
        public static int PainkillerUpgrades
        {
            get => PlayerPrefs.GetInt(PainkillerUpgradesPlayerPrefsKey, 0);
            set => PlayerPrefs.SetInt(PainkillerUpgradesPlayerPrefsKey, value);
        }
    }
}
