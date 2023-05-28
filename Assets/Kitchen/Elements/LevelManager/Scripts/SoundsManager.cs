using FMODUnity;
using UnityEngine;

namespace Kitchen
{
    public class SoundsManager : MonoBehaviour
    {
        [Header("SFX Dictionary")]
        //Clients SFX Events
        string clientArriveEvent = "event:/SFX/Clientes/Llega Cliente";
        string clientDieEvent = "event:/SFX/Clientes/Muere Cliente";
        string clientServedEvent = "event:/SFX/Clientes/Atiende cliente";
        string clientMaleGruntEvent = "event:/SFX/Clientes/Grunt Male"; //TODO client waiting bar is red
        string clientFemaleGruntEvent = "event:/SFX/Clientes/Grunt Female"; //TODO

        //Ingredients SFX Events
        string takesWaterEvent = "event:/SFX/Cocina/Coge Agua";
        string takesIngredientEvent = "event:/SFX/Cocina/Coge Ingrediente";

        //Cooking Tools SFX Events
        string foodReadyEvent = "event:/SFX/Cocina/Comida Lista";
        string burningFood = "event:/SFX/Cocina/Comida quemándose";
        string foodBurntEvent = "event:/SFX/Cocina/Comida Quemada";
        string addIngredientEvent = "event:/SFX/Cocina/Infusión";
        string addWaterEvent = "event:/SFX/Cocina/Sirve agua";
        string shakeIngredientsEvent = "event:/SFX/Cocina/Shaker";
        string shakerPoofEvent = "event:/SFX/Cocina/Completa receta";
        string failedRecipeEvent = "event:/SFX/Cocina/Receta incorrecta";
        string hoverEvent = "event:/SFX/Cocina/Snap Object";
        string throwTrashEvent = "event:/SFX/Cocina/Trash";

        //InGameEvents
        public static string backgroundMusic = "event:/Music/MusicaFondo";
        public static string hurryParameter = "Hurry";

        public static string endlevelStarsMusic = "event:/SFX/UI/Stars";
        public static string endLevelStarsParameter = "Stars Count";

        //Game Status SFX Events
        string wonEvent = "event:/SFX/Jugabilidad/Gana Nivel";
        string lostEvent = "event:/SFX/Jugabilidad/Pierde Nivel";

        //Main Menu Events
        string playButtonEvent = "event:/SFX/UI/Play Button";

        //History Book Events
        string unlockedButtonEvent = "event:/SFX/UI/Flip page";
        string lockedButtonEvent = "event:/SFX/UI/Blocked button";
        
        //Note: Upgrades are deactivated in-game 
        string upgradeEvent = "event:/SFX/Jugabilidad/Upgrade";
        
        //UI Events 
        string buttonSound = "event:/SFX/UI/Click button";
        string openMenu = "event:/SFX/UI/Pop Up";
        private void Awake()
        {
            //Clients SFX
            Events.EventManager.AddListener(ClientEvent.Arrived, PlayClientArriveSFX);
            Events.EventManager.AddListener(ClientEvent.Dying, PlayClientDieSFX);
            Events.EventManager.AddListener(ClientEvent.Served, PlayClientServedSFX);
            Events.EventManager.AddListener<Gender>(ClientEvent.Grunt, PlayClientGruntSFX);

            //Ingredients SFX
            Events.EventManager.AddListener(IngredientName.Water, PlayWaterSFX);
            Events.EventManager.AddListener(IngredientName.Carnauba, PlayIngredientSFX);
            Events.EventManager.AddListener(IngredientName.Pequi, PlayIngredientSFX);

            //Cooking Tools SFX
            Events.EventManager.AddListener(IngredientState.Cooked, PlayFoodReadySFX);
            Events.EventManager.AddListener(IngredientState.Burnt, PlayFoodBurntSFX);
            Events.EventManager.AddListener(PotionEvent.AddIngredient, PlayAddIngredientSFX);
            Events.EventManager.AddListener(PotionEvent.AddWater, PlayAddWater);
            Events.EventManager.AddListener(PotionEvent.FailedRecipe, PlayFailedRecipeSFX);
            Events.EventManager.AddListener(ObjectInteractionEvents.Hover, PlayHoverSFX);
            Events.EventManager.AddListener(TrashEvent.Throw, PlayThrowTrashSFX);

            Events.EventManager.AddListener(PotionEvent.Poof, PlayPoofSFX);

            //Game Status SFX
            Events.EventManager.AddListener(GameStatus.Won, PlayWonSFX);
            Events.EventManager.AddListener(GameStatus.Lost, PlayLostSFX);

            //Main Menu SFX
            Events.EventManager.AddListener(Events.GlobalEvent.Play, PlayplayButtonSFX);

            //History Book SFX
            Events.EventManager.AddListener(Events.GlobalEvent.Locked, PlayLockedButtonSFX);
            Events.EventManager.AddListener(Events.GlobalEvent.Unlocked, PlayUnLockedButtonSFX);
            
            //TODO: General UI SFX
        }

        private void OnDestroy()
        {
            //Clients SFX
            Events.EventManager.RemoveListener(ClientEvent.Arrived, PlayClientArriveSFX);
            Events.EventManager.RemoveListener(ClientEvent.Dying, PlayClientDieSFX);
            Events.EventManager.RemoveListener(ClientEvent.Served, PlayClientServedSFX);
            Events.EventManager.RemoveListener<Gender>(ClientEvent.Grunt, PlayClientGruntSFX);

            //Ingredients SFX
            Events.EventManager.RemoveListener(IngredientName.Water, PlayWaterSFX);
            Events.EventManager.RemoveListener(IngredientName.Carnauba, PlayIngredientSFX);
            Events.EventManager.RemoveListener(IngredientName.Pequi, PlayIngredientSFX);

            //Cooking Tools SFX
            Events.EventManager.RemoveListener(IngredientState.Cooked, PlayFoodReadySFX);
            Events.EventManager.RemoveListener(IngredientState.Burnt, PlayFoodBurntSFX);
            Events.EventManager.RemoveListener(PotionEvent.AddIngredient, PlayAddIngredientSFX);
            Events.EventManager.RemoveListener(PotionEvent.AddWater, PlayAddWater);
            Events.EventManager.RemoveListener(PotionEvent.FailedRecipe, PlayFailedRecipeSFX);
            Events.EventManager.RemoveListener(ObjectInteractionEvents.Hover, PlayHoverSFX);
            Events.EventManager.RemoveListener(TrashEvent.Throw, PlayThrowTrashSFX);

            Events.EventManager.RemoveListener(PotionEvent.Poof, PlayPoofSFX);

            //Game Status SFX
            Events.EventManager.RemoveListener(GameStatus.Won, PlayWonSFX);
            Events.EventManager.RemoveListener(GameStatus.Lost, PlayLostSFX);

            //Main Menu SFX
            Events.EventManager.RemoveListener(Events.GlobalEvent.Play, PlayplayButtonSFX);

            //History Book SFX
            Events.EventManager.RemoveListener(Events.GlobalEvent.Locked, PlayLockedButtonSFX);
            Events.EventManager.RemoveListener(Events.GlobalEvent.Unlocked, PlayUnLockedButtonSFX);

            //TODO: General UI SFX
        }

        void PlaySFX(string sFX) =>
            RuntimeManager.PlayOneShot(sFX);

        //Clients SFX
        void PlayClientArriveSFX() => PlaySFX(clientArriveEvent);
        void PlayClientDieSFX() => PlaySFX(clientDieEvent);
        void PlayClientServedSFX() => PlaySFX(clientServedEvent);
        void PlayClientGruntSFX(Gender gender) =>
            PlaySFX(gender == Gender.female ? clientFemaleGruntEvent : clientMaleGruntEvent);

        //Ingredients SFX
        void PlayWaterSFX() => PlaySFX(takesWaterEvent);
        void PlayIngredientSFX() => PlaySFX(takesIngredientEvent);

        //Cooking Tools SFX
        void PlayFoodReadySFX() => PlaySFX(foodReadyEvent);
        void PlayFoodBurntSFX() => PlaySFX(foodBurntEvent);
        void PlayAddIngredientSFX() => PlaySFX(addIngredientEvent);
        void PlayAddWater() => PlaySFX(addWaterEvent);
        void PlayFailedRecipeSFX() => PlaySFX(failedRecipeEvent);
        void PlayHoverSFX() => PlaySFX(hoverEvent);
        void PlayThrowTrashSFX() => PlaySFX(throwTrashEvent);

        void PlayPoofSFX() => PlaySFX(shakerPoofEvent);

        //Game Status SFX
        void PlayWonSFX() => PlaySFX(wonEvent);
        void PlayLostSFX() => PlaySFX(lostEvent);

        //Main Menu SFX
        void PlayplayButtonSFX() => PlaySFX(playButtonEvent);

        //History Book SFX
        void PlayLockedButtonSFX() =>PlaySFX(lockedButtonEvent);
        void PlayUnLockedButtonSFX() =>PlaySFX(unlockedButtonEvent);
        
        //General UI SFX
        void PlayClickSFX() => PlaySFX(buttonSound);
        void MenuPopUpSFX() => PlaySFX(openMenu);
    }
}
