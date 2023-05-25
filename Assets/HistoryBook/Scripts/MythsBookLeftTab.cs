using HistoryBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

public class MythsBookLeftTab : MythsBookTab
{
    [SerializeField] LocalizeStringEvent _title;
    [SerializeField] LocalizeStringEvent _description;
    [SerializeField] LocalizeStringEvent _goal;
    [SerializeField] Sprite _sprite;

    static UnityAction<MythsBookLeftTab> currentTabSwitchedEvent;

    public void SetBookEntry(ENUM_bookTabs bookEntryType, LocalizeStringEvent title, LocalizeStringEvent description, LocalizeStringEvent goal, Sprite sprite)
    {
       _bookEntryType= bookEntryType;
        _title= title;
        _description= description;
        _goal= goal;
        _sprite= sprite;
    }

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
