using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class GoalsHUD : MonoBehaviour
    {

        private LevelInstantiator levelInstantiator;
        [SerializeField] private TextMeshProUGUI goalText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private Image[] stars = new Image[3];
        [SerializeField] private float currentTime;

        private void Awake()
        {
            levelInstantiator= FindObjectOfType<LevelInstantiator>();
        }

        private void Start()
        {
            goalText.text = levelInstantiator.LevelData.goal.ToString();
            currentTime = levelInstantiator.LevelData.time;
            StartCoroutine(LevelTimer());

            foreach (var star in stars)
                star.color = Color.black;

        }

        private IEnumerator LevelTimer()
        {
            speedText.text = currentTime.ToString();

            WaitForSeconds wait = new WaitForSeconds(1f);
            while (currentTime > 0)
            {
                currentTime--;
                speedText.text = string.Format("{0:0}:{1:00}", (currentTime / 60) % 60, currentTime % 60);
                yield return wait;
            }
            yield return null;
        }
    }


}
