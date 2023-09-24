using Events;
using UnityEngine;

namespace Kitchen
{
    public class RandomWindGenerator : MonoBehaviour
    {
        [Header("Mortar relateds")]
        [SerializeField] GameObject windPrefab;

        [Header("Counter Timer")]
        private float timeInterval = 5f;
        private float elapsedTime = 0.0f;

        [Header("Global Stopper")]
        private bool inTutorial = false;
        private void Awake()
        {
            EventManager.AddListener<bool>(GlobalTutorialEvent.inTutorial, SwitchInTutorialVariable);
        }
        private void OnDestroy()
        {
            EventManager.RemoveListener<bool>(GlobalTutorialEvent.inTutorial, SwitchInTutorialVariable);      
        }
        void SwitchInTutorialVariable(bool tutorial) => inTutorial = tutorial;
        void Update()
        {
            if (inTutorial)
                return;

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= timeInterval)
            {
                BackgroundEvent direction = Random.Range(0, 2) switch
                {
                    0 => BackgroundEvent.windRight,
                    1 => BackgroundEvent.windLeft,
                    _ => BackgroundEvent.windRight
                };
                EventManager.Dispatch(direction);
                elapsedTime = 0.0f;
            }
        }
    }
}