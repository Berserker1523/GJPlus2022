using UnityEngine;

namespace Kitchen
{
    public static class LevelManager
    {
        private const string CurrentLevelPlayerPrefsKey = "LEVEL_CURRENT";

        public static int CurrentLevel
        {
            get => PlayerPrefs.GetInt(CurrentLevelPlayerPrefsKey, 1);
            set => PlayerPrefs.SetInt(CurrentLevelPlayerPrefsKey, value);
        }
    }
}
