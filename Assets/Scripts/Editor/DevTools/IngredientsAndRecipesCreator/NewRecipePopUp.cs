using DevTools;
using DevTools.PopUps;
using Kitchen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class NewRecipePopUp : AbstractNewAssetPopUP<RecipeName>
{
    protected override string enumFolder => Path.Combine("Kitchen", "Elements", "Recipe", "Scripts");

    protected override string enumFile => "RecipeName.cs";

    static SerializedProperty m_RecipeName = null;
    static SerializedProperty m_Disease = null;
    static SerializedProperty m_sprite = null;

    [SerializeField] DiseaseName diseasesItCures;
    [SerializeField] Sprite recipeSprite;


    private new void OnEnable()
    {
        base.OnEnable();
        m_RecipeName = so.FindProperty("name");
        m_Disease = so.FindProperty("diseasesItCures");
        m_sprite = so.FindProperty("recipeSprite");
    }


    protected override void DisplayProperties()
    {
        EditorGUILayout.PropertyField(m_RecipeName);
        EditorGUILayout.PropertyField(m_Disease, new GUIContent("Disease(s) it cures"));
        EditorGUILayout.PropertyField(m_sprite, new GUIContent("Sprite"));

        if (GUILayout.Button("Create new Recipe", GUILayout.Height(30f)))
        {
            if (String.IsNullOrEmpty(m_RecipeName.stringValue))
                warningLabel = "Recipe Name cannot be empty";
            else if (Array.Exists<string>(enumValues, element => element == m_RecipeName.stringValue))
                warningLabel = "Recipe Name already exists";
            else if (m_Disease.enumValueFlag == 0)
                warningLabel = "Must Select At least One Disease"; 
            else if (m_sprite.objectReferenceValue == null)
                warningLabel = "Must Set the recipe Sprite";
            else
            {
                AddToEnum();
                warningLabel = "";
                Debug.Log("Recipe " + m_RecipeName.stringValue + " created");
            }
        }
    }
    [DidReloadScripts]
    protected static void InstantiateScriptableObject()
    {
        if(typeof(RecipeData) !=null &&  m_RecipeName != null && !String.IsNullOrEmpty(m_RecipeName.stringValue))
        {
            RecipeData SO = (RecipeData)ScriptableObject.CreateInstance(typeof(RecipeData));
            SO.recipeName = (RecipeName)Enum.Parse(typeof(RecipeName), m_RecipeName.stringValue);
            SO.diseasesItCures = (DiseaseName)m_Disease.enumValueFlag;
            SO.sprite = (Sprite)m_sprite.objectReferenceValue;

            string scriptSOFolder = IngredientsNRecipesCreatorWindow.RecipesSOPath;
            string scriptSOFile = m_RecipeName.stringValue;

            AssetDatabase.CreateAsset(SO, Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));
            AssetDatabase.ImportAsset(Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));

            so.Update();
            m_RecipeName.stringValue = null;
            m_Disease.enumValueFlag = 0;
            m_sprite.objectReferenceValue=null;
            so.ApplyModifiedProperties();
        }
    }

    protected override void AddToEnum()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("namespace Kitchen");
        sb.AppendLine("{");
        sb.AppendLine("    public enum RecipeName");
        sb.AppendLine("    {");
        sb.Append("        ");
        foreach (var recipe in enumValues)
            sb.Append(recipe + ", ");
        sb.AppendLine(m_RecipeName.stringValue);
        sb.AppendLine("    }");
        sb.AppendLine("}");

        string filePath = Path.Combine(enumFolder, enumFile);
        CheckAndCreate(enumFolder, filePath, sb);
    }      
}
