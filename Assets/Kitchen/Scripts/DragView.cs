using Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(Collider2D))]
    public class DragView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> OnDragBegan;
        public event Action<PointerEventData> OnDragEnded;

        private new Collider2D collider;
        private Camera mainCamera;

        private Vector3 initialDragPosition;
        private Vector3 currentDragPosition;

        bool isDragging, draggingAllowed = true;
        private void Awake()
        {
            mainCamera = Camera.main;
            collider = GetComponent<Collider2D>();
            EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, ForceEndDrag);
        }

        private void OnDestroy() => EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, ForceEndDrag);

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!draggingAllowed) return;
            //Debug.Log($"OnBeginDrag {eventData.position}", gameObject);
            initialDragPosition = transform.position;
            collider.enabled = false;
            isDragging = true;
            OnDragBegan?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!draggingAllowed)
            {
                if (isDragging)               
                    OnEndDrag(eventData);
                
                eventData.pointerDrag = null;
                return;
            }
            //Debug.Log($"OnDrag {eventData.position}", gameObject);
            currentDragPosition = mainCamera.ScreenToWorldPoint(eventData.position);
            currentDragPosition.z = transform.position.z;
            transform.position = currentDragPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log($"OnEndDrag {eventData.position}", gameObject);
            transform.position = initialDragPosition;
            collider.enabled = true;    
            isDragging= false;
            OnDragEnded?.Invoke(eventData);
        }

        public void ForceAllowDragging() => draggingAllowed = true;
        public void ForceEndDrag(bool inTutorial) => draggingAllowed = !inTutorial;
    }
}