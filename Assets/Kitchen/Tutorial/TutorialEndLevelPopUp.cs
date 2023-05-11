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
    }

    private void OnDestroy() =>
        EventManager.RemoveListener(GameStatus.LevelFinished, HandleWon);

    private void HandleWon()
    {
        gameObject.SetActive(true);
        positiveButton.onClick.AddListener(NextLevel);
    }

    public void NextLevel()
    {
        if (LevelManager.CurrentLevel != 0)
            SceneManager.LoadScene($"{SceneName.Kitchen}{LevelManager.CurrentLevel}", LoadSceneMode.Single);
        else
          SceneManager.LoadScene(("Tutorial" + (tutorialId + 1)));
    }

}
