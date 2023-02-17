using Kitchen;
using UnityEngine;
using UnityEngine.SceneManagement;
using Events;
using System.Collections;

namespace Mainmenu
{
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
            new GameObject("SoundsManager").AddComponent<SoundsManager>();
        }

        public void PlayGame()=>     
          StartCoroutine(OpenKitchenScene());
        

        private IEnumerator OpenKitchenScene()
        {
            EventManager.Dispatch(GlobalEvent.Play);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene($"Kitchen{LevelManager.CurrentLevel}", LoadSceneMode.Single);
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
}
