using UnityEngine;

namespace Kitchen
{
    public static class SelectionManager
    {
        private static ClickHandlerBase selectedGameObject;
        public static ClickHandlerBase SelectedGameObject
        {
            get => selectedGameObject;
            set
            {
                selectedGameObject?.SetImageColor(Color.white);
                selectedGameObject = value;
                selectedGameObject?.SetImageColor(Color.yellow);
            }
        }
    }
}
