using UnityEngine.EventSystems;

namespace Kitchen
{
    public class TrashController : ClickHandlerBase
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            IReleaseable releaseable = SelectionManager.SelectedGameObject as IReleaseable;
            SelectionManager.SelectedGameObject = null;

            if (releaseable == null)
                return;

            releaseable.Release();
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cocina/Trash");
        }
    }
}
