using Events;
using TMPro;
using UnityEngine;

namespace Kitchen
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText;

        private void Awake() =>
            EventManager.AddListener<int>(MoneyEvents.ValueChanged, SetMoney);

        private void Start() =>
            SetMoney(MoneyManager.Money);

        private void OnDestroy() =>
            EventManager.RemoveListener<int>(MoneyEvents.ValueChanged, SetMoney);

        private void SetMoney(int money) =>
            moneyText.text = $"${money}";
    }
}
