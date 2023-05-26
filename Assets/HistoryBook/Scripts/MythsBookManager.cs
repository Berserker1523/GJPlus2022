using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
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

        private void Start()
        {
            MythsBookTopTab.currentTabSwitchedEvent += TopTabSwitched;
            MythsBookLeftTab.currentTabSwitchedEvent += LeftTabSwitched;
            TopTabSwitched(new MythsBookTopTab());
        }

        private void OnDestroy()
        {
            MythsBookTopTab.currentTabSwitchedEvent -= TopTabSwitched;            
            MythsBookTopTab.currentTabSwitchedEvent -= TopTabSwitched;            
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

                leftTab.SetBookEntry(bookEntries[i].bookEntryType, bookEntries[i].name, bookEntries[i].texts[0], bookEntries[i].goals[0], bookEntries[i].sprite);
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
           // descriptionText.text = leftTab._description;
        }
    }
}