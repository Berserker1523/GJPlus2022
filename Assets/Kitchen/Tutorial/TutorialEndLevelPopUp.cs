using Events;
using Kitchen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialEndLevelPopUp : MonoBehaviour
{
    int tutorialId;
    [SerializeField] protected Button positiveButton;

    private void Awake()
    {
        tutorialId = FindObjectOfType<TutorialPopUpComponent>().tutorialId;
        EventManager.AddListener(GameStatus.LevelFinished, HandleWon);
        positiveButton.onClick.AddListener(NextLevel);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(GameStatus.LevelFinished, HandleWon);
        positiveButton.onClick.RemoveListener(NextLevel);
    }

    private void HandleWon()
    {
        gameObject.SetActive(true);

    }

    public void NextLevel()
    {
        if (LevelManager.CurrentLevel != 0)
            SceneManager.LoadScene($"{SceneName.Kitchen}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        else
            SceneManager.LoadScene(("Tutorial" + (tutorialId + 1)));
    }

}
