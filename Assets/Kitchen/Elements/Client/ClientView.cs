using UnityEngine;
using System.Collections.Generic;
using Events;

namespace Kitchen
{
    public enum ClientEvent
    {
        Served,
        Died
    }

    public class ClientView : ButtonHandler
    {
        private List<IngredientName> requiredRecipe = new();
        private float waitingTime;

        protected override void OnClick()
        {
            PotionView potionView = SelectionManager.selectedGameObject as PotionView;
            SelectionManager.selectedGameObject = null;

            if (potionView == null)
                return;

            bool acceptedRecipe = true;
            foreach(IngredientName ingredient in requiredRecipe)
            {
                if(!potionView.ingredients.Contains(ingredient))
                {
                    acceptedRecipe = false;
                    break;
                }
            }

            if (!acceptedRecipe)
                return;

            potionView.Clear();
            EventManager.Dispatch(ClientEvent.Served, waitingTime);
            Destroy(gameObject);
        }

        private void Update()
        {
            waitingTime -= Time.deltaTime;
            if (waitingTime <= 0)
            {
                EventManager.Dispatch(ClientEvent.Died);
                Destroy(gameObject);
            }
        }
    }
}
