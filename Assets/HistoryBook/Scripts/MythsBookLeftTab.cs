using HistoryBook;
using Kitchen;
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
    [SerializeField] public int[] _goalsInt;
    [SerializeField] public IngredientName _refIngredient;

    public static UnityAction<MythsBookLeftTab> currentTabSwitchedEvent;

    GameData _gameData;

    public void SetBookEntry(GameData gameData, BookEntry entry)
    {
        _tabName.StringReference = entry.name;
       _bookEntryType = entry.bookEntryType;
        _title= entry.name;
        _description= SetMythText(entry.texts);
        _goal= GetCurrentGoal(entry.goals);
        _sprite= entry.sprite;
        _gameData = gameData;
    }

    public void SetBookEntry(GameData gameData, IndigenousCommunity entry)
    {
        _goalsInt = entry.goalsInt;      
        SetBookEntry(gameData, (BookEntry)entry);
    }

    public void SetBookEntry(GameData gameData, Ingredient entry)
    {
        _goalsInt = entry.goalsInt;
        _refIngredient = entry.refIngredient;
        SetBookEntry(gameData,  (BookEntry)entry);
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
        if (_gameData == null)
            _gameData = new GameData();

        string mythText = "";
        for(int i=0; i<texts.Length; i++)
        {    // TODO check the current Goal
            bool entryUnlocked=false;

            switch (_bookEntryType)
            {
                case ENUM_bookTabs.Myths:
                    mythText = CheckMythsStars(i, texts);
                    break;
                case ENUM_bookTabs.Ingredients:
                    entryUnlocked = CheckAttendedClients(i);
                    break;
                case ENUM_bookTabs.Indigenous:
                    entryUnlocked = CheckStreaks(i);
                    break;
                case ENUM_bookTabs.Places:
                    entryUnlocked = CheckLevelCompletions(i);
                    break;
            }
             if(entryUnlocked)
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

    string CheckMythsStars(int pos, LocalizedString[] texts)
    {
        int currentStars=0;
        foreach (bool star in GetRow(_gameData.stars, pos))      
              if(star)
                currentStars++;

        string mythText = "";
       for (int i=0; i<currentStars;i++)
            mythText += GetStringFromLocalizedString(texts[i]);
       return mythText;
    }

    bool CheckLevelCompletions(int pos)
    {
        if (_gameData.stars[pos,0])
            return true;
        else
            return false;
    }

    bool CheckStreaks(int pos)
    {
        if (_gameData.streaksAmount >= _goalsInt[pos])
            return true;
        else
            return false;
    }
    
    bool CheckAttendedClients(int pos)
    {
        if (_gameData.attendedClients[(int)_refIngredient] >= _goalsInt[pos])
            return true;
        else
            return false;
    }

    public static T[] GetRow<T>(T[,] array2D, int rowIndex)
    {
        int rowLength = array2D.GetLength(1);
        T[] row = new T[rowLength];

        for (int i = 0; i < rowLength; i++)
        {
            row[i] = array2D[rowIndex, i];
        }
        return row;
    }
}
