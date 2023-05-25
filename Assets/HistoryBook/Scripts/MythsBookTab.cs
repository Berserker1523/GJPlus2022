using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HistoryBook
{
    public class MythsBookTab : MonoBehaviour
    {
        [SerializeField] Image highlightedImage;
        [SerializeField] private bool currentTab;
        [SerializeField] Button button;
        [SerializeField] ENUM_bookTabs tabType;
        static UnityAction<MythsBookTab> currentTabSwitchedEvent;

        private void Start()
        {
            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(SetAsCurrentTab);
            currentTabSwitchedEvent += CheckCurrentTab;
            highlightedImage.enabled = currentTab;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(SetAsCurrentTab);          
        }


        public void SetAsCurrentTab( )
        {
            currentTabSwitchedEvent?.Invoke(this);
        }

        public void CheckCurrentTab(MythsBookTab tab)
        {
            currentTab = tab == this ? true : false;
            highlightedImage.enabled = currentTab;
        }
    }
}