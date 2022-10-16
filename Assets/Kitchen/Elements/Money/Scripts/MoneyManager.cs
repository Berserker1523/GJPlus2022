using Events;
using UnityEngine;

namespace Kitchen
{

    public static class MoneyManager
    {
        private const string MoneyPlayerPresKey = "MONEY";
        public static int Money
        {
            get { return PlayerPrefs.GetInt(MoneyPlayerPresKey, 0); }
            set
            {
                PlayerPrefs.SetInt(MoneyPlayerPresKey, value);
                EventManager.Dispatch(MoneyEvents.ValueChanged, value);
            }
        }
    }
}
