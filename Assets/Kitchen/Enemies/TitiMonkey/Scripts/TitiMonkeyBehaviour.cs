using DG.Tweening;
using Events;
using Kitchen;
using System;
using System.Collections;
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

    private Animator animator;
    private readonly string stealAnimationParam = "Steal";
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
        animator = GetComponent <Animator>();
        startPos = transform.position;
        MoveToFruit();
    }

    [ContextMenu("Start movement")]
    void MoveToFruit()
    {
        tween = transform.DOMove(targetPos.parent.position, fallDuration).OnComplete(() =>
        {
            StartCoroutine(Steal());
        });
    }

    IEnumerator Steal()
    {
        if (targetPos.position != transform.position)
        {
            //Miss steal()
            Flee();
            yield break;
        }

        animator.SetTrigger(stealAnimationParam);
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => !IsAnimationPlaying(animator, stealAnimationParam));
        CookingToolController mortar = targetPos.GetComponent<CookingToolController>();
        mortar.Release();
        Flee();
    }

    bool IsAnimationPlaying(Animator anim, string animName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animName) && stateInfo.normalizedTime < 1.0f;
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