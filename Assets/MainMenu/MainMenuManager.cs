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
        GameObject _musicObject;
        public void Awake()
        {
            new GameObject("SoundsManager").AddComponent<SoundsManager>();
        }

        private void Start()
        {
            gameData = SaveManager.LoadStarsData();
            _musicObject = GameObject.Find("Music");
        }

        public void PlayGame() => StartCoroutine(OpenKitchenScene());

        private IEnumerator OpenKitchenScene()
        {
            EventManager.Dispatch(GlobalEvent.Play);
            Destroy(_musicObject);
            yield return new WaitForSeconds(2f);

            if (gameData != null)
            {
                for (int i = 0; i < gameData.tutorials.Length; i++)
                {
                    if (!gameData.tutorials[i])
                    {
                        SceneManager.LoadScene("Tutorial" + (i));
                        yield return null;
                    }
                }
                SceneManager.LoadScene($"{SceneName.Kitchen}{LevelManager.CurrentLevel}", LoadSceneMode.Single); //Todo level save
            }
        }

        /* ==== ICase of want to only display trash tutorial once   ====
         [SerializeField] GameObject _triggerablesTutorialsCanvas;
         [SerializeField] GameObject _trashTutorialPrefab;
         public void CheckTriggerableTutorials()
         {
             if (!gameData.tutorials[(int)GlobalTrigerableTutorialEvent.TrashTutorialTriggered])           
                Instantiate(_trashTutorialPrefab, _triggerablesTutorialsCanvas.transform);          
         }   */

        public void Credits()
        {
            SceneManager.LoadSceneAsync(SceneName.Credits2.ToString(), LoadSceneMode.Single);
        }

        public void History()
        {
            SceneManager.LoadSceneAsync(SceneName.HistoryBook.ToString(), LoadSceneMode.Additive);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
