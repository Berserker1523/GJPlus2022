using DG.Tweening;
using UnityEngine;

public class LeafBehaviour : MonoBehaviour
{
    private void Start()
    {
        int orderInlayer = Random.Range(2, 8);
        GetComponent<SpriteRenderer>().sortingOrder = orderInlayer;
    }
    public void MoveHorizontal(float target)
    {
        float time = Random.Range(1f, 1.5f);
        transform.DOMoveX(target, time).SetEase(Ease.Linear).OnComplete( ()=> 
        { 
            Destroy(gameObject);
        });
    }
}