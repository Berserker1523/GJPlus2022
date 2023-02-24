using UnityEngine;

namespace Kitchen
{
    public class PotionParticles : MonoBehaviour
    {
        [SerializeField] ParticleSystem successParticle;
        [SerializeField] ParticleSystem failureParticle;
        // Start is called before the first frame update
        private void Awake()
        {
            successParticle.Stop();
            failureParticle.Stop();
        }

        public void SuccesActivator() => successParticle.Play();
        public void FailureActivator() => failureParticle.Play();
    }

}
