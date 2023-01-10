using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ClickHandlerBase : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] public SpriteRenderer spriteRenderer;

        public abstract void OnPointerClick(PointerEventData eventData);

        public void SetImageColor(Color color) =>
            spriteRenderer.color = color;
    }
}
