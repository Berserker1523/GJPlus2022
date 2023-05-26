using Events;
using Kitchen;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HistoryBook
{
    public class MythsBookManager : MonoBehaviour
    {
        [SerializeField] public LegendsScriptableObject mythsDatabase;
        [SerializeField] public GameObject leftList;
        [SerializeField] public GameObject leftTabPrefab;

        [SerializeField] public TextMeshProUGUI descriptionText;
        [SerializeField] public LocalizeStringEvent titleText;
        [SerializeField] public LocalizeStringEvent lockedTextTag;
        [SerializeField] public Image refImage;

        [SerializeField] Scrollbar scrollbar;
        GameData gameData;

        private void Awake()
        {
            gameData = SaveManager.LoadStarsData();
            if (gameData == null)
                gameData = new GameData();

            MythsBookTopTab.currentTabSwitchedEvent += TopTabSwitched;
            MythsBookLeftTab.currentTabSwitchedEvent += LeftTabSwitched;
            TopTabSwitched(new MythsBookTopTab()); 

        }

        private void OnDestroy()
        {
            MythsBookTopTab.currentTabSwitchedEvent -= TopTabSwitched;            
            MythsBookTopTab.currentTabSwitchedEvent -= TopTabSwitched;            
        }

        public void Back()
        {
            EventManager.Dispatch(GlobalEvent.Unlocked);
            SceneManager.UnloadSceneAsync(SceneName.HistoryBookRework.ToString());
        }

        private void TopTabSwitched(MythsBookTopTab newTab)
        {
            ENUM_bookTabs tabType = newTab._bookEntryType;
            BookEntry[] bookEntry = new BookEntry[] { };
            switch (tabType)
            {
                case ENUM_bookTabs.Myths:
                    bookEntry = mythsDatabase.myths;
                break;
                    case ENUM_bookTabs.Ingredients:
                    bookEntry = mythsDatabase.ingredients;
                break;
                    case ENUM_bookTabs.Indigenous:
                    bookEntry = mythsDatabase.indigenousCommunities;
                break;
                    case ENUM_bookTabs.Places:
                    bookEntry = mythsDatabase.places;
                break;
                    default:
                        bookEntry = mythsDatabase.myths;
                    break;
            }
            RemoveTabs();
            InstantiateTabs(bookEntry);
        }

        private void InstantiateTabs(BookEntry[] bookEntries)
        {
            for(int i=0; i<bookEntries.Length; i++) 
            { 
                GameObject newTab = Instantiate(leftTabPrefab, leftList.transform);
                MythsBookLeftTab leftTab = newTab.GetComponent<MythsBookLeftTab>();

                ENUM_bookTabs type = bookEntries[i].bookEntryType;
                switch (type)
                {
                    case ENUM_bookTabs.Indigenous:
                    leftTab.SetBookEntry(gameData,(IndigenousCommunity) bookEntries[i]);
                        break;
                    case ENUM_bookTabs.Ingredients:
                        leftTab.SetBookEntry(gameData, (Ingredient) bookEntries[i]);
                        break;
                    default:
                        leftTab.SetBookEntry(gameData, bookEntries[i]);
                        break;
                }


                if (i == 0)
                {
                    Debug.Log(leftTab.name);
                    MythsBookLeftTab.currentTabSwitchedEvent?.Invoke(leftTab); 
                }
            }
        }

        private void RemoveTabs()
        {
            foreach(Transform tab in leftList.transform)
                Destroy(tab.gameObject);
        }


        private void LeftTabSwitched(MythsBookLeftTab leftTab)
        {
            titleText.StringReference = leftTab._title;
            lockedTextTag.StringReference = leftTab._goal;
            refImage.sprite = leftTab._sprite;
            descriptionText.text = leftTab._description;
            StartCoroutine(ResetHandlePos());
        }

        private IEnumerator ResetHandlePos()
        {
            yield return new WaitForSeconds(0.1f);
            scrollbar.value = 0.9999f;
        }
    }
}