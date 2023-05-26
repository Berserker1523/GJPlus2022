using HistoryBook;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class MythsBookLeftTab : MythsBookTab
{
    [SerializeField] public LocalizedString _title;
    [SerializeField] public string _description;
    [SerializeField] public LocalizedString _goal;
    [SerializeField] public Sprite _sprite;
    [SerializeField] public LocalizeStringEvent _tabName;

    public static UnityAction<MythsBookLeftTab> currentTabSwitchedEvent;

    public void SetBookEntry(ENUM_bookTabs bookEntryType, LocalizedString title, LocalizedString[] description, LocalizedString[] goal, Sprite sprite)
    {
        _tabName.StringReference = title;
       _bookEntryType = bookEntryType;
        _title= title;
        _description= SetMythText(description);
        _goal= GetCurrentGoal(goal);
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

    protected string SetMythText(LocalizedString[] texts)
    {
        string mythText = "";
        for(int i=0; i<texts.Length; i++)
        {    // TODO check the current Goal
            mythText += GetStringFromLocalizedString(texts[i]);
        }
        return mythText;
    }

    protected string GetStringFromLocalizedString(LocalizedString localizedString)=>   
        LocalizationSettings.StringDatabase.GetLocalizedString(localizedString.TableReference, localizedString.TableEntryReference);  

    protected LocalizedString GetCurrentGoal(LocalizedString[] goals)
    {
        // TODO check the current Goal
        return goals[0];
    }
    
}
