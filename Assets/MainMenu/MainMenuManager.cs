using Events;
using Kitchen;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mainmenu
{
    public class MainMenuManager : MonoBehaviour
    {
        GameData gameData;
        public void Awake()
        {
            new GameObject("SoundsManager").AddComponent<SoundsManager>();
        }

        private void Start()
        {
             gameData = SaveManager.LoadStarsData();           
        }

        public void PlayGame()=>  StartCoroutine(OpenKitchenScene());

        private IEnumerator OpenKitchenScene()
        {
            EventManager.Dispatch(GlobalEvent.Play);
            yield return new WaitForSeconds(2f);

           // if (gameData != null && gameData.tutorialCompleted)
                SceneManager.LoadScene($"{SceneName.Kitchen}{/*LevelManager.CurrentLevel*/1}", LoadSceneMode.Single); //Todo level save
            //else
               // SceneManager.LoadScene("Tutorial");
        }


        public void Credits()
        {
            SceneManager.LoadSceneAsync(SceneName.Credits.ToString(), LoadSceneMode.Single);
        }

        public void History()
        {
            SceneManager.LoadSceneAsync(SceneName.HistoryBook.ToString(), LoadSceneMode.Single);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
