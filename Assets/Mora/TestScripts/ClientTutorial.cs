using Events;
using Kitchen;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace tutorial
{
    [RequireComponent(typeof(Collider2D))]
    public class ClientTutorial : MonoBehaviour, IDropHandler
    {
        private bool clientServed = false;
        [SerializeField] private RecipeData requiredRecipe;

        //Animation Params
        [SerializeField] Animator animator;
        private string animatorIllnessParameter = "Illness";
        private string animatorRecuperatedParameter = "Recuperated";
        private readonly string animatorArriveParameter = "Arrive";
        private ParticleSystem healingVFX;


        public void OnDrop(PointerEventData pointerEventData)
        {
            pointerEventData.pointerDrag.TryGetComponent(out PotionResultController potionController);
            pointerEventData.pointerDrag.TryGetComponent(out PainkillerController painkillerController);


            if (potionController == null)
                return;

            bool acceptedRecipe = potionController.CurrentRecipe == requiredRecipe;

            if (!acceptedRecipe)
                return;

            for (int i = 0; i < potionController.CurrentRecipe.ingredients.Length; i++)
            {
                for (int j = 0; j < Enum.GetValues(typeof(IngredientName)).Length; j++)
                {
                    if ((int)potionController.CurrentRecipe.ingredients[i].ingredient.ingredientName == j)
                        GlobalCounter.attendedClients[j]++;
                }
            }

            potionController.Release();
            StartCoroutine(ClientServedRoutine());
        }

        protected void Awake()
        {
            animator.SetInteger(animatorIllnessParameter, (int)requiredRecipe.diseasesItCures);
            animator.SetTrigger(animatorArriveParameter);
            healingVFX = GetComponentInChildren<ParticleSystem>();
        }

        public IEnumerator ClientServedRoutine()
        {
            clientServed = true;
            animator.SetBool(animatorRecuperatedParameter, clientServed);
            EventManager.Dispatch(ClientEvent.Served);
            healingVFX.Play();
            yield return new WaitForSeconds(3f);
        }
    }

}
