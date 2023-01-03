using UnityEditor;

namespace Kitchen
{
    [CustomEditor(typeof(IngredientData))]
    public class IngredientDataCustomInspector : Editor
    {
        SerializedProperty ingredientName;
        SerializedProperty cookingTool;

        SerializedProperty rawState;
        SerializedProperty cookedState;
        SerializedProperty burntState;
        SerializedProperty entireState;
        SerializedProperty crushedState;
        private void OnEnable()
        {
            ingredientName = serializedObject.FindProperty("ingredientName");
            cookingTool = serializedObject.FindProperty("necessaryCookingTool");
            rawState = serializedObject.FindProperty("rawState");
            cookedState = serializedObject.FindProperty("cookedState");
            burntState = serializedObject.FindProperty("burntState");
            entireState = serializedObject.FindProperty("entireState");
            crushedState = serializedObject.FindProperty("crushedState");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(ingredientName);
            EditorGUILayout.PropertyField(cookingTool);

            if((cookingTool.enumValueFlag & (int)CookingToolName.Stove) != 0)
            {
                EditorGUILayout.PropertyField(rawState);
                EditorGUILayout.PropertyField(cookedState);
                EditorGUILayout.PropertyField(burntState);
            }

            if((cookingTool.enumValueFlag & (int)CookingToolName.Mortar) != 0)
            {
                EditorGUILayout.PropertyField(entireState);
                EditorGUILayout.PropertyField(crushedState);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
