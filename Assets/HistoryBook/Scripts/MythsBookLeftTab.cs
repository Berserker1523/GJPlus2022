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
    [SerializeField] public LocalizedString _ingredientName;
    private int _index;

    private int _argument0, argument1;

    public static UnityAction<MythsBookLeftTab> currentTabSwitchedEvent;

    GameData _gameData;

    public void SetBookEntry(GameData gameData, BookEntry entry, int index=0)
    {
        _gameData = gameData;
        _index = index;
        _tabName.StringReference = entry.name;
       _bookEntryType = entry.bookEntryType;
        _title= entry.name;     
        _description = SetMythText(entry.texts, entry.goal); 
        _sprite= entry.sprite;
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
        _ingredientName= entry.ingredientName;
        SetBookEntry(gameData,  (BookEntry)entry);
    }
    private void OnEnable()
    {
        _goal.Arguments = new object[] { _argument0, argument1 };
        currentTabSwitchedEvent += CheckCurrentTab;       
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        currentTabSwitchedEvent -= CheckCurrentTab;
    }

    protected override void SetAsCurrentTab()
    {
        if (!currentTab)
        currentTabSwitchedEvent?.Invoke(this);
    }

    protected string SetMythText(LocalizedString[] texts, LocalizedString goal)
    {              
        if(_bookEntryType== ENUM_bookTabs.Myths)
            return CheckMythsStars(_index,texts, goal);
        
        string mythText = "";

        for (int i=0; i<texts.Length; i++)
        {    // TODO check the current Goal
            bool entryUnlocked=false;

            switch (_bookEntryType)
            {                 
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
            else
            {
                GetCurrentGoal(goal, i);
                break;
            }

        }
        return mythText;
    }

    protected string GetStringFromLocalizedString(LocalizedString localizedString)=>   
        LocalizationSettings.StringDatabase.GetLocalizedString(localizedString.TableReference, localizedString.TableEntryReference);  

    protected LocalizedString GetCurrentGoal(LocalizedString goals, int position)
    {
        _goal.SetReference(goals.TableReference, goals.TableEntryReference);
        switch (_bookEntryType)
        {
            case ENUM_bookTabs.Ingredients:
                _goal.Arguments[0] = _ingredientName.GetLocalizedString();
                _goal.Arguments[1] =  _goalsInt[position]- _gameData.attendedClients[(int)_refIngredient];
                break;
            case ENUM_bookTabs.Indigenous:
                _goal.Arguments[0] = _goalsInt[position] - _gameData.streaksAmount;
                break;
            case ENUM_bookTabs.Places:
                _goal.Arguments[0] = GetArgumentTakingInAccountTheTutorialCompletion(position);
                break;
        }       
        return goals;
    }

    string CheckMythsStars(int pos, LocalizedString[] texts, LocalizedString goals)
    {
        int currentStars=0;
        foreach (bool star in GetRow(_gameData.stars, pos))      
              if(star)
                currentStars++;

        string mythText = "";
       for (int i=0; i<currentStars;i++)                
            mythText += GetStringFromLocalizedString(texts[i]);

        int restantStars = 3 - currentStars;
        if(restantStars != 0)
        {
            _goal.SetReference(goals.TableReference, goals.TableEntryReference);
            _goal.Arguments[0] = restantStars;
            _goal.Arguments[1] = GetArgumentTakingInAccountTheTutorialCompletion(pos);
        }

        return mythText;
    }

    string GetArgumentTakingInAccountTheTutorialCompletion(int position)
    {
        if (position == 0)
            return "tutorial";
        else
            return position.ToString();
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
