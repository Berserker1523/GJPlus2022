using UnityEngine;

namespace Kitchen
{
    [CreateAssetMenu(fileName = "CookingTool", menuName = "ScriptableObjects/CookingTool", order = 2)]
    public class CookingToolData : ScriptableObject
    {
        public CookingToolName cookingToolName;
        public float cookingSeconds;
        public float burningSeconds;
    }
}
