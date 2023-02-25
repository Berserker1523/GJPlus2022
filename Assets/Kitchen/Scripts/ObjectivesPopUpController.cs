using TMPro;
using UnityEngine;

namespace Kitchen
{
    public class ObjectivesPopUpController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI clientGoalsText;
        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] private TextMeshProUGUI timeText;

        private LevelInstantiator levelInstantiator;

        private void Awake()
        {
            levelInstantiator = FindObjectOfType<LevelInstantiator>();
            clientGoalsText.text = $"{levelInstantiator.LevelData.goal}";
            streakText.text = $"{levelInstantiator.LevelData.streak}";
            timeText.text = string.Format("{0:0}:{1:00}", levelInstantiator.LevelData.time / 60 % 60, levelInstantiator.LevelData.time % 60);
        }
    }
}
