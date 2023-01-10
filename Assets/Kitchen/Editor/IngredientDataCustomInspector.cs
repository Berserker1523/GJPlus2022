using UnityEditor;

namespace Kitchen
{
    [CustomEditor(typeof(IngredientData))]
    public class IngredientDataCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.ingredientName)));

            SerializedProperty necessaryCookingTool = serializedObject.FindProperty(nameof(IngredientData.necessaryCookingTool));
            EditorGUILayout.PropertyField(necessaryCookingTool);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.rawSprite)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.dishSprite)));

            if((necessaryCookingTool.enumValueFlag & (int)CookingToolName.Stove) != 0)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.stoveRawSprite)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.stoveCookedSprite)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.stoveBurntSprite)));
            }

            if((necessaryCookingTool.enumValueFlag & (int)CookingToolName.Mortar) != 0)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.mortarRawSprite)));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(IngredientData.mortarCrushedSprite)));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
