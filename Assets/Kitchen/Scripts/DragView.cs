﻿using System;
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

        private void Awake()
        {
            mainCamera = Camera.main;
            collider = GetComponent<Collider2D>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log($"OnBeginDrag {eventData.position}", gameObject);
            initialDragPosition = transform.position;
            collider.enabled = false;
            OnDragBegan?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
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
            OnDragEnded?.Invoke(eventData);
        }
    }
}
