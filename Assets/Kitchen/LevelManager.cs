using UnityEngine;

namespace Kitchen
{
    public static class LevelManager
    {
        private const string CurrentLevelPlayerPrefsKey = "CURRENT_LEVEL";

        public static int CurrentLevel
        {
            get => PlayerPrefs.GetInt(CurrentLevelPlayerPrefsKey, 1);
            set => PlayerPrefs.SetInt(CurrentLevelPlayerPrefsKey, value);
        }
    }
}
