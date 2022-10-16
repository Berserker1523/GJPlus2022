using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace HistoryBook {
    public class HistoryBookManager : MonoBehaviour
    {
        [SerializeField] public LegendsScriptableObject mythsDatabase;
        [SerializeField] public TextMeshProUGUI historyText;
        [SerializeField] public TextMeshProUGUI titleText;
        [SerializeField] public Image associatedIngredient;

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform mythsList;
        [SerializeField] private Scrollbar scrollbar;

        [HideInInspector] public List<Button> buttonsList;
        [HideInInspector] public string mainMenuSceneName = "MainMenu";

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
            scrollbar.value = 1;
        }

        private void CreateBookEntrys()
        {
            foreach (Myth myth in mythsDatabase.myths)
            {
                GameObject button = Instantiate(buttonPrefab, mythsList);
                button.name = myth.name;
                button.GetComponentInChildren<TextMeshProUGUI>().text = myth.name;
                Button newbutton = button.GetComponent<Button>();
                newbutton.onClick.AddListener(delegate { HandleChangeText(myth.name, myth.description, myth.ingredientSprite); });
                buttonsList.Add(newbutton);
            }
            // DefaultEntrySetted?.Invoke(0);
            EventManager.Dispatch(EventsHistoryBook.setDefault, 0);
        }

        public void HandleChangeText(string mythName, string mythDescription, Sprite ingredient)
        {
            titleText.text = mythName;
            historyText.text = mythDescription;
            associatedIngredient.sprite = ingredient;
        }

        public void SetDefaultEntry(int entryId)
        {
            buttonsList[entryId].Select();
            HandleChangeText(mythsDatabase.myths[entryId].name, mythsDatabase.myths[entryId].description,mythsDatabase.myths[entryId].ingredientSprite);
        }

        public void Back()
        {
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
