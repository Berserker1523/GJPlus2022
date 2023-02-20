using Events;
using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

namespace Kitchen
{
    public class BackgroundMusic : MonoBehaviour
    {
        public PARAMETER_ID leveltimerParameterId;
        public EventInstance instance;

        [EventRef, SerializeField] public string fmodEvent;
        [SerializeField] public string fmodParameterName;
        [Range(0.1f, 1f)] public float levelTimerTransitionVelocity = 0.2f;
        public float levelTimertransitionIncrease;

        private void Awake()
        {
            fmodEvent = SoundsManager.backgroundMusic;
            fmodParameterName = SoundsManager.hurryParameter;
        }

        void Start()
        {
            instance = RuntimeManager.CreateInstance(fmodEvent);
            EventDescription pitchEventDescription;
            instance.getDescription(out pitchEventDescription);
            PARAMETER_DESCRIPTION pitchParameterDescription;
            pitchEventDescription.getParameterDescriptionByName(fmodParameterName, out pitchParameterDescription);
            leveltimerParameterId = pitchParameterDescription.id;

            instance.start();

            //InGame Events SFX
            Events.EventManager.AddListener(LevelEvents.Hurry, PlayLevelTimerParameter);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(LevelEvents.Hurry, PlayLevelTimerParameter);
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        void PlayLevelTimerParameter()
        {
            EventManager.RemoveListener(LevelEvents.Hurry, PlayLevelTimerParameter);
            instance.setParameterByName("Hurry", 1);
            //StartCoroutine(CallLevelTimerParameter());
        }

        IEnumerator CallLevelTimerParameter()
        {
            while (levelTimertransitionIncrease < 1)
            {
                instance.setParameterByID(leveltimerParameterId, levelTimertransitionIncrease);
                yield return new WaitForSeconds(levelTimerTransitionVelocity * 10f);
                levelTimertransitionIncrease += levelTimerTransitionVelocity;
            }
        }
    }
}
