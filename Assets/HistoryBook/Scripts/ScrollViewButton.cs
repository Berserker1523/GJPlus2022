using UnityEngine;
using UnityEngine.UI;

namespace HistoryBook
{
    public class ScrollViewButton : MonoBehaviour
    {
        [SerializeField] Scrollbar scrollViewSlider;
        Button button;
        [SerializeField] float value;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(MoveScrollView);
        }

        private void MoveScrollView() => scrollViewSlider.value = value;
    }
}