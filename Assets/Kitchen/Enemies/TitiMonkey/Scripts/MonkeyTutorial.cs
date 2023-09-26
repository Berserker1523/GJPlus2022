using Events;
using Kitchen.Tutorial;
using System.Collections;
using UnityEngine;

public class MonkeyTutorial : MonoBehaviour
{
    private HandTutorial handTutorial;
    [SerializeField] Animator thinkGlobeImage;
    [SerializeField] Transform handTargetPos;

    private void Awake()
    {
        EventManager.AddListener<Transform>(MonkeyEvents.spawn, CallMonkeyTutorialRoutine);
        EventManager.AddListener(MonkeyEvents.frightened, FinishTutorial);
        handTutorial = FindObjectOfType<HandTutorial>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<Transform>(MonkeyEvents.spawn, CallMonkeyTutorialRoutine);        
        EventManager.RemoveListener(MonkeyEvents.frightened, FinishTutorial);
    }

    private void CallMonkeyTutorialRoutine(Transform monkeyTransform)
    {
        monkeyTransform.GetComponent<TitiMonkeyBehaviour>().inMonkeyTutorial = true;
        StartCoroutine(MonkeyTutorialRoutine());
    }

    private IEnumerator MonkeyTutorialRoutine()
    {
        thinkGlobeImage.SetBool("Display",true);
        yield return new WaitForSeconds(1f);
        EventManager.Dispatch(GlobalTutorialEvent.inTutorial, true);
        handTutorial.StartNewSequence(new Transform[]{ handTargetPos });
    }
    private void FinishTutorial()
    {
        EventManager.Dispatch(GlobalTutorialEvent.inTutorial, false);
        thinkGlobeImage.SetBool("Display", false);
        handTutorial.SwitchEnableHand(false);
        EventManager.RemoveListener<Transform>(MonkeyEvents.spawn, CallMonkeyTutorialRoutine);
    }
}