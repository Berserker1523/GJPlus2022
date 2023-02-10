using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(Collider2D))]
    public class TrashController : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData pointerEventData)
        {
            if (!pointerEventData.pointerDrag.TryGetComponent(out IReleaseable releaseable))
                return;

            releaseable.Release();
            EventManager.Dispatch(TrashEvent.Throw);
        }
    }
}
