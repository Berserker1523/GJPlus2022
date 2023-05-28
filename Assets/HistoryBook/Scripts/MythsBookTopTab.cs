using HistoryBook;
using UnityEngine.Events;

namespace HistoryBook
{
    public class MythsBookTopTab : MythsBookTab
    {
        public static UnityAction<MythsBookTopTab> currentTabSwitchedEvent;

        protected override void Start()
        {
            base.Start();
            currentTabSwitchedEvent += CheckCurrentTab;
        }

        protected override void SetAsCurrentTab()
        {   if(!currentTab)
            currentTabSwitchedEvent?.Invoke(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            currentTabSwitchedEvent-= CheckCurrentTab;
        }
    }
}