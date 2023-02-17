using Events;
using System.Collections;
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
        [SerializeField] private bool hurryDispatched;

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

            for (int i = 0; i < stars.Length; i++)
            {
                if (!levelInstantiator.LevelData.stars[i])
                    stars[i].color = Color.black;
            }
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
                else if (currentTime < 30  && !hurryDispatched)
                {
                    speedText.color = Color.yellow;
                    EventManager.Dispatch(LevelEvents.Hurry);
                    hurryDispatched = true;
                }

                yield return wait;

            }
            yield return null;
        }

        private void HandleClientServed()
        {
            currentGoal--;
            SetGoal();

            //Goal Star
            if (currentGoal <= 0)
            {
                stars[0].color = Color.white;
                EventManager.Dispatch(LevelEvents.Goal);

                //Time Star
                if (currentTime > 0)
                {
                    stars[1].color = Color.white;
                    EventManager.Dispatch(LevelEvents.Speed);
                }
            }
        }
    }


}
