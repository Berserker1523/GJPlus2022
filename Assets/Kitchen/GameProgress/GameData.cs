namespace Kitchen
{
    [System.Serializable]
    public class GameData
    {
        public int currentLevel;
        public bool[,] stars = new bool[5,3];

        public GameData(StarsData starsData)
        {
            currentLevel = LevelManager.CurrentLevel;
            stars = starsData.stars;
        }
    }
}

