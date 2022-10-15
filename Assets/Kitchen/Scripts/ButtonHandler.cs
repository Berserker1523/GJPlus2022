using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        [SerializeField] protected Button button;

        protected virtual void Awake() =>
            button.onClick.AddListener(OnClick);

        protected virtual void OnDestroy() =>
            button.onClick.RemoveListener(OnClick);

        protected abstract void OnClick();
    }
}
