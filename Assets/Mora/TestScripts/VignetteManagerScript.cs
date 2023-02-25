using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Events;
using Kitchen;
using UnityEngine.SceneManagement;

public class VignetteManagerScript : MonoBehaviour
{
    #region variables
    //post process values
    private Volume vignetteVolume;
    private Vignette vignetteInstance;
    //post process coordenates
    public float xCoordenates;
    public float yCoordenates;
    //post procees Intensity
    private float initialIntensity;
    private float ActiveIntensity;
    //timer setter
    public float deactivaterTimer;
    private float initialDeactivaterTimer;
    //timer first animation
    public float FirstTimer;
    private float initialFirstTimer;
    //timer cross animation 1 - 2
    public float secondTimer;
    private float initialSecondTimer;
    //timer cross animation 2 - 3
    public float thirdTimer;
    private float initialThirdTimer;
    //timer cross animation 3 - 4;
    public float fourthTimer;
    private float initialFourthTimer;
    //timer cross animation 4 - 5
    public float fifthTimer;
    private float initialFifthTimer;
    //timer cross animation 5 - 6
    public float sixthTimer;
    private float initialSixthTimer;
    //timer cross animation 6 - 7
    public float seventhTimer;
    private float initialSeventhTimer;
    //timer cross animation 7 - 8
    public float eightTimer;
    private float initialEightTimer;
    //timer cross animation 7 - 9
    public float ninethTimer;
    private float initialNinethTimer;
    //timer cross animation 9 - 10
    public float tenthTimer;
    private float initialTenthTimer;
    //timer cross animation 10 - 11
    public float eleventhTimer;
    private float initialEleventhTimer;
    //timer cross animation 11 - 12
    public float twelthTimer;
    private float initialTwelthTimer;
    //timer cross animation 12 - 13
    public float thirteenthTimer;
    private float initialThirteenthTimer;
    //timer cross animation 13 - 14;
    public float fourteenthTimer;
    private float initialFourteenthTimer;
    //timer cross animation 14 - 15
    public float fifthteenthTimer;
    private float initialFifthteenthTimer;
    //timer cross animation 15 - 16
    public float sixteenthTimer;
    private float initialSixteenthTimer;
    //timer cross animation 16 - 17
    public float  seventeenthTimer;
    private float initialSeventeenthimer;
    //timer cross animation 17 - 18
    public float eighteenthTimer;
    private float initialEighteenthTimer;
    //timer cross animation 18 - 19
    public float nineteenthTimer;
    private float initialNineteenthTimer;
    //timer cross animation 19 - 20
    public float twentiethTimer;
    private float initialTwentiethTimer;
    //timer cross animation 20 - 21
    public float twentyfirstTimer;
    private float initialTwentyfirstTimer;
    //timer cross animation 21 - 22
    public float twentysecondTimer;
    private float initialTwentysecondTimer;
    //timer cross animation 22 - 23
    public float twentythirdTimer;
    private float initialTwentythirdTimer;
    //timer cross animation 23 - 24
    public float twentyfourthTimer;
    private float initialTwentyfourthTimer;
    //cases
    private int cases;
    //box collider objects
    public GameObject PequiObject;
    public GameObject WaterObject;
    public GameObject MortarObject;
    public GameObject stoveObject;
    public GameObject shakerObject;
    //screen aspect
    private float currentAspect = (float)Screen.width / Screen.height;
    private float targetAspect = 16f / 9f;
    //coordenates
    private Vector2 currentCenter;
    private Vector2 targetCenter;

    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        EventManager.AddListener(IngredientState.Cooked, SixthVignette);
        EventManager.AddListener(PotionEvent.AddIngredient, eleventhVignette);
        EventManager.AddListener(PotionEvent.AddWater, sixteenthVignette);
        EventManager.AddListener(PotionEvent.Poof, nineteenthVignette);
        EventManager.AddListener(ClientEvent.Served, twentyfourthVignette);
    }
    void Start()
    {
        xCoordenates = 0.29f;
        yCoordenates = 0.5f;
        ActiveIntensity = 1f;
        initialDeactivaterTimer = deactivaterTimer;
        initialFirstTimer = FirstTimer;
        initialSecondTimer = secondTimer;
        initialThirdTimer = thirdTimer;
        initialFourthTimer = fourthTimer;
        initialFifthTimer = fifthTimer;
        initialSixthTimer = sixthTimer;
        initialSeventhTimer = seventhTimer;
        initialEightTimer = eightTimer;
        initialNinethTimer = ninethTimer;
        initialTenthTimer = tenthTimer;
        initialEleventhTimer = eleventhTimer;
        initialTwelthTimer = twelthTimer;
        initialThirteenthTimer = thirteenthTimer;
        initialFourteenthTimer = fourteenthTimer;
        initialFifthteenthTimer = fifthteenthTimer;
        initialSixteenthTimer = sixteenthTimer;
        initialSeventeenthimer = seventeenthTimer;
        initialEighteenthTimer = eighteenthTimer;
        initialNineteenthTimer = nineteenthTimer;
        initialTwentiethTimer = twentiethTimer;
        initialTwentyfirstTimer = twentyfirstTimer;
        initialTwentysecondTimer = twentysecondTimer;
        initialTwentythirdTimer = twentythirdTimer;
        initialTwentyfourthTimer = twentiethTimer;
        vignetteVolume = this.gameObject.GetComponent<Volume>();
        vignetteVolume.profile.TryGet<Vignette>(out vignetteInstance);
        cases = 0;
        PequiObject = GameObject.Find("Pequi");
        WaterObject = GameObject.Find("Water");

        LevelManager.CurrentLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (cases)
        {
            case 0:
                //objects setter
                deactivaterTimer -= Time.deltaTime;
                if (deactivaterTimer <= 0)
                {
                    MortarObject = GameObject.Find("Mortar(Clone)");
                    stoveObject = GameObject.Find("Stove(Clone)");
                    shakerObject = GameObject.Find("Potion(Clone)");
                    PequiObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    WaterObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    MortarObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    stoveObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
            break;
            case 1:
                //the vignette appears
                FirstTimer -= Time.deltaTime;
                if (FirstTimer <= 0 && initialIntensity <= ActiveIntensity)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    FirstTimer = initialFirstTimer;
                    initialIntensity += 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    secondVignette();
                }
                //vignetteInstance.center.value = new Vector2(5f, 5f);
            break;
            case 2:
                //waits to the player to see the ingredient
                secondTimer -= Time.deltaTime;
                if (secondTimer <= 0)
                {
                    thirdVignette();
                }
            break;
            case 3:
                // move givnette to morter
                thirdTimer -= Time.deltaTime;
                if (thirdTimer <= 0 && xCoordenates <= 0.48f && yCoordenates >= 0.27f)
                {
                    vignetteInstance.center.value = new Vector2(xCoordenates, yCoordenates);
                    xCoordenates += 0.005f;
                    yCoordenates -= 0.005f;
                    thirdTimer = initialThirdTimer;
                }
                if (xCoordenates >= 0.4799998f && yCoordenates <= 0.3100002f)
                {
                    fourthVignette();
                }
            break;
            case 4:
                //wait to show the morter
                fourthTimer -= Time.deltaTime;
                if (fourthTimer <= 0f)
                {
                    fifthVignette();
                }
            break;
            case 5:
                //vignette disapears
                fifthTimer -= Time.deltaTime;
                if (fifthTimer <= 0 && initialIntensity >= 0f)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    fifthTimer = initialFifthTimer;
                    initialIntensity -= 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    PequiObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    MortarObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
            break;
            case 6:
                //focus on morter
                PequiObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                MortarObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                sixthTimer -= Time.deltaTime;
                if (sixthTimer <= 0 && initialIntensity <= 1f)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    sixthTimer = initialSixthTimer;
                    initialIntensity += 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    seventhVignette();
                }
            break;
            case 7:
                //wait to player to see morter
                seventhTimer -= Time.deltaTime;
                if (seventhTimer <= 0)
                {
                    eightVignette();
                }
            break;
            case 8:
                //focus on shaker
                eightTimer-= Time.deltaTime;
                if (eightTimer <= 0 && xCoordenates >= 0.28f)
                {
                    xCoordenates -= 0.005f;
                    if (yCoordenates >= 0.27f)
                    {
                        yCoordenates -= 0.001f;
                    }
                    vignetteInstance.center.value = new Vector2(xCoordenates, yCoordenates);
                    eightTimer = initialEightTimer;
                }
                if (xCoordenates >= 0.28222f && yCoordenates <= 0.272222f)
                {
                    ninethVignette();
                }
            break;
                //vignette stops in shaker
            case 9:
                ninethTimer -= Time.deltaTime;
                if (ninethTimer <= 0)
                {
                    tenthVignette();
                }
            break;
            case 10:
                //vignette disapears from shaker
                tenthTimer -= Time.deltaTime;
                if (tenthTimer <= 0 && initialIntensity >= 0f)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    tenthTimer = initialTenthTimer;
                    initialIntensity -= 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    MortarObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
            break;
            case 11:
                //vignette focus on water
                eleventhTimer -= Time.deltaTime;
                shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                MortarObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                vignetteInstance.center.value = new Vector2(0.95f, 0.77f);
                if (eleventhTimer <= 0 && initialIntensity <= ActiveIntensity)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    eleventhTimer = initialEleventhTimer;
                    initialIntensity += 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    twelthVignette();
                    xCoordenates = vignetteInstance.center.value.x;
                    yCoordenates = vignetteInstance.center.value.y;
                }
            break;
            case 12:
                //vignette waits for the player to focus
                twelthTimer -= Time.deltaTime;
                if (twelthTimer <= 0)
                {
                    thirteenthVignette();
                }
            break;
            case 13:
                //vignette moves to shaker
                thirteenthTimer -= Time.deltaTime;
                if (thirteenthTimer <= 0 && xCoordenates >= 0.28f)
                {
                    xCoordenates -= 0.005f;
                    if (yCoordenates >= 0.27f)
                    {
                        yCoordenates -= 0.0045f;
                    }
                    vignetteInstance.center.value = new Vector2(xCoordenates, yCoordenates);
                    thirteenthTimer = initialThirteenthTimer;
                }
                if (xCoordenates <= 0.28222f && yCoordenates <= 0.272222f)
                {
                    fourteenthVignette();
                }
            break;
            case 14:
                //vignette stays on shaker
                fourteenthTimer -= Time.deltaTime;
                if (fourteenthTimer <= 0f)
                {
                    fifthteenthVignette();
                    xCoordenates = vignetteInstance.center.value.x;
                    yCoordenates = vignetteInstance.center.value.y;
                }
            break;
            case 15:
                //vignette disapears
                fifthteenthTimer -= Time.deltaTime;
                if (fifthteenthTimer <= 0 && initialIntensity >= 0f)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    fifthteenthTimer = initialFifthteenthTimer;
                    initialIntensity -= 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    WaterObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
            break;
            case 16:
                //vignette focus on shaker
                WaterObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                sixteenthTimer -= Time.deltaTime;
                if (sixteenthTimer <= 0 && initialIntensity <= ActiveIntensity)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    sixteenthTimer = initialSixteenthTimer;
                    initialIntensity += 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    seventeenthVignette();
                }
            break;
            case 17:
                //vignette stays on shaker
                seventeenthTimer -= Time.deltaTime;
                if (seventeenthTimer <= 0f)
                {
                    eighteenthVignette();
                }
            break;
            case 18:
                //Vignette disapears from shaker
                eighteenthTimer -= Time.deltaTime;
                if (eighteenthTimer <= 0 && initialIntensity >= 0f)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    eighteenthTimer = initialEighteenthTimer;
                    initialIntensity -= 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
            break;
            case 19:
                //focus on shaker
                shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                nineteenthTimer -= Time.deltaTime;
                if (nineteenthTimer <= 0 && initialIntensity <= ActiveIntensity)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    nineteenthTimer = initialNineteenthTimer;
                    initialIntensity += 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    twentiethVignette();
                }
            break;
            case 20:
                //keeps the focus on shaker
                twentiethTimer -= Time.deltaTime;
                if (twentiethTimer <= 0)
                {
                    twentyfirstVignette();
                }
            break;
            case 21:
                //move to client
                twentyfirstTimer -= Time.deltaTime;
                Debug.Log(yCoordenates);
                if (twentyfirstTimer <= 0 && yCoordenates <= 0.62f)
                {
                    Debug.Log(xCoordenates);
                    yCoordenates += 0.005f;
                    if (xCoordenates <= 0.35f)
                    {
                        xCoordenates += 0.001f;
                    }
                    vignetteInstance.center.value = new Vector2(xCoordenates, yCoordenates);
                    twentyfirstTimer = initialTwentyfirstTimer;
                }
                if (yCoordenates >= 0.61888f)
                {
                    if (xCoordenates >= 0.34888f)
                    {
                        twentysecondVignette();
                    }
                }
             break;
            case 22:
                //wait on client
                twentysecondTimer -= Time.deltaTime;
                if (twentysecondTimer <= 0)
                {
                    twentythirdVignette();
                }
            break;
            case 23:
                //vignette disapears on client
                twentythirdTimer -= Time.deltaTime;
                if (twentythirdTimer <= 0 && initialIntensity >= 0f)
                {
                    vignetteInstance.intensity.value = initialIntensity;
                    twentythirdTimer = initialTwentythirdTimer;
                    initialIntensity -= 0.01f;
                }
                if (initialIntensity >= ActiveIntensity)
                {
                    shakerObject.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                }
            break;
            case 24:
                twentyfourthTimer -= Time.deltaTime;
                if (twentyfourthTimer <= 0)
                {
                    EventManager.Dispatch(GlobalEvent.TutorialCompleted);

                    LoadKitchen1();
                }
            break;
        }
    }

    void LoadKitchen1() => SceneManager.LoadScene("Kitchen1");

    #region Vignettes
    public void initialVignette()
    {
        cases = 1;
    }
    public void secondVignette()
    {
        cases = 2;
    }
    public void thirdVignette()
    {
        cases = 3;
    }
    public void fourthVignette()
    {
        cases = 4;
    }
    public void fifthVignette()
    {
        cases = 5;
    }
    public void SixthVignette()
    {
        cases = 6;
    }
    public void seventhVignette()
    {
        cases = 7;
    }
    public void eightVignette()
    {
        cases = 8;
    }
    public void ninethVignette()
    {
        cases = 9;
    }
    public void tenthVignette()
    {
        cases = 10;
    }
    public void eleventhVignette()
    {
        cases = 11;
    }
    public void twelthVignette()
    {
        cases = 12;
    }
    public void thirteenthVignette()
    {
        cases = 13;
    }
    public void fourteenthVignette() 
    {
        cases = 14;
    }
    public void fifthteenthVignette()
    {
        cases = 15;
    }
    public void sixteenthVignette()
    {
        cases = 16;
    }
    public void seventeenthVignette()
    {
        cases = 17;
    }
    public void eighteenthVignette()
    {
        cases = 18;
    }
    public void nineteenthVignette()
    {
        cases = 19;
    }
    public void twentiethVignette()
    {
        cases = 20;
    }
    public void twentyfirstVignette()
    {
        cases = 21;
    }
    public void twentysecondVignette()
    {
        cases = 22;
    }
    public void twentythirdVignette()
    {
        cases = 23;
    }
    public void twentyfourthVignette()
    {
        cases = 24;
    }

        #endregion
        void OnDestroy()
    {
        EventManager.RemoveListener(IngredientState.Cooked, SixthVignette);
        EventManager.RemoveListener(PotionEvent.AddIngredient, eleventhVignette);
        EventManager.RemoveListener(PotionEvent.AddWater, sixteenthVignette);
        EventManager.RemoveListener(PotionEvent.Poof, nineteenthVignette);
    }
    public void calculateCenter()
    {
        currentCenter = (Vector2)vignetteInstance.center;
        currentCenter.x /= currentAspect;
        targetCenter = currentCenter * targetAspect;
        targetCenter.x *= currentAspect;
        vignetteInstance.center.value = targetCenter;
    }
}
