using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Kitchen.Tutorial
{
    [RequireComponent(typeof(Animator))]
    public class HandTutorial : MonoBehaviour
    {
        [SerializeField] Transform[] _points;
        [SerializeField, Range(0f, 10f)] float duration =2f;

        Animator _animator;
        const string _animParamTap = "Tap";
        const string _animParamEndDrag = "EndDrag";

        public void SwitchEnableHand(bool enable) => gameObject.SetActive(enable);

        private void Awake()
        {
            _animator= GetComponent<Animator>();          
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
            if (_points.Length<=0)
                return;

            Vector3[] path = CalculatePointsCoordinates();
            transform.position = path[0];
            if (path.Length > 1)
            {
                _animator.SetBool(_animParamTap, false);
                transform.DOPath(path, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Restart).OnStepComplete(() =>
                {
                    _animator.SetTrigger(_animParamEndDrag);
                });
            }
            else
                _animator.SetBool(_animParamTap, true);
        }

        private Vector3[] CalculatePointsCoordinates()
        {
            List<Vector3> pointsTemp = new List<Vector3>();
            foreach (var point in _points)
                pointsTemp.Add (Camera.main.WorldToScreenPoint(point.position));
            return pointsTemp.ToArray();
        }
        
        private void SetNewPoints(Transform[] newPoints) => _points= newPoints;

        public void StartNewSequence(Transform[] newPoints)
        {
            SwitchEnableHand(false);
            SetNewPoints(newPoints);
            MoveBetweenPoints();
            SwitchEnableHand(true);
        }
    }

}