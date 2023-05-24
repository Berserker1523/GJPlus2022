using Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kitchen.Tutorial
{
    public class TryAgainComponent : MonoBehaviour
    {
        [SerializeField] Button _okButton;
        [SerializeField] public LevelEvents _levelEvent;

        // Start is called before the first frame update
       protected void Awake()
       {
            EventManager.AddListener(_levelEvent,ShowPopup);
            _okButton.onClick.AddListener(RestartLevel);
       }

        protected void ShowPopup() =>  gameObject.SetActive(true);

        private void OnDestroy()
        {
            EventManager.RemoveListener(_levelEvent, ShowPopup);
            
        }
        protected void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}