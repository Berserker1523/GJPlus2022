using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteManagerScript : MonoBehaviour
{
    //post process values
    private Volume vignetteVolume;
    private Vignette vignetteInstance;
    //post process coordenates
    private float xCoordenates;
    private float yCoordenates;
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
    //cases
    private int cases;
    //box collider objects
    private GameObject PequiObject;
    private GameObject WaterObject;
    private GameObject MortarObject;
    private GameObject stoveObject;
    // Start is called before the first frame update
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
        vignetteVolume = this.gameObject.GetComponent<Volume>();
        vignetteVolume.profile.TryGet<Vignette>(out vignetteInstance);
        cases = 0;
        PequiObject = GameObject.Find("Pequi");
        WaterObject = GameObject.Find("Water");
    }

    // Update is called once per frame
    void Update()
    {
        switch (cases)
        {
            case 0:
                deactivaterTimer -= Time.deltaTime;
                if (deactivaterTimer <= 0)
                {
                    MortarObject = GameObject.Find("Mortar(Clone)");
                    stoveObject = GameObject.Find("Stove(Clone)");
                    PequiObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    WaterObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    MortarObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    stoveObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                }
            break;
            case 1:
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
                secondTimer -= Time.deltaTime;
                if (secondTimer <= 0)
                {
                    thirdVignette();
                }
            break;
            case 3:
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
                fourthTimer -= Time.deltaTime;
                if (fourthTimer <= 0f)
                {
                    fifthVignette();
                }
            break;
            case 5:
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
        }

    }
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
    public void Test()
    {
        vignetteInstance.center.value = new Vector2(5f,5f);
    }
}
