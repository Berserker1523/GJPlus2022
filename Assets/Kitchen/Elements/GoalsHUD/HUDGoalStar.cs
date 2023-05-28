using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class HUDGoalStar : MonoBehaviour
    {
        [SerializeField] LevelEvents levelEvent;
        Image starSprite;
        Color enableColor = new Color(253f/255f, 203f/255f, 2f/255f, 1f);
        private void Awake()
        {
            EventManager.AddListener(levelEvent, EnableStar);
            starSprite = GetComponent<Image>();
        }

        private void OnDestroy() => EventManager.RemoveListener(levelEvent, EnableStar);

        private void EnableStar() => starSprite.color = enableColor;
    }
}