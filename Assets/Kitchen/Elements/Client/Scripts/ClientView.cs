using UnityEngine;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace Kitchen
{
    public class ClientView : ButtonHandler
    {
        [SerializeField] private TextMeshProUGUI IngredientsLabel;
        [SerializeField] private Slider slider;

        public int ID { get; set; }

        private List<IngredientName> requiredRecipe = new();
        public float WaitingTime { get; set; }
        private float currentlyWaitingTime;

        protected override void Awake()
        {
            base.Awake();
            WaitingTime = 70f;
            currentlyWaitingTime = WaitingTime;
            slider.value = currentlyWaitingTime;

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Llega Cliente");
        }

        private void Update()
        {
            currentlyWaitingTime -= Time.deltaTime;
            slider.value = currentlyWaitingTime / WaitingTime;
            if (currentlyWaitingTime <= 0)
            {
                EventManager.Dispatch(SpawnPointEvent.Released, ID);
                EventManager.Dispatch(ClientEvent.Died);
                Destroy(gameObject);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Muere Cliente");
            }
        }

        public void Initialize(int ID, List<IngredientName> recipe)
        {
            this.ID = ID;
            requiredRecipe = new List<IngredientName>(recipe);
            foreach (IngredientName ingredient in requiredRecipe)
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
                Destroy(painkillerView.transform.parent.gameObject);
                return;
            }

            if (potionView == null)
                return;

            bool acceptedRecipe = requiredRecipe.OrderBy(x => x).SequenceEqual(potionView.Ingredients.OrderBy(x => x));

            if (!acceptedRecipe)
                return;

            potionView.Clear();
            AddMoney();
            EventManager.Dispatch(SpawnPointEvent.Released, ID);
            Destroy(gameObject);

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Atiende cliente");
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(WaitingTime * currentlyWaitingTime / WaitingTime) + 10;
    }
}
