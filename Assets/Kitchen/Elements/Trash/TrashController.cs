using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(Collider2D))]
    public class TrashController : MonoBehaviour, IDropHandler
    {
        [SerializeField] ParticleSystem trashParticle;

        void Awake()
        {
            trashParticle.Stop();
        }
        public void OnDrop(PointerEventData pointerEventData)
        {
            if (!pointerEventData.pointerDrag.TryGetComponent(out IReleaseable releaseable))
                return;

            releaseable.Release();
            EventManager.Dispatch(TrashEvent.Throw);
            trashParticle.Play();
        }
    }
}
