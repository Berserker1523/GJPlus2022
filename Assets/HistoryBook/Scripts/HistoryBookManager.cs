using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Kitchen;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using System.Text.RegularExpressions;

namespace HistoryBook {
    public class HistoryBookManager : MonoBehaviour
    {
        [SerializeField] public LegendsScriptableObject mythsDatabase;
        [SerializeField] public TextMeshProUGUI historyText;
        [SerializeField] public TextMeshProUGUI attendedPatientsText;
        [SerializeField] public LocalizeStringEvent titleText;
        [SerializeField] public LocalizeStringEvent mythTitle;
        [SerializeField] public LocalizeStringEvent zoneText;
        [SerializeField] public Image associatedIngredient;

        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private GameObject buttonLockedPrefab;
        [SerializeField] private Transform mythsList;
        [SerializeField] private Scrollbar scrollbar;

        [HideInInspector] public List<Button> buttonsList;
        [HideInInspector] public string mainMenuSceneName = "MainMenu";
        [HideInInspector] public string lockedTextTag = "[Earn Stars To unlock more] ";

        [SerializeField] public Sprite selectedTagSprite;
        [SerializeField] public Sprite unselectedTagSprite; 

        //public event Action<int> DefaultEntrySetted;

        public enum EventsHistoryBook
        {
            setDefault
        }

        private void Awake()
        {
            buttonPrefab = Resources.Load<GameObject>("BookEntryButton");
            buttonLockedPrefab = Resources.Load<GameObject>("BookEntryButtonLocked");
            new GameObject("SoundsManager").AddComponent<SoundsManager>();
            //DefaultEntrySetted += SetDefaultEntry;     
            EventManager.AddListener<int>(EventsHistoryBook.setDefault, SetDefaultEntry);
           
        }

        private void OnDestroy()
        {
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

                button.GetComponentsInChildren<Image>()[1].sprite = myth.ingredientSprite;
                Button newbutton = button.GetComponent<Button>();
                buttonsList.Add(newbutton);
                newbutton.onClick.AddListener((UnityEngine.Events.UnityAction)delegate { HandleChangeText(myth.ingredient,myth.name, myth.description, myth.region, myth.mythP1, myth.mythP2, myth.ingredientSprite, buttonsList.IndexOf(newbutton)); });
               
                //Myths limitation for vertical slice
                if (buttonsList.Count >= 3)
                    break;
            }

            //Myths limitation for vertical slice
                GameObject buttonlocked1 =   Instantiate(buttonLockedPrefab, mythsList);
                GameObject buttonlocked2 = Instantiate(buttonLockedPrefab, mythsList);

                buttonlocked1.GetComponent<Button>().onClick.AddListener(HandleClickLockedButton);
                buttonlocked2.GetComponent<Button>().onClick.AddListener(HandleClickLockedButton);

            EventManager.Dispatch(EventsHistoryBook.setDefault, 0);


            StartCoroutine(ResetHandlePos());
        }

        public void HandleChangeText(LocalizedString ingredientName, LocalizedString mythName, LocalizedString mythDescription, LocalizedString region, LocalizedString mythP1 , LocalizedString mythP2,Sprite ingredient, int buttonPos)
        {
            titleText.StringReference = ingredientName;
            mythTitle.StringReference = mythName;
            zoneText.StringReference = region;
            associatedIngredient.sprite = ingredient;
            scrollbar.value = 0.9999f;

            GameData gameData = SaveManager.LoadStarsData();

            if (gameData == null)
                gameData = new GameData();

            string value1, value2;

            if (gameData.stars[buttonPos, 1])
                value1 = LocalizationSettings.StringDatabase.GetLocalizedString(mythP1.TableReference, mythP1.TableEntryReference);
            else
                value1 = "\n\n" + lockedTextTag;

            if (gameData.stars[buttonPos, 2])
                value2 = LocalizationSettings.StringDatabase.GetLocalizedString(mythP2.TableReference, mythP2.TableEntryReference);
            else
                value2 = "\n\n" + lockedTextTag;

            var tableReference = mythDescription.TableReference;

            string mythText = value1 + value2;

            historyText.text = mythText;

            EventManager.Dispatch(GlobalEvent.Unlocked);
            UpdateButtonTags(buttonPos);
            
            attendedPatientsText.text = gameData.attendedClients[(int)mythsDatabase.myths[buttonPos].refIngredient].ToString();
            StartCoroutine( ResetHandlePos());
        }

        private void UpdateButtonTags(int buttonPos)
        {
            for (int i = 0; buttonsList.Count > i; i++)
            {
                Image[] buttonSprites = buttonsList[i].GetComponentsInChildren<Image>();

                if (i != buttonPos)
                {
                    buttonSprites[0].rectTransform.sizeDelta = new Vector2(120f, 130f);
                    buttonSprites[0].sprite = unselectedTagSprite;
                    buttonSprites[1].rectTransform.localPosition = new Vector3(0f, 0, 0);
                }
                else
                {
                    buttonSprites[0].rectTransform.sizeDelta = new Vector2(220f, 130f);
                    buttonSprites[0].sprite = selectedTagSprite;
                    buttonSprites[1].rectTransform.localPosition = new Vector3(30f, 0, 0);
                }
            }
        }

        private IEnumerator ResetHandlePos()
        {
            yield return new WaitForSeconds(0.1f);
            scrollbar.value = 0.9999f; 
        }

        public void SetDefaultEntry(int entryId)
        {
            buttonsList[entryId].Select();
            HandleChangeText(mythsDatabase.myths[entryId].ingredient, mythsDatabase.myths[entryId].name, mythsDatabase.myths[entryId].description, mythsDatabase.myths[entryId].region, mythsDatabase.myths[entryId].mythP1, mythsDatabase.myths[entryId].mythP2, mythsDatabase.myths[entryId].ingredientSprite, 0);
        }

        public void Back()
        {
            EventManager.Dispatch(GlobalEvent.Unlocked);
            SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Single);
        }

        public void HandleClickLockedButton() =>
            EventManager.Dispatch(GlobalEvent.Locked);
    }
}
