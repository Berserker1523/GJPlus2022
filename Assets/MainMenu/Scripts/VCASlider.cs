using UnityEngine;
using UnityEngine.UI;

public class VCASlider : MonoBehaviour
{

    private FMOD.Studio.VCA VcaController;
    public string vcaName;

    Slider mySlider;
    // Start is called before the first frame update
    void Start()
    {
        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        mySlider = GetComponent<Slider>();
    }

    public void setVCAVolume(float volume)
    {
        VcaController.setVolume(volume);
    }
}
