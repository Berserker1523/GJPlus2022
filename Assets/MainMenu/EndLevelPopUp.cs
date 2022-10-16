using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndLevelPopUp : MonoBehaviour
{
    [SerializeField] GameObject upgradesPopUp;
    [SerializeField] string mainMenuSceneName="MainMenu";
    [SerializeField] string kitchenScene="Kitchen";

    private void Start()
    {
        UpgradesPopUp(false);
    }

    public void NextLevel()
    {      
        SceneManager.LoadScene(kitchenScene, LoadSceneMode.Single);
    }

    public void UpgradesPopUp(bool showPopUP)
    {
        upgradesPopUp.SetActive(showPopUP);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
    }

}
