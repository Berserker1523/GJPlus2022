using Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kitchen
{
    [RequireComponent(typeof(DragView))]
    public class IngredientController : MonoBehaviour
    {
        [SerializeField] private IngredientData ingredientData;

        private DragView dragView;

        public IngredientData IngredientData => ingredientData;

        private void Awake()
        {
            dragView = GetComponent<DragView>();
            dragView.OnDragBegan += HandleDragBegan;
        }

        private void OnDestroy()
        {
            dragView.OnDragBegan -= HandleDragBegan;
        } 

        private void HandleDragBegan(PointerEventData _)
        {
            EventManager.Dispatch(IngredientData.ingredientName);
        }
    }
}
