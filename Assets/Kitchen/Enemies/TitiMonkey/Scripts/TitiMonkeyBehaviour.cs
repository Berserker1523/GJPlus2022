using DG.Tweening;
using Kitchen;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitiMonkeyBehaviour : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] Vector3 startPos;
    [SerializeField] Transform targetPos;
    [SerializeField, Range(1f, 10f)] float fallDuration, fleeDuration;

    private Tween tween;
    public void OnPointerDown(PointerEventData eventData)
    {
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
        tween = transform.DOMove(targetPos.position, fallDuration).OnComplete(() =>
        {
            Steal();
            Flee();
        });
    }

    void Steal()
    {
        CookingToolController mortar = targetPos.GetComponent<CookingToolController>();
        mortar.Release();
    }

    void Flee()
    {
        tween.Kill();
        transform.DOMove(startPos, fleeDuration).OnComplete(() =>
        {
           Destroy(gameObject);
        });
    }
}