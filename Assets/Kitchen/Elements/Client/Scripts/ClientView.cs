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
        [SerializeField] private GameObject treePrefab;

        public int ID { get; set; }

        private RecipeData requiredRecipe;
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
            else if (currentlyWaitingTime <= WaitingTime * 0.6)
                sliderBarImage.color = Color.yellow;
            else if (currentlyWaitingTime > WaitingTime * 0.6)
                sliderBarImage.color = Color.white;

            if (currentlyWaitingTime <= 0)
            {
                Instantiate(treePrefab, transform.parent.parent);
                Destroy(transform.parent.gameObject);
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Muere Cliente");
                EventManager.Dispatch(ClientEvent.Died);
            }
        }

        public void Initialize(int ID, RecipeData recipe, Sprite potionSkin)
        {
            this.ID = ID;
            potionImage.sprite = potionSkin;
            requiredRecipe = recipe;
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

            bool acceptedRecipe = potionView.currentRecipe == requiredRecipe;

            if (!acceptedRecipe)
                return;

            potionView.Clear();
            AddMoney();
            EventManager.Dispatch(SpawnPointEvent.Released, ID);
            Destroy(transform.parent.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Clientes/Atiende cliente");
            EventManager.Dispatch(ClientEvent.Served);
        }

        private void AddMoney() =>
            MoneyManager.Money += (int)(WaitingTime * currentlyWaitingTime / WaitingTime) + 10;
    }
}
