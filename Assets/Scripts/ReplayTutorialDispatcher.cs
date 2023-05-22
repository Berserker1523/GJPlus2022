using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReplayTutorialDispatcher : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(DispatchReplay);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(DispatchReplay);       
    }

    void DispatchReplay()
    {
        EventManager.Dispatch(GlobalEvent.Play);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) => EventManager.Dispatch(GlobalTutorialEvent.replayingTutorial);
}
