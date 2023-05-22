using Kitchen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSelectorPadlocks : MonoBehaviour
{
    [SerializeField] GameObject[] tutorialPadLocks;

    void Start()
    {
        CheckTutorialsData();
    }

    void CheckTutorialsData()
    {
        GameData gameData = SaveManager.LoadStarsData();

        if (gameData != null)
        {
            for (int i = 0; i < gameData.tutorials.Length; i++)
            {
                if (gameData.tutorials[i])
                {
                    tutorialPadLocks[i].SetActive(false);
                }
            }
        }

    }
}
