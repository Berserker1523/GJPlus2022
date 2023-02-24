using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
public class PotionParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem failureParticle;
    // Start is called before the first frame update
    private void Awake()
    {
        EventManager.AddListener(PotionEvent.Poof, succesActivator);
        EventManager.AddListener(PotionEvent.FailedRecipe, failureActivator);
        successParticle.Stop();
        failureParticle.Stop();
    }
    void succesActivator() => successParticle.Play();
    void failureActivator() => failureParticle.Play();
    private void OnDestroy()
    {
        EventManager.RemoveListener(PotionEvent.FailedRecipe, failureActivator);
        EventManager.RemoveListener(PotionEvent.Poof, succesActivator); 
    }
}
