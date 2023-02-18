using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image contentImage;

        public void SetBackgroundColor(Color color) =>
            backgroundImage.color = color;

        public void SetFillAmount(float amount) =>
            fillImage.fillAmount = amount;

        public void SetFillColor(Color color) =>
            fillImage.color = color;

        public void SetContentSprite(Sprite sprite) =>
            contentImage.sprite = sprite;
    }
}
