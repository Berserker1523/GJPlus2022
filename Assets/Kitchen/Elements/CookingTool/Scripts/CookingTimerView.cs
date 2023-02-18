using UnityEngine;

namespace Kitchen
{
    public class CookingTimerView : TimerView
    {
        [Header("CookingAssets")]
        [SerializeField] private Color cookingBackgroundColor;
        [SerializeField] private Color cookingFillColor;
        [SerializeField] private Sprite cookingContentSprite;

        [Header("BurningAssets")]
        [SerializeField] private Color burningBackgroundColor;
        [SerializeField] private Color burningFillColor;
        [SerializeField] private Sprite burningContentSprite;

        public void SetCooking()
        {
            SetBackgroundColor(cookingBackgroundColor);
            SetFillColor(cookingFillColor);
            SetContentSprite(cookingContentSprite);
        }

        public void SetBurning()
        {
            SetBackgroundColor(burningBackgroundColor);
            SetFillColor(burningFillColor);
            SetContentSprite(burningContentSprite);
        }
    }
}

