using Events;
using Kitchen;

public class GameManager : SinglentonParent<GameManager>
{
    private GameData gameData;
    private void Start()
    {
        EventManager.AddListener(LevelEvents.Goal, SetGoalStar);
        EventManager.AddListener(LevelEvents.Speed, SetSpeedStar);
        EventManager.AddListener(LevelEvents.Streak, SetStreak);
        EventManager.AddListener(GameStatus.Won, CallSaveData);
        EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial0Completed, MarkTutorialAsComplete);
        EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial1Completed, MarkTutorialAsComplete);
        EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial2Completed, MarkTutorialAsComplete);
        EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial3Completed, MarkTutorialAsComplete);
        EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial4Completed, MarkTutorialAsComplete);
        EventManager.AddListener<int>(GlobalTutorialEvent.Tutorial5Completed, MarkTutorialAsComplete);
        LoadStarsDataFileIfExists();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(LevelEvents.Goal, SetGoalStar);
        EventManager.RemoveListener(LevelEvents.Speed, SetSpeedStar);
        EventManager.RemoveListener(LevelEvents.Streak, SetStreak);
        EventManager.RemoveListener(GameStatus.Won, CallSaveData);
        EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial0Completed, MarkTutorialAsComplete);
        EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial1Completed, MarkTutorialAsComplete);
        EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial2Completed, MarkTutorialAsComplete);
        EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial3Completed, MarkTutorialAsComplete);
        EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial4Completed, MarkTutorialAsComplete);
        EventManager.RemoveListener<int>(GlobalTutorialEvent.Tutorial5Completed, MarkTutorialAsComplete);
    }

    public void CallSaveData() 
    {
        gameData.attendedClients = GlobalCounter.attendedClients;
        gameData.streaksAmount = GlobalCounter.streaksAmount;
        gameData.frightenedMonkeys = GlobalCounter.frightenedMonkeys;
        SaveManager.SavePlayerData(gameData);
    }

    public void SetGoalStar() =>
        gameData.stars[LevelManager.CurrentLevel].goalStar = true;

    public void SetSpeedStar() =>
       gameData.stars[LevelManager.CurrentLevel].timeStar = true;

    public void SetStreak() =>
       gameData.stars[LevelManager.CurrentLevel].streakStar = true;

    public void MarkTutorialAsComplete(int id)
    {
        if (id == gameData.tutorials.Length - 1)
            LevelManager.CurrentLevel = 1;
        if (id < 4)
            gameData.stars = GameData.NewLevelStarsArray(6);

        gameData.tutorials[id] = true;
        CallSaveData();
    }

    void LoadStarsDataFileIfExists() => gameData = SaveManager.LoadStarsData();
}