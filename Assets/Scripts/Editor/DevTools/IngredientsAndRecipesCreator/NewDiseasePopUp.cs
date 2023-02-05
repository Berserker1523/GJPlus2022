using DevTools;
using DevTools.PopUps;
using Kitchen;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class NewDiseasePopUp : AbstractNewAssetPopUP<DiseaseName>
{
    protected override string enumFolder => Path.Combine("Kitchen", "Elements", "Disease", "Scripts");

    protected override string enumFile => "DiseaseName.cs";

    static SerializedProperty m_DiseaseName = null;
    static SerializedProperty m_sprite = null;

    [SerializeField] Sprite diseaseSprite;


    private new void OnEnable()
    {
        base.OnEnable();
        m_DiseaseName = so.FindProperty("name");
        m_sprite = so.FindProperty("diseaseSprite");
    }

    protected override void DisplayProperties()
    {
        EditorGUILayout.PropertyField(m_DiseaseName, new GUIContent("New Disease Name"));
        EditorGUILayout.PropertyField(m_sprite, new GUIContent("Sprite"));

        if (GUILayout.Button("Create new Disease", GUILayout.Height(30f)))
        {
            if (String.IsNullOrEmpty(m_DiseaseName.stringValue))
                warningLabel = "Recipe Name cannot be empty";
            else if (Array.Exists<string>(enumValues, element => element == m_DiseaseName.stringValue))
                warningLabel = "Recipe Name already exists";
            else if (m_sprite.objectReferenceValue == null)
                warningLabel = "Must Set the Disease Sprite";
            else
            {
                AddToEnum();
                warningLabel = "";
                Debug.Log("Disease " + m_DiseaseName.stringValue + " created");
            }
        }
    }

    [DidReloadScripts]
    protected static void InstantiateScriptableObject()
    {
        if (typeof(DiseaseData) != null && m_DiseaseName != null && !String.IsNullOrEmpty(m_DiseaseName.stringValue))
        {
            DiseaseData SO = (DiseaseData)ScriptableObject.CreateInstance(typeof(DiseaseData));
            SO.disease = (DiseaseName)Enum.Parse(typeof(DiseaseName), m_DiseaseName.stringValue);
            SO.name = m_DiseaseName.stringValue;
            SO.sprite = (Sprite)m_sprite.objectReferenceValue;

            string scriptSOFolder = IngredientsNRecipesCreatorWindow.DiseasesSOPath;
            string scriptSOFile = m_DiseaseName.stringValue;

            AssetDatabase.CreateAsset(SO, Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));
            AssetDatabase.ImportAsset(Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));

            so.Update();
            m_DiseaseName.stringValue = null;
            m_sprite.objectReferenceValue = null;
            so.ApplyModifiedProperties();
        }
    }

    protected override void AddToEnum()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("namespace Kitchen");
        sb.AppendLine("{");
        sb.AppendLine("    [System.Flags]");
        sb.AppendLine("    public enum DiseaseName");
        sb.AppendLine("    {");
        for(int i=0; i<enumValues.Length; i++)
            sb.AppendLine("        " + enumValues[i] + " = (1 << "+ i + "), ");
        sb.AppendLine("        " + m_DiseaseName.stringValue + " = (1 << " + (enumValues.Length+1) + ")");
        sb.AppendLine("    }");
        sb.AppendLine("}");

        string filePath = Path.Combine(enumFolder, enumFile);
        CheckAndCreate(enumFolder, filePath, sb);
    }     
}
