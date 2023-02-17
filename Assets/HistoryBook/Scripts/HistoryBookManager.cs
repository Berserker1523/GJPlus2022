using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Kitchen;

namespace HistoryBook {
    public class HistoryBookManager : MonoBehaviour
    {
        [SerializeField] public LegendsScriptableObject mythsDatabase;
        [SerializeField] public TextMeshProUGUI historyText;
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public TextMeshProUGUI mythTitle;
        [SerializeField] public TextMeshProUGUI zoneText;
        [SerializeField] public Image associatedIngredient;

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform mythsList;
        [SerializeField] private Scrollbar scrollbar;

        [HideInInspector] public List<Button> buttonsList;
        [HideInInspector] public string mainMenuSceneName = "MainMenu";
        [HideInInspector] public string lockedTextTag = "[Earn Stars To unlock more] "; 

        //public event Action<int> DefaultEntrySetted;

        public enum EventsHistoryBook
        {
            setDefault
        }

        private void Awake()
        {
            buttonPrefab = Resources.Load<GameObject>("BookEntryButton");
            //DefaultEntrySetted += SetDefaultEntry;     
            EventManager.AddListener<int>(EventsHistoryBook.setDefault, SetDefaultEntry);
        }

        private void OnDestroy()
        {
            //DefaultEntrySetted -= SetDefaultEntry;
            EventManager.RemoveListener<int>(EventsHistoryBook.setDefault, SetDefaultEntry);
        }

        private void Start()
        {
            CreateBookEntrys();
            
        }

        private void CreateBookEntrys()
        {
            foreach (Myth myth in mythsDatabase.myths)
            {
                GameObject button = Instantiate(buttonPrefab, mythsList);
                button.name = myth.name;
                button.GetComponentInChildren<TextMeshProUGUI>().text = myth.name;
                Button newbutton = button.GetComponent<Button>();
                buttonsList.Add(newbutton);
                newbutton.onClick.AddListener((UnityEngine.Events.UnityAction)delegate { HandleChangeText((string)myth.ingredient.ToString(),(string)myth.name, myth.description, myth.region, myth.mythP1, myth.mythP2, myth.ingredientSprite, buttonsList.IndexOf(newbutton)); });
               
                //Myths limitation for vertical slice
                if (buttonsList.Count >= 3)
                    break;
            }
            // DefaultEntrySetted?.Invoke(0);
            EventManager.Dispatch(EventsHistoryBook.setDefault, 0);
        }

        public void HandleChangeText(string ingredientName, string mythName, string mythDescription, string region, string mythP1 , string mythP2,Sprite ingredient, int buttonPos)
        {
            titleText.text = ingredientName;
            mythTitle.text = mythName;
            zoneText.text = region;
            associatedIngredient.sprite = ingredient;
            scrollbar.value = 1;

            GameData gameData = SaveManager.LoadStarsData();
            if (!gameData.stars[buttonPos, 1])           
                mythP1 = lockedTextTag;

            if (!gameData.stars[buttonPos, 2])
                mythP2 = lockedTextTag;

            string mythText = mythDescription+ "\n\n"+mythP1+"\n\n"+mythP2;

            historyText.text = mythText;

        }

        public void SetDefaultEntry(int entryId)
        {
            buttonsList[entryId].Select();
            HandleChangeText(mythsDatabase.myths[entryId].ingredient.ToString(), mythsDatabase.myths[entryId].name, mythsDatabase.myths[entryId].description, mythsDatabase.myths[entryId].region, mythsDatabase.myths[entryId].mythP1, mythsDatabase.myths[entryId].mythP2, mythsDatabase.myths[entryId].ingredientSprite, 0);
        }

        public void Back()
        {
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
