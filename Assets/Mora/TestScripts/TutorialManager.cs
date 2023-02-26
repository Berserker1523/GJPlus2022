using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Kitchen;
using Events;
using System;
public class TutorialManager : MonoBehaviour
{
    private Volume vignetteVolume;
    private Vignette vignetteInstance;

    private enum TutorialActors
    {
        Pequi=0, Water=1, Mortar=2, Shaker=3, Stove =4, Client=5
    }

    //Assing Pequi Water an client from inspector
    [SerializeField] private Transform[] tutorialElements = new Transform[5];

    //Post procees Intensity
    private float activeIntensity =1f, initialIntensity;

    //timer setter
    [SerializeField] private float deactivaterTimer, initialDeactivaterTimer;
    [SerializeField, Range(0,5)] private float[] timers = new float[23];

    private float standardWaitStep = 0.1f;
    WaitForSeconds coroutineWaitStep;

    [ContextMenuItem("MoveObject", "Move")]
    [SerializeField] Transform currentPos;

    Vector2 targetResolution = new Vector2(1920f, 1080f); 

    /*
    IEnumerator[] vignetteCoroutines;
    List<EventHandlerDelegate> methodsArray  = new List<EventHandlerDelegate>();*/

    private void Awake()
    {
        EventManager.AddListener(IngredientState.Cooked, UnusefulMethod);
        EventManager.AddListener(PotionEvent.AddIngredient, UnusefulMethod);
        EventManager.AddListener(PotionEvent.AddWater, UnusefulMethod);
        EventManager.AddListener(PotionEvent.Poof, UnusefulMethod);
        EventManager.AddListener(ClientEvent.Served, UnusefulMethod);

      /*  vignetteCoroutines = new IEnumerator[]
        {
            FirstVignetteCoroutine(), SecondVignetteCoroutine() , ThirdVignetteCoroutine()
        };*/
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(IngredientState.Cooked, UnusefulMethod);
        EventManager.RemoveListener(PotionEvent.AddIngredient, UnusefulMethod);
        EventManager.RemoveListener(PotionEvent.AddWater, UnusefulMethod);
        EventManager.RemoveListener(PotionEvent.Poof, UnusefulMethod);
    }

    private void Start()
    {
        //Try automating Handlers
       /* for (int i = 0; i < vignetteCoroutines.Length-1; i++)
        {
            string methodName = "Method" + i.ToString();

            EventHandlerDelegate methodDelegate = () => StartCoroutine(vignetteCoroutines[i]);
            Debug.Log(vignetteCoroutines[i]);
            methodsArray.Add(methodDelegate);
            EventManager.AddListener((TutorialEvent)i, methodsArray[i]);
            Debug.Log(methodsArray[i].Method);
            Debug.Log((TutorialEvent)i);

        }*/

        vignetteVolume = this.gameObject.GetComponent<Volume>();
        vignetteVolume.profile.TryGet<Vignette>(out vignetteInstance);

        vignetteInstance.intensity.value = 0f;

        coroutineWaitStep = new WaitForSeconds(standardWaitStep);

        LevelManager.CurrentLevel = 0;

        tutorialElements[(int)TutorialActors.Mortar] = GameObject.Find("Mortar(Clone)").GetComponent<Transform>();
        tutorialElements[(int)TutorialActors.Shaker] = GameObject.Find("Potion(Clone)").GetComponent<Transform>();
        tutorialElements[(int)TutorialActors.Stove] = GameObject.Find("Stove(Clone)").GetComponent<Transform>();

        foreach(var tutorialActor in tutorialElements)
            SwitchObjectCollider(tutorialActor, false);

        Addlisteners();   
    }

    void Addlisteners()
    {
        EventHandlerDelegate methodDelegate1 = () => StartCoroutine(FirstVignetteCoroutine());
        EventHandlerDelegate methodDelegate2 = () => StartCoroutine(SecondVignetteCoroutine());
        EventHandlerDelegate methodDelegate3 = () => StartCoroutine(ThirdVignetteCoroutine());
        EventHandlerDelegate methodDelegate4 = () => StartCoroutine(FourthVignetteCoroutine());
        EventHandlerDelegate methodDelegate5 = () => StartCoroutine(FifthVignetteCoroutine());
        EventHandlerDelegate methodDelegate6 = () => StartCoroutine(SixthVignetteCoroutine());
        EventHandlerDelegate methodDelegate7 = () => StartCoroutine(SeventhVignetteCoroutine());
        EventHandlerDelegate methodDelegate8 = () => StartCoroutine(EightVignetteCoroutine());
        EventHandlerDelegate methodDelegate9 = () => StartCoroutine(NinethVignetteCoroutine());
        EventHandlerDelegate methodDelegate10 = () => StartCoroutine(TenthVignetteCoroutine());



       EventManager.AddListener(TutorialEvent.FirstVignette, methodDelegate1);
       EventManager.AddListener(TutorialEvent.SecondVignette, methodDelegate2);
        EventManager.AddListener(TutorialEvent.ThirdVignette, methodDelegate3);
        EventManager.AddListener(TutorialEvent.FourthVignette, methodDelegate4);
        EventManager.AddListener(TutorialEvent.FifthVignette, methodDelegate5);
        /*EventManager.AddListener(TutorialEvent.SixthVignette, methodDelegate6);
        EventManager.AddListener(TutorialEvent.SeventhVignette, methodDelegate7);
        EventManager.AddListener(TutorialEvent.EightVignette, methodDelegate8);
        EventManager.AddListener(TutorialEvent.NinethVignette, methodDelegate9);
        EventManager.AddListener(TutorialEvent.TenthVignette, methodDelegate10);*/
    }

    void SwitchObjectCollider(Transform collider, bool enabled) => collider.GetComponent<BoxCollider2D>().enabled = enabled;

    void UnusefulMethod()
    {
        //TODO Replace Unuseful method to the correspondant listener Method
        Debug.Log(" Replace Unuseful method to the correspondant listener Method");
    }
   
    public IEnumerator MoveCourutine(Transform targetPos)
    {
        Vector3 CurrentVignettePosition;
        while (currentPos.position != targetPos.position)
        {
            currentPos.position = Vector3.MoveTowards(currentPos.position, targetPos.position, standardWaitStep);
            CurrentVignettePosition = Camera.main.WorldToScreenPoint(currentPos.position);
            CurrentVignettePosition /= targetResolution; 
            vignetteInstance.center.value = CurrentVignettePosition;
            yield return standardWaitStep;
        }
    }

    //Method called from Timeline Signal
    public void EnableVignetteFirstTime() => EventManager.Dispatch(TutorialEvent.FirstVignette);

    private IEnumerator FirstVignetteCoroutine()
    {
        //the vignette appears
        Debug.Log("First Coroutine");
        StartCoroutine(MoveCourutine(tutorialElements[(int)TutorialActors.Pequi]));

        yield return new WaitForSeconds(deactivaterTimer);
        float waitTime = timers[0];

        while(initialIntensity <= activeIntensity) 
        {
            initialIntensity += 0.1f;
            vignetteInstance.intensity.value = initialIntensity;
            waitTime -= standardWaitStep;

            yield return coroutineWaitStep;
        }

        EventManager.Dispatch(TutorialEvent.SecondVignette);
    }

    private IEnumerator SecondVignetteCoroutine()
    {
        //waits to the player to see the ingredient
        Debug.Log("Second Coroutine");
        float waitTime = timers[1];

        while (waitTime >= 0)
        {
            waitTime-= standardWaitStep;
            yield return coroutineWaitStep;
        }

        EventManager.Dispatch(TutorialEvent.ThirdVignette);
    } 

    private IEnumerator ThirdVignetteCoroutine()
    {
        // Move givnette to morter
        Debug.Log("Third Coroutine");

        StartCoroutine(MoveCourutine(tutorialElements[(int)TutorialActors.Mortar]));
        float waitTime = timers[2];

        yield return new WaitForSeconds(waitTime);

        EventManager.Dispatch(TutorialEvent.FourthVignette);

    }
    private IEnumerator FourthVignetteCoroutine()
    {
        // Wait to show the morter
        Debug.Log("Fourth Coroutine");

        float waitTime = timers[3];

        yield return new WaitForSeconds(waitTime);
        EventManager.Dispatch(TutorialEvent.FifthVignette);
    }
     private IEnumerator FifthVignetteCoroutine()
    {
        //Vignette disapears
        Debug.Log("Fifth Coroutine");

        float waitTime = timers[4];

        while (initialIntensity > 0f)
        {
            initialIntensity -= 0.1f;
            vignetteInstance.intensity.value = initialIntensity;
            waitTime -= standardWaitStep;
            yield return coroutineWaitStep;
        }
       
        waitTime = timers[4];
        initialIntensity -= 0.01f;

        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Pequi], true);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], true);

        EventManager.Dispatch(TutorialEvent.SixthVignette);
    }

     private IEnumerator SixthVignetteCoroutine()
    {
        Debug.Log("Sixth Coroutine");
        yield return null;
    }
     private IEnumerator SeventhVignetteCoroutine()
    {
        Debug.Log("here2");
        yield return null;
    }
     private IEnumerator EightVignetteCoroutine()
    {
        Debug.Log("here2");
        yield return null;
    }
     private IEnumerator NinethVignetteCoroutine()
    {
        Debug.Log("here2");
        yield return null;
    }
     private IEnumerator TenthVignetteCoroutine()
    {
        Debug.Log("here2");
        yield return null;
    }
     private IEnumerator EleventhVignetteCoroutine()
    {
        Debug.Log("here2");
        yield return null;
    }

}

