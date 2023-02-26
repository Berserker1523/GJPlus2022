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
        Pequi = 0, Water = 1, Mortar = 2, Shaker = 3, Stove = 4, Client = 5
    }

    //Assing Pequi Water an client from inspector
    [SerializeField] private Transform[] tutorialElements = new Transform[5];

    //Post procees Intensity
    private float activeIntensity = 1f, initialIntensity;

    //timer setter
    [SerializeField] private float deactivaterTimer, initialDeactivaterTimer;
    private float waitToShowTimer = 1f, waitToMoveTimer = 3f;

    private float standardWaitStep = 0.1f;
    WaitForSeconds coroutineWaitStep;
    WaitForSeconds waitSecondsToShow;
    WaitForSeconds waitSecondsToMove;

    [ContextMenuItem("MoveObject", "Move")]
    [SerializeField] Transform currentPos;

    Vector2 targetResolution = new Vector2(1920f, 1080f);

    private void Awake()
    {
        EventManager.AddListener(IngredientState.Cooked, CallSecondCoroutine);
        EventManager.AddListener(PotionEvent.AddIngredient, UnusefulMethod);
        EventManager.AddListener(PotionEvent.AddWater, UnusefulMethod);
        EventManager.AddListener(PotionEvent.Poof, UnusefulMethod);
        EventManager.AddListener(ClientEvent.Served, UnusefulMethod);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(IngredientState.Cooked, UnusefulMethod);
        EventManager.RemoveListener(PotionEvent.AddIngredient, UnusefulMethod);
        EventManager.RemoveListener(PotionEvent.AddWater, UnusefulMethod);
        EventManager.RemoveListener(PotionEvent.Poof, UnusefulMethod);
        EventManager.RemoveListener(ClientEvent.Served, UnusefulMethod);
    }

    private void Start()
    {
        vignetteVolume = this.gameObject.GetComponent<Volume>();
        vignetteVolume.profile.TryGet<Vignette>(out vignetteInstance);

        vignetteInstance.intensity.value = 0f;

        coroutineWaitStep = new WaitForSeconds(standardWaitStep);
        waitSecondsToShow = new WaitForSeconds(waitToShowTimer);
        waitSecondsToMove = new WaitForSeconds(waitToMoveTimer);

        LevelManager.CurrentLevel = 0;

        tutorialElements[(int)TutorialActors.Mortar] = GameObject.Find("Mortar(Clone)").GetComponent<Transform>();
        tutorialElements[(int)TutorialActors.Shaker] = GameObject.Find("Potion(Clone)").GetComponent<Transform>();
        tutorialElements[(int)TutorialActors.Stove] = GameObject.Find("Stove(Clone)").GetComponent<Transform>();

        foreach (var tutorialActor in tutorialElements)
            SwitchObjectCollider(tutorialActor, false);

    }

    void SwitchObjectCollider(Transform collider, bool enabled) => collider.GetComponent<BoxCollider2D>().enabled = enabled;
    void UnusefulMethod()
    {
        //TODO Replace Unuseful method to the correspondant listener Method
        Debug.Log(" Replace Unuseful method to the correspondant listener Method");
    }

    #region VignetteUsefulCororutines
    public void EnableVignetteFirstTime() => StartCoroutine(FirstVignetteCoroutine());

    public IEnumerator MoveVignetteCoroutine(Transform targetPos)
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

    private IEnumerator DisplayVignette()
    {
        float waitTime = waitToShowTimer;

        while (initialIntensity < activeIntensity)
        {
            initialIntensity += 0.1f;
            vignetteInstance.intensity.value = initialIntensity;
            waitTime -= standardWaitStep;
            yield return coroutineWaitStep;
        }
    }

    private IEnumerator DispelVignette()
    {
        float waitTime = waitToShowTimer;

        while (initialIntensity > 0f)
        {
            initialIntensity -= 0.1f;
            vignetteInstance.intensity.value = initialIntensity;
            waitTime -= standardWaitStep;
            yield return coroutineWaitStep;
        }
    }
    #endregion

    private IEnumerator FirstVignetteCoroutine()
    {
        //The vignette appears
        StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Pequi]));

        yield return new WaitForSeconds(deactivaterTimer);

        yield return StartCoroutine(DisplayVignette());

        //Waits to the player to see the ingredient
        yield return waitSecondsToShow;

        // Move givnette to morter
        StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Mortar]));
        yield return waitSecondsToMove;

        // Wait to show the morter
        yield return waitSecondsToShow;

        //Vignette disapears
        yield return StartCoroutine(DispelVignette());

        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Pequi], true);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], true);
    }

    private void CallSecondCoroutine () => StartCoroutine(SecondVignetteCoroutine());

    private IEnumerator SecondVignetteCoroutine()
    {
        yield return null;
    }
}

