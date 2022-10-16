using Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Kitchen
{
    public class ClientView : ButtonHandler
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderBarImage;
        [SerializeField] private Image potionImage;

        public int ID { get; set; }

        private List<IngredientName> requiredRecipe = new();
        public float WaitingTime { get; set; }
        private float currentlyWaitingTime;

        protected override void Awake()
        {
            base.Awake();
            WaitingTime = 70f;
            currentlyWaitingTime = WaitingTime;
            slider.value = 1;

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Llega Cliente");
        }

        private void Update()
        {
            currentlyWaitingTime -= Time.deltaTime;
            slider.value = currentlyWaitingTime / WaitingTime;

            if (currentlyWaitingTime <= WaitingTime * 0.3)
                sliderBarImage.color = Color.red;
            else if(currentlyWaitingTime <= WaitingTime * 0.6)
                sliderBarImage.color = Color.yellow;
            else if (currentlyWaitingTime > WaitingTime * 0.6)
                sliderBarImage.color = Color.white;

            if (currentlyWaitingTime <= 0)
            {
                EventManager.Dispatch(SpawnPointEvent.Released, ID);
                EventManager.Dispatch(ClientEvent.Died);
                Destroy(transform.parent.gameObject);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Muere Cliente");
            }
        }

        public void Initialize(int ID, List<IngredientName> recipe, Sprite potionSkin)
        {
            this.ID = ID;
            potionImage.sprite = potionSkin;
            requiredRecipe = new List<IngredientName>(recipe);
        }

        protected override void OnClick()
        {
            PotionView potionView = SelectionManager.SelectedGameObject as PotionView;
            PainkillerView painkillerView = SelectionManager.SelectedGameObject as PainkillerView;
            SelectionManager.SelectedGameObject = null;

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
            Destroy(transform.parent.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Atiende cliente");
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(WaitingTime * currentlyWaitingTime / WaitingTime) + 10;
    }
}
