using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kitchen
{
    public class Timer : MonoBehaviour
    {
        SpriteRenderer radiusFill;
        [SerializeField] public SpriteRenderer background;

        Material timerShader;
        // Start is called before the first frame update
        void Start()
        {
            radiusFill = GetComponent<SpriteRenderer>();
            timerShader= radiusFill.material;
            SwitchTimerVisibility(false);
        }

        public void StartTimer(float seconds) => StartCoroutine(StartTimerCoroutine(seconds));

        public void StartRedTimer(float seconds)
        {
            radiusFill.color = Color.red;
            timerShader.SetFloat("_Arc1", 0f);
            StartTimerCoroutine(seconds);
        }

       IEnumerator StartTimerCoroutine(float seconds)
       {
            SwitchTimerVisibility(true);

            float FillAmountPerInterval = (360* 0.1f)/seconds;
            while (timerShader.GetFloat("_Arc1") <= 360)
            {
               timerShader.SetFloat("_Arc1", timerShader.GetFloat("_Arc1") + FillAmountPerInterval);
               yield return new WaitForSeconds(0.1f);
            }
            SwitchTimerVisibility(false);
       }

        void SwitchTimerVisibility(bool display)
        {
            radiusFill.enabled = display;
            background.enabled = display;
        }
    }
}
