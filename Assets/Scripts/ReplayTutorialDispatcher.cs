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
        button.onClick.AddListener(OnSceneLoaded);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnSceneLoaded);       
    }

    void OnSceneLoaded() => SceneManager.sceneLoaded += DispatchReplay;

    private void DispatchReplay(Scene arg0, LoadSceneMode arg1) => EventManager.Dispatch(GlobalTutorialEvent.replayingTutorial);
}
