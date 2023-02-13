using Kitchen;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace LevelSelector
{
    public class PregamePopUp : MonoBehaviour
    {

        public LevelData level;
        [SerializeField] public Image[] stars;
        public TextMeshProUGUI text;
        [SerializeField] private LocalizedString localStringLevel;
        public UnityAction<bool> popUpEnabled;
        public UnityAction<bool> enablePlayButton;

        private int levelNum;

        private void Start()=>     
            gameObject.SetActive(false);
        
        private void OnEnable()
        {
            localStringLevel.Arguments = new object[] { levelNum };
            localStringLevel.StringChanged += UpdateText;
        }

        private void OnDisable()=>      
            localStringLevel.StringChanged -= UpdateText;
      
        public void EnablePopUP(LevelData newLevel)
        {
            level = newLevel;
            UpdateLevel();
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

        public void EnablePlayButton()=>     
            enablePlayButton?.Invoke(true);
        
        private void UpdateLevel()
        {
            levelNum = level.level;
            localStringLevel.Arguments[0] = levelNum;
            localStringLevel.RefreshString(); 
        }

        private void UpdateText(string value)=>
            text.text = value;
        
    }
}
