using Events;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Animator animator;
    readonly string animLeftParam = "MoveLeft", animRightParam ="MoveRight";
    // Start is called before the first frame update
    private void Awake()
    {
        EventManager.AddListener(BackgroundEvent.windLeft, AnimToTheLeft);
        EventManager.AddListener(BackgroundEvent.windRight, AnimToTheRight);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(BackgroundEvent.windLeft, AnimToTheLeft);
        EventManager.RemoveListener(BackgroundEvent.windRight, AnimToTheRight);
        
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void AnimToTheRight() => animator.SetTrigger(animRightParam);
    void AnimToTheLeft() => animator.SetTrigger(animLeftParam);
}