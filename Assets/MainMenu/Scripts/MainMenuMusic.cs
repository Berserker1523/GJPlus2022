using Events;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    private void Awake() => EventManager.AddListener(GlobalEvent.Play, DestroyObject);

    private void DestroyObject()  => Destroy(gameObject);

    private void OnDestroy() => EventManager.RemoveListener(GlobalEvent.Play, DestroyObject);
}
