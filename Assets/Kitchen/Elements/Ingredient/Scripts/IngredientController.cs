using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    public class IngredientController : ClickHandlerBase
    {
        [SerializeField] private IngredientData ingredientData;

        public IngredientData IngredientData => ingredientData;

        public override void OnPointerClick(PointerEventData eventData)
        {
            SelectionManager.SelectedGameObject = this;
            if (ingredientData.ingredientName == IngredientName.Water)
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Coge Agua");
        }
    }
}
