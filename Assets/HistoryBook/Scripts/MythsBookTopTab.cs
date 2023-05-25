using HistoryBook;
using UnityEngine.Events;

namespace HistoryBook
{
    public class MythsBookTopTab : MythsBookTab
    {
        static UnityAction<MythsBookTopTab> currentTabSwitchedEvent;

        protected override void Start()
        {
            base.Start();
            currentTabSwitchedEvent += CheckCurrentTab;
        }

        protected override void SetAsCurrentTab()
        {
            currentTabSwitchedEvent?.Invoke(this);
        }
    }
}