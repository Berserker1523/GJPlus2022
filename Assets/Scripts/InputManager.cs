using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    public class InputManager : MonoBehaviour
    {
        private Physics2DRaycaster cameraPhysicsRaycaster;

        private bool isInGameplay;
        public bool IsInGameplay
        {
            get => isInGameplay;
            set
            {
                isInGameplay = value;
                cameraPhysicsRaycaster.enabled = isInGameplay;
            }
        }

        private void Awake() =>
            cameraPhysicsRaycaster = Camera.main.GetComponent<Physics2DRaycaster>();
    }
}