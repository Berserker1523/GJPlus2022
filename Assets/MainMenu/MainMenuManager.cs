using Events;
using Kitchen;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mainmenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Awake()
        {
            new GameObject("SoundsManager").AddComponent<SoundsManager>();
        }

        public void PlayGame() =>
          StartCoroutine(OpenKitchenScene());


        private IEnumerator OpenKitchenScene()
        {
            EventManager.Dispatch(GlobalEvent.Play);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene($"{SceneName.Kitchen}{/*LevelManager.CurrentLevel*/1}", LoadSceneMode.Single); //Todo level save
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
