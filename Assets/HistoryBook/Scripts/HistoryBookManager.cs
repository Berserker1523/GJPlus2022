using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HistoryBook {
    public class HistoryBookManager : MonoBehaviour
    {
        [SerializeField] public LegendsScriptableObject mythsDatabase;
        [SerializeField] public TextMeshProUGUI historyText;
        [SerializeField] public TextMeshProUGUI titleText;

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform mythsList;
        [SerializeField] private Scrollbar scrollbar;

        [HideInInspector] public List<Button> buttonsList;

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
                newbutton.onClick.AddListener(delegate { HandleChangeText(myth.name, myth.description); });
                buttonsList.Add(newbutton);
            }
            // DefaultEntrySetted?.Invoke(0);
            EventManager.Dispatch(EventsHistoryBook.setDefault, 0);
        }

        public void HandleChangeText(string mythName, string mythDescription)
        {
            titleText.text = mythName;
            historyText.text = mythDescription;
        }

        public void SetDefaultEntry(int entryId)
        {
            buttonsList[entryId].Select();
            HandleChangeText(mythsDatabase.myths[entryId].name, mythsDatabase.myths[entryId].description);
        }
    }
}
