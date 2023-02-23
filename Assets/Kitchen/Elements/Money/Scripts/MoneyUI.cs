using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private Slider slider;

        private LevelInstantiator levelInstantiator;

        private int initialMoney;
        private int currentLevelMoney;
        private int maxLevelMoney;

        private void Awake()
        {
            EventManager.AddListener<int>(MoneyEvents.ValueChanged, SetMoney);
            initialMoney = MoneyManager.Money;
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
        }

        private void Start()
        {
            maxLevelMoney = levelInstantiator.LevelData.clientNumber * (int)(ClientController.MaxWaitingSeconds / 3);
            SetMoney(initialMoney);
        }

        private void OnDestroy() =>
            EventManager.RemoveListener<int>(MoneyEvents.ValueChanged, SetMoney);

        private void SetMoney(int money)
        {
            currentLevelMoney = money - initialMoney;
            moneyText.text = $"{currentLevelMoney}/{maxLevelMoney}";
            slider.SetValueWithoutNotify((float)currentLevelMoney / maxLevelMoney);
        }    
    }
}
