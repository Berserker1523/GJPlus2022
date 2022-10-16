using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        [SerializeField] public Button button;

        protected virtual void Awake() =>
            button.onClick.AddListener(OnClick);

        protected virtual void OnDestroy() =>
            button.onClick.RemoveListener(OnClick);

        protected abstract void OnClick();

        public void SetButtonImageColor(Color color) =>
            button.targetGraphic.color = color;
    }
}
