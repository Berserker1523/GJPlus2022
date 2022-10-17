using Events;
using UnityEngine;

namespace Kitchen
{

    public class Upgrades : MonoBehaviour
    {

        public enum UpgradesList
        {
            Medicine, Clients, Kitchen, Mortar, PainRelieved
        }

        private void Awake()
        {
            EventManager.AddListener<int>(UpgradeEvents.clientsChanged, Upgrade);
            EventManager.AddListener<int>(UpgradeEvents.KitchenChanged, Upgrade);
            EventManager.AddListener<int>(UpgradeEvents.medicineChanged, Upgrade);
            EventManager.AddListener<int>(UpgradeEvents.MortarChanged, Upgrade);
            EventManager.AddListener<int>(UpgradeEvents.PainRelievedChanged, Upgrade);
        }
        
        private void OnDestroy()
        {
            EventManager.RemoveListener<int>(UpgradeEvents.clientsChanged, Upgrade);
            EventManager.RemoveListener<int>(UpgradeEvents.KitchenChanged, Upgrade);
            EventManager.RemoveListener<int>(UpgradeEvents.medicineChanged, Upgrade);
            EventManager.RemoveListener<int>(UpgradeEvents.MortarChanged, Upgrade);
            EventManager.RemoveListener<int>(UpgradeEvents.PainRelievedChanged, Upgrade);
        }

        private void Start()
        {

        }
               

        private void Upgrade(int upgrade)
        {
            
        }
    }

}
