using Events;
using Kitchen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStreakRestarter : MonoBehaviour
{
    TutorialPopUpComponent _tutorialPopUp;
    [SerializeField] GameObject _tryAgainGO;

    private void Awake()
    {
        EventManager.AddListener(LevelEvents.StreakLost, RestartLevel);
        _tutorialPopUp = FindObjectOfType<TutorialPopUpComponent>();
    }

    private void RestartLevel()
    {

    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(LevelEvents.StreakLost, RestartLevel);       
    }
}
