using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    public enum SceneNames
    {
        MainMenu, Kitchen, History, Credits, Garden
    }

    public void Awake()
    {
        settingsPanel.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(((int)SceneNames.Kitchen), LoadSceneMode.Single);
    }

    public void Options()
    {
        settingsPanel.SetActive(settingsPanel.activeSelf ? false : true);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(((int)SceneNames.Credits), LoadSceneMode.Single);
    }
    public void History()
    {
        SceneManager.LoadSceneAsync(((int)SceneNames.History), LoadSceneMode.Single);
    } 
    public void Quit()
    {
        Application.Quit();
    }

}
