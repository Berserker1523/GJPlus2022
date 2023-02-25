using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(DropView))]
    public class TrashController : MonoBehaviour
    {
        [SerializeField] ParticleSystem trashParticle;

        private DropView dropView;

        private void Awake()
        {
            dropView = GetComponent<DropView>();
            dropView.OnDropped += HandleDropped;
            dropView.IsDraggedObjectInteractableWithMe = IsDraggedObjectInteractableWithMe;
            trashParticle.Stop();
        }

        private void OnDestroy() =>
            dropView.OnDropped -= HandleDropped;

        public void HandleDropped(PointerEventData pointerEventData)
        {
            if (!IsDraggedObjectInteractableWithMe(pointerEventData))
                return;

            IReleaseable releaseable = pointerEventData.pointerDrag.GetComponent<IReleaseable>();
            releaseable.Release();
            EventManager.Dispatch(TrashEvent.Throw);
            trashParticle.Play();
        }

        private bool IsDraggedObjectInteractableWithMe(PointerEventData pointerEventData) =>
            pointerEventData.pointerDrag.TryGetComponent(out IReleaseable _);
    }
}
