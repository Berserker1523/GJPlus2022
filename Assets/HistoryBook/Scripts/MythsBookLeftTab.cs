using HistoryBook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class MythsBookLeftTab : MythsBookTab
{
    [SerializeField] LocalizedString _title;
    [SerializeField] LocalizedString _description;
    [SerializeField] LocalizedString _goal;
    [SerializeField] Sprite _sprite;

    public static UnityAction<MythsBookLeftTab> currentTabSwitchedEvent;

    public void SetBookEntry(ENUM_bookTabs bookEntryType, LocalizedString title, LocalizedString description, LocalizedString goal, Sprite sprite)
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
