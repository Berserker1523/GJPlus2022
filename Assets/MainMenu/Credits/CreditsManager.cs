using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mainmenu
{
    public class CreditsManager : MonoBehaviour
    {
        [HideInInspector] public string mainMenuSceneName = "MainMenu";

        public void Back()
        {
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
