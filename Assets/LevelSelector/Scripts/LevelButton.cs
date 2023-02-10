using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kitchen;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace LevelSelector
{
    public class LevelButton : MonoBehaviour
    {

        [SerializeField] public LevelData level;
        [SerializeField] SpriteRenderer[] stars;
        [SerializeField] public TextMesh text;

        private void Awake()
        {
            stars =  transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        }
        private void Start()
        {
            for (int i=0; i<stars.Length; i++)
            {
                if (!level.stars[i])
                    stars[i].color= Color.black;
            }
        }

        public void OnMouseDown()
        {
            SceneManager.LoadScene("Kitchen1");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject[] gameObjects = scene.GetRootGameObjects();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<LevelInstantiator>() != null)
                {
                    gameObject.GetComponent<LevelInstantiator>().SetLevelData(level);
                    break;
                }
            }
        }

    }
}
