using HistoryBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class MythsBookLeftTab : MythsBookTab
{
    [SerializeField] public LocalizedString _title;
    [SerializeField] public LocalizedString _description;
    [SerializeField] public LocalizedString _goal;
    [SerializeField] public Sprite _sprite;
    [SerializeField] public LocalizeStringEvent _tabName;

    public static UnityAction<MythsBookLeftTab> currentTabSwitchedEvent;

    public void SetBookEntry(ENUM_bookTabs bookEntryType, LocalizedString title, LocalizedString description, LocalizedString goal, Sprite sprite)
    {
        _tabName.StringReference = title;
       _bookEntryType = bookEntryType;
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        currentTabSwitchedEvent -= CheckCurrentTab;
    }

    protected override void SetAsCurrentTab()
    {
        currentTabSwitchedEvent?.Invoke(this);
    }
}
