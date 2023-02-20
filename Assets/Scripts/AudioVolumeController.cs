using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioVolumeView
{
    event Action<float> VolumeChanged;

    void SetVolumeWithoutNotify(float volume);
}

public class AudioVolumeController : MonoBehaviour
{
    [SerializeField] private string vcaName;

    private IAudioVolumeView[] audioVolumeViews;
    private VCA vcaController;

    private float Volume
    {
        get => PlayerPrefs.GetFloat(vcaName, 1);
        set => PlayerPrefs.SetFloat(vcaName, value);
    }

    private void Awake()
    {
        audioVolumeViews = GetComponentsInChildren<IAudioVolumeView>();
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        foreach (IAudioVolumeView audioVolumeView in audioVolumeViews)
            audioVolumeView.VolumeChanged += SetVolume;
    }

    private void Start() =>
        SetVolume(Volume);

    private void OnDestroy()
    {
        foreach (IAudioVolumeView audioVolumeView in audioVolumeViews) 
            audioVolumeView.VolumeChanged -= SetVolume; 
    }

    private void SetVolume(float volume)
    {
        Volume = volume;
        vcaController.setVolume(volume);
        foreach (IAudioVolumeView audioVolumeView in audioVolumeViews)
            audioVolumeView.SetVolumeWithoutNotify(volume);
    }
}
