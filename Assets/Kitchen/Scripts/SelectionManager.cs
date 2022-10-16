using UnityEngine;

namespace Kitchen
{
    public static class SelectionManager
    {
        private static ButtonHandler selectedGameObject;
        public static ButtonHandler SelectedGameObject
        {
            get { return selectedGameObject; }
            set
            {
                selectedGameObject?.SetButtonImageColor(Color.white);
                selectedGameObject = value;
                selectedGameObject?.SetButtonImageColor(Color.yellow);
            }
        }
    }
}
