using Events;
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
        [SerializeField] private int currentTime;
        [SerializeField] private int currentGoal;

        private void Awake()
        {
            levelInstantiator= FindObjectOfType<LevelInstantiator>();
        }

        private void Start()
        {
            EventManager.AddListener(ClientEvent.Served, HandleClientServed);
            currentGoal = levelInstantiator.LevelData.goal;
            SetGoal();

            currentTime = levelInstantiator.LevelData.time;
            StartCoroutine(LevelTimer());

            foreach (var star in stars)
                star.color = Color.black;
        }

        private void SetGoal()=>
            goalText.text = currentGoal.ToString();

        private IEnumerator LevelTimer()
        {
            speedText.text = currentTime.ToString();

            WaitForSeconds wait = new WaitForSeconds(1f);
            while (currentTime > 0)
            {
                currentTime--;
                speedText.text = string.Format("{0:0}:{1:00}", (currentTime / 60) % 60, currentTime % 60);


                if (currentTime < 10)
                    speedText.color = Color.red;
                else if (currentTime < 30)
                    speedText.color = Color.yellow;

                yield return wait;

            }
            yield return null;
        }

        private void HandleClientServed()
        {
            currentGoal--;
            SetGoal();

            if (currentGoal <= 0)
            {
                stars[0].color = Color.white;

                if (currentTime > 0)
                    stars[1].color = Color.white;
            }
        }
    }


}
