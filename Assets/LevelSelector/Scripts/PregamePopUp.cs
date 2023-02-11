using Kitchen;
using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace LevelSelector
{
    public class PregamePopUp : MonoBehaviour
    {

        public LevelData level;
        [SerializeField] public Image[] stars;
        public TextMeshProUGUI text;
        public UnityAction<bool> popUpEnabled;
        public UnityAction<bool> enablePlayButton;
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void EnablePopUP(LevelData newLevel, string newText)
        {
            level = newLevel;
            text.text = "Level "+ newText;
            for (int i = 0; i < stars.Length; i++)
            {
                if (!level.stars[i])
                    stars[i].color = Color.black;
                else
                    stars[i].color = Color.white;
            }
            popUpEnabled?.Invoke(true);  
            gameObject.SetActive(true);
        }

        public void DisablePopUp()
        {
            popUpEnabled?.Invoke(false);
            gameObject.SetActive(false);
            enablePlayButton?.Invoke(false);
        }

        public void EnablePlayButton()
        {
            enablePlayButton?.Invoke(true);
        }
    }
}
