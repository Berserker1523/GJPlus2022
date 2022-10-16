using UnityEngine;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine.UI;

namespace Kitchen
{
    public class ClientView : ButtonHandler
    {
        [SerializeField] private TextMeshProUGUI IngredientsLabel;
        [SerializeField] private Slider slider;

        private List<IngredientName> requiredRecipe = new();
        public float WaitingTime { get; set; }
        private float currentlyWaitingTime;

        protected override void Awake()
        {
            base.Awake();
            WaitingTime = 30f;
            currentlyWaitingTime = WaitingTime;
            slider.value = currentlyWaitingTime;
        }

        private void Update()
        {
            currentlyWaitingTime -= Time.deltaTime;
            slider.value = currentlyWaitingTime / WaitingTime;
            if (currentlyWaitingTime <= 0)
            {
                EventManager.Dispatch(ClientEvent.Died);
                Destroy(gameObject);
            }
        }

        public void SetRequiredRecipe(List<IngredientName> recipe)
        {
            requiredRecipe = new List<IngredientName>(recipe);
            foreach(IngredientName ingredient in requiredRecipe)
                IngredientsLabel.text += $"{ingredient}\n";
        }

        protected override void OnClick()
        {
            PotionView potionView = SelectionManager.selectedGameObject as PotionView;
            PainkillerView painkillerView = SelectionManager.selectedGameObject as PainkillerView;
            SelectionManager.selectedGameObject = null;

            if (painkillerView != null)
            {
                currentlyWaitingTime = WaitingTime;
                Destroy(painkillerView.gameObject);
                return;
            }

            if (potionView == null)
                return;

            bool acceptedRecipe = true;
            foreach (IngredientName ingredient in requiredRecipe)
            {
                if (!potionView.ingredients.Contains(ingredient))
                {
                    acceptedRecipe = false;
                    break;
                }
            }

            if (!acceptedRecipe)
                return;

            potionView.Clear();
            EventManager.Dispatch(ClientEvent.Served, currentlyWaitingTime);
            Destroy(gameObject);
        }
    }
}
