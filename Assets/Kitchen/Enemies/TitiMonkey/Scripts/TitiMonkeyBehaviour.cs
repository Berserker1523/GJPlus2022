using DG.Tweening;
using Events;
using Kitchen;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitiMonkeyBehaviour : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] Vector3 startPos;
    [SerializeField] public Transform targetPos;
    [SerializeField, Range(1f, 10f)] float fallDuration, fleeDuration;

    private Tween tween;
    private bool inTutorial;
    public bool inMonkeyTutorial;

    private void Awake() => EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, PauseMonkeyBehaviourWhileInTutorial);

    private void PauseMonkeyBehaviourWhileInTutorial(bool _inTutorial) 
    {
        if (_inTutorial)
            tween.Pause();
        else
            tween.Play();

        inTutorial = _inTutorial;
    }

    private void OnDestroy() => EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, PauseMonkeyBehaviourWhileInTutorial);
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inTutorial &&!inMonkeyTutorial) return;
        EventManager.Dispatch(MonkeyEvents.frightened);
        Flee();
    }

    void Start()
    {
        startPos = transform.position;
        MoveToFruit();
    }

    [ContextMenu("Start movement")]
    void MoveToFruit()
    {
        tween = transform.DOMove(targetPos.parent.position, fallDuration).OnComplete(() =>
        {
            Steal();
            Flee();
        });
    }

    void Steal()
    {
        if (targetPos.position != transform.position)
        {
            //Miss steal()
            Flee();
            return;
        }

        CookingToolController mortar = targetPos.GetComponent<CookingToolController>();
        mortar.Release();
    }

    void Flee()
    {
        tween.Kill();
        tween = transform.DOMove(startPos, fleeDuration).OnComplete(() =>
        {
           Destroy(gameObject);
        });
    }
}