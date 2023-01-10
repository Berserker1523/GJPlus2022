using UnityEngine.EventSystems;

namespace Kitchen
{
    public class PainkillerController : ClickHandlerBase
    {
        public override void OnPointerClick(PointerEventData eventData) =>
            SelectionManager.SelectedGameObject = this;
    }
}
