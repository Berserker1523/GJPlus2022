using System;
using UnityEditor;

namespace Kitchen
{
    [CustomEditor(typeof(DiseaseData))]
    public class DiseaseDataCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty disease = serializedObject.FindProperty(nameof(DiseaseData.disease));
            disease.enumValueIndex = EditorGUILayout.Popup("Disease", disease.enumValueIndex, Enum.GetNames(typeof( DiseaseName)));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(DiseaseData.sprite)));

            serializedObject.FindProperty(nameof(DiseaseData.name)).stringValue= Enum.GetName(typeof(DiseaseName),disease.enumValueIndex);

            serializedObject.ApplyModifiedProperties();
        }
    }
}