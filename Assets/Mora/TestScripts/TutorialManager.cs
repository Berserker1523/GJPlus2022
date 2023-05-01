using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Kitchen;
using Events;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class TutorialManager : MonoBehaviour
{
    private Volume vignetteVolume;
    private Vignette vignetteInstance;

    private enum TutorialActors
    {
        Pequi = 0, Water = 1, Mortar = 2, Shaker = 3, Client = 4, PotionResult = 5,
    }
    public PlayableDirector finalTimeline;
    //Assing Pequi Water an client from inspector
    [SerializeField] private Transform[] tutorialElements = new Transform[5];
    [SerializeField] private StarsData starsData;

    //Post procees Intensity
    private float activeIntensity = 1f, initialIntensity;

    private float waitToShowTimer = 1f, waitToMoveTimer = 3f;

    private float standardWaitStep = 0.1f;
    WaitForSeconds coroutineWaitStep;
    WaitForSeconds waitSecondsToShow;
    WaitForSeconds halfSecond = new WaitForSeconds(0.5f);

    [ContextMenuItem("MoveObject", "Move")]
    [SerializeField] Transform currentPos;

    Vector2 targetResolution = new Vector2(1920f, 1080f);

    [SerializeField] private GameObject updatedMythsGO;

    private void Awake()
    {
        EventManager.AddListener(IngredientState.Cooked, CallSecondCoroutine);
        EventManager.AddListener(PotionEvent.AddIngredient, CallThirdCoroutine);
        EventManager.AddListener(PotionEvent.AddWater, callFourthCoroutine);
        EventManager.AddListener(PotionEvent.Poof, callFifhtCoroutine);
        EventManager.AddListener(ClientEvent.Served, sixthVignetteCoroutineStart);

        updatedMythsGO.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener(IngredientState.Cooked, CallSecondCoroutine);
        EventManager.RemoveListener(PotionEvent.AddIngredient, CallThirdCoroutine);
        EventManager.RemoveListener(PotionEvent.AddWater, callFourthCoroutine);
        EventManager.RemoveListener(PotionEvent.Poof, callFifhtCoroutine);
        EventManager.RemoveListener(ClientEvent.Served, sixthVignetteCoroutineStart);
    }

    private void Start()
    {
        vignetteVolume = this.gameObject.GetComponent<Volume>();
        vignetteVolume.profile.TryGet<Vignette>(out vignetteInstance);

        vignetteInstance.intensity.value = 0f;
        waitSecondsToShow = new WaitForSeconds(waitToShowTimer);
        LevelManager.CurrentLevel = 0;

        tutorialElements[(int)TutorialActors.Mortar] = GameObject.Find("Mortar(Clone)").GetComponent<Transform>();
        tutorialElements[(int)TutorialActors.Shaker] = GameObject.Find("Potion(Clone)").GetComponent<Transform>();
        //tutorialElements[(int)TutorialActors.Stove] = GameObject.Find("Stove(Clone)").GetComponent<Transform>();
        tutorialElements[(int)TutorialActors.PotionResult] = GameObject.Find("Result").GetComponent<Transform>();

        foreach (var tutorialActor in tutorialElements)
            SwitchObjectCollider(tutorialActor, false);

        targetResolution = new Vector2(Screen.width, Screen.height);
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
        StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Pequi])); ;

        yield return StartCoroutine(DisplayVignette());

        //Waits to the player to see the ingredient
        yield return waitSecondsToShow;

        // Move givnette to morter
        yield return StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Mortar]));

        // Wait to show the morter
        yield return waitSecondsToShow;

        //Vignette disapears
        yield return StartCoroutine(DispelVignette());

        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Pequi], true);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], true);
    }

    private void CallSecondCoroutine() => StartCoroutine(SecondVignetteCoroutine());

    private IEnumerator SecondVignetteCoroutine()
    {
        //deactivate colliders
        yield return new WaitForSeconds(0.1f);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Pequi], false);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], false);
        // vignette moves to mortar
        yield return StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Mortar]));

        //vignette appears on mortar
        yield return StartCoroutine(DisplayVignette());

        //vignette waits
        yield return waitSecondsToShow;

        //move vignette to shaker
        yield return StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Shaker]));

        //vignette stops in shake
        yield return waitSecondsToShow;

        //vignette disappears
        yield return StartCoroutine(DispelVignette());

        //activates colliders
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], true);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], true);
    }
    private void CallThirdCoroutine() => StartCoroutine(ThirdVignetteCoroutine());
    private IEnumerator ThirdVignetteCoroutine()
    {
        //turn off the colliders
        yield return new WaitForSeconds(0.1f);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Mortar], false);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], false);

        //vignette moves to water
        yield return StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Water]));

        //vignette shows on water
        yield return StartCoroutine(DisplayVignette());

        //vignette waits
        yield return waitSecondsToShow;

        //vignette moves to shaker
        yield return StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Shaker]));

        //vignette waits
        yield return waitSecondsToShow;

        //vignette disappears
        yield return StartCoroutine(DispelVignette());

        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Water], true);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], true);
    }
    private void callFourthCoroutine() => StartCoroutine(fourthVignetteCoroutine());


    private IEnumerator VignetteGlow(float seconds)
    {
        float tempSeconds = 0;
        while (tempSeconds <= seconds)
        {
            float currentVignetteIntensity = vignetteInstance.intensity.value;
            float targetVignetteIntensity = currentVignetteIntensity == 1 ? 0.8f : 1;
            float tempT = 0f;
            while (vignetteInstance.intensity.value != targetVignetteIntensity)
            {
                tempSeconds += 0.05f;
                tempT += 0.05f;
                vignetteInstance.intensity.value = Mathf.Lerp(currentVignetteIntensity, targetVignetteIntensity, tempT);
                yield return new WaitForSeconds(0.05f);
            }
        }

        vignetteInstance.intensity.value = 1f;
    }

    private IEnumerator fourthVignetteCoroutine()
    {
        //deactivate colliders
        yield return new WaitForSeconds(0.1f);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Water], false);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], false);

        //vignette shows on shaker
        yield return StartCoroutine(DisplayVignette());

        //vignette waits
        yield return waitSecondsToShow;

        yield return StartCoroutine(VignetteGlow(3f));

        yield return waitSecondsToShow;

        //vignette disappears
        yield return StartCoroutine(DispelVignette());

        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], true);
    }
    private void callFifhtCoroutine() => StartCoroutine(fifthVignetteCoroutine());
    private IEnumerator fifthVignetteCoroutine()
    {
        //deactivate colliders
        yield return new WaitForSeconds(0.1f);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.PotionResult], false);

        //vignette shows on potion result
        yield return MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.PotionResult]);
        yield return StartCoroutine(DisplayVignette());

        //vignette waits
        yield return waitSecondsToShow;

        //vignette moves to Client
        yield return StartCoroutine(MoveVignetteCoroutine(tutorialElements[(int)TutorialActors.Client]));

        //vignette waits
        yield return waitSecondsToShow;

        //vignette disappears
        yield return StartCoroutine(DispelVignette());

        SwitchObjectCollider(tutorialElements[(int)TutorialActors.PotionResult], true);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Client], true);
    }

    private void sixthVignetteCoroutineStart() =>
        StartCoroutine(sixthVignetteCoroutine());

    public IEnumerator sixthVignetteCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Shaker], false);
        SwitchObjectCollider(tutorialElements[(int)TutorialActors.Client], false);
        finalTimeline.Play();
        StartCoroutine(endOfTutorial());
    }

    public IEnumerator endOfTutorial()
    {
        yield return new WaitForSeconds(14f);
        EventManager.Dispatch(GlobalTutorialEvent.Tutorial1Completed);
        yield return StartCoroutine(DisplayMythsUpdatedPopUP());
        LoadKitchen1();

    }
    void LoadKitchen1() => SceneManager.LoadScene("Tutorial2");

    IEnumerator DisplayMythsUpdatedPopUP()
    {
        updatedMythsGO.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        updatedMythsGO.SetActive(false);
    }
}

