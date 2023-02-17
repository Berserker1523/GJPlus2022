using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

namespace Kitchen
{ 
    public class SoundsManager : MonoBehaviour
    {
        [Header("SFX Dictionary")]
        //Clients SFX Events
        string clientArriveEvent = "event:/SFX/Clientes/Llega Cliente";
        string clientDieEvent = "event:/SFX/Clientes/Muere Cliente";
        string clientServedEvent = "event:/SFX/Clientes/Atiende cliente";

        //Ingredients SFX Events
        string takesWaterEvent = "event:/SFX/Cocina/Coge Agua";

        //Cooking Tools SFX Events
        string foodReadyEvent = "event:/SFX/Cocina/Comida Lista";
        string foodBurntEvent = "event:/SFX/Cocina/Comida Quemada";
        string addIngredientEvent = "event:/SFX/Cocina/Infusión";
        string shakeIngredientsEvent = "";
        string shakerPoofEvent = "event:/SFX/Jugabilidad/Completa receta";
        string failedRecipeEvent = "";
        string throwTrashEvent = "event:/SFX/Cocina/Trash";

        //InGameEvents
        public static string backgroundMusic = "event:/Music/MusicaFondo";
        public static string hurryParameter = "";

        //Game Status SFX Events
        string wonEvent = "event:/SFX/Jugabilidad/Gana Nivel";
        string lostEvent = "event:/SFX/Jugabilidad/Pierde Nivel";

        //Main Menu Events
        string playButtonEvent = "";

        //History Book Events
        string unlockedButtonEvent = "";
        string lockedButtonEvent = "";
        
        //Note: Upgrades are deactivated in-game 
        string upgradeEvent = "event:/SFX/Jugabilidad/Upgrade";

        private void Awake()
        {
            //Clients SFX
            Events.EventManager.AddListener(ClientEvent.Arrived, PlayClientArriveSFX);
            Events.EventManager.AddListener(ClientEvent.Died, PlayClientDieSFX);
            Events.EventManager.AddListener(ClientEvent.Served, PlayClientServedSFX);

            //Ingredients SFX
            Events.EventManager.AddListener(IngredientName.Water, PlayWaterSFX);

            //Cooking Tools SFX
            Events.EventManager.AddListener(IngredientState.Cooked, PlayFoodReadySFX);
            Events.EventManager.AddListener(IngredientState.Burnt, PlayFoodBurntSFX);
            Events.EventManager.AddListener(PotionEvent.AddIngredient, PlayAddIngredientSFX);
            Events.EventManager.AddListener(PotionEvent.FailedRecipe, PlayFailedRecipeSFX);
            Events.EventManager.AddListener(TrashEvent.Throw, PlayThrowTrashSFX);

            //Game Status SFX
            Events.EventManager.AddListener(GameStatus.Won, PlayWonSFX);
            Events.EventManager.AddListener(GameStatus.Lost, PlayLostSFX);

            //Main Menu SFX
            Events.EventManager.AddListener(Events.GlobalEvent.Play, PlayplayButtonSFX);

            //History Book SFX
            Events.EventManager.AddListener(Events.GlobalEvent.Locked, PlayLockedButtonSFX);
            Events.EventManager.AddListener(Events.GlobalEvent.Unlocked, PlayUnLockedButtonSFX);
        }

        void PlaySFX(string sFX) =>
            RuntimeManager.PlayOneShot(sFX);

        //Clients SFX
        void PlayClientArriveSFX() => PlaySFX(clientArriveEvent);
        void PlayClientDieSFX() => PlaySFX(clientDieEvent);
        void PlayClientServedSFX() => PlaySFX(clientServedEvent);

        //Ingredients SFX
        void PlayWaterSFX() => PlaySFX(takesWaterEvent);

        //Cooking Tools SFX
        void PlayFoodReadySFX() => PlaySFX(foodReadyEvent);
        void PlayFoodBurntSFX() => PlaySFX(foodBurntEvent);
        void PlayAddIngredientSFX() => PlaySFX(addIngredientEvent);
        void PlayFailedRecipeSFX() => PlaySFX(failedRecipeEvent);
        void PlayThrowTrashSFX() => PlaySFX(throwTrashEvent);

        //Game Status SFX
        void PlayWonSFX() => PlaySFX(wonEvent);
        void PlayLostSFX() => PlaySFX(lostEvent);

        //Main Menu SFX
        void PlayplayButtonSFX() => PlaySFX(playButtonEvent);

        //History Book SFX
        void PlayLockedButtonSFX() =>PlaySFX(lockedButtonEvent);
        void PlayUnLockedButtonSFX() =>PlaySFX(unlockedButtonEvent);    
    }
}

