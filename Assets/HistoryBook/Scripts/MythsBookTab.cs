using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HistoryBook
{
    public abstract class MythsBookTab : MonoBehaviour
    {
        [SerializeField] Image highlightedImage;
        [SerializeField] protected bool currentTab;
        [SerializeField] Button button;
        [SerializeField] public ENUM_bookTabs _bookEntryType;

        protected virtual void Start()
        {
            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(SetAsCurrentTab);
            highlightedImage.enabled = currentTab;
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(SetAsCurrentTab);          
        }

        protected abstract void SetAsCurrentTab();

        protected void CheckCurrentTab(MythsBookTab tab)
        {
            currentTab = tab == this ? true : false;
            highlightedImage.enabled = currentTab;
        }
    }
}