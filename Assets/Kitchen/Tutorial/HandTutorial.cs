using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace Kitchen.Tutorial
{
    [RequireComponent(typeof(Animator))]
    public class HandTutorial : MonoBehaviour
    {
        [SerializeField] Transform[] _points;
        private float duration = 1.5f;
        private float backwardsDurationMultiplier = 0.75f;
        private Ease easeForward = Ease.InOutSine;
        private Ease easeBackwards = Ease.InOutSine;

        Animator _animator;
        const string _animParamIdle = "Idle";
        const string _animParamTap = "Tap";
        const string _animParamDrag = "Drag";
        public void SwitchEnableHand(bool enable) => gameObject.SetActive(enable);

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            MoveBetweenPoints();
        }

        private void OnDisable()
        {
            transform.DOPause();
        }

        private void MoveBetweenPoints()
        {
            if (_points.Length <= 0)
                return;

            Vector3[] path = CalculatePointsCoordinates();
            transform.position = path[0];

            if (path.Length > 1)
                DoCycle(path);
            else
                _animator.SetTrigger(_animParamTap);
        }

        private void DoCycle(Vector3[] path, bool forward = true)
        {
            _animator.SetTrigger(forward ? _animParamDrag : _animParamIdle);
            transform.DOPath(path, forward ? duration : duration * backwardsDurationMultiplier, pathType: PathType.CatmullRom)
                    .SetEase(forward ? easeForward : easeBackwards)
                    .OnComplete(() =>
                    {
                        Array.Reverse(path);
                        DoCycle(path, !forward);
                    });
        }

        private Vector3[] CalculatePointsCoordinates()
        {
            List<Vector3> pointsTemp = new List<Vector3>();
            foreach (var point in _points)
                pointsTemp.Add(Camera.main.WorldToScreenPoint(point.position));
            return pointsTemp.ToArray();
        }

        private void SetNewPoints(Transform[] newPoints) => _points = newPoints;

        public void StartNewSequence(Transform[] newPoints)
        {
            SwitchEnableHand(false);
            SetNewPoints(newPoints);
            MoveBetweenPoints();
            SwitchEnableHand(true);
        }
    }

}