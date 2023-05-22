using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private SceneName sceneName;
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeScene);
    }

    private void OnDestroy() =>
        button.onClick.RemoveListener(ChangeScene);

    protected virtual void ChangeScene() => SceneManager.LoadScene(sceneName.ToString(), loadSceneMode);
}
