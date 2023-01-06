using DevTools;
using DevTools.PopUps;
using Kitchen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class NewRecipePopUp : AbstractNewAssetPopUP<IngredientName>
{
    protected override string enumFolder => throw new System.NotImplementedException();

    protected override string enumFile => throw new System.NotImplementedException();

    static SerializedProperty m_RecipeName = null;

    private new void OnEnable()
    {
        base.OnEnable();
        m_RecipeName = so.FindProperty("name");

    }


    protected override void DisplayProperties()
    {
        EditorGUILayout.PropertyField(m_RecipeName);

        if (GUILayout.Button("Create new Recipe", GUILayout.Height(30f)))
        {
            if (String.IsNullOrEmpty(m_RecipeName.stringValue))
                warningLabel = "Recipe Name cannot be empty";
            else
            {
                AddToEnum();
                warningLabel = "";
                Debug.Log("Recipe " + m_RecipeName.stringValue + " created");
            }
        }
    }
  /*  [DidReloadScripts]
    protected static void InstantiateScriptableObject()
    {
        if(typeof(RecipeData) !=null &&  m_RecipeName != null && !String.IsNullOrEmpty(m_RecipeName.stringValue))
        {
            RecipeData SO = (RecipeData)ScriptableObject.CreateInstance(typeof(RecipeData));
            SO.name = m_RecipeName.stringValue;

            string scriptSOFolder = IngredientsNRecipesCreatorWindow.RecipesSOPath;
            string scriptSOFile = m_RecipeName.stringValue;

            AssetDatabase.CreateAsset(SO, Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));
            AssetDatabase.ImportAsset(Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));

            so.Update();
            m_RecipeName.stringValue = null;
            so.ApplyModifiedProperties();
        }
    }*/

    protected override void AddToEnum()
    {
        if(typeof(RecipeData) != null && m_RecipeName != null && !String.IsNullOrEmpty(m_RecipeName.stringValue))
        {
            RecipeData SO = (RecipeData)ScriptableObject.CreateInstance(typeof(RecipeData));
            SO.name = m_RecipeName.stringValue;

            string scriptSOFolder = IngredientsNRecipesCreatorWindow.RecipesSOPath;
            string scriptSOFile = m_RecipeName.stringValue;

            AssetDatabase.CreateAsset(SO, Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));
            AssetDatabase.ImportAsset(Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));

            so.Update();
            m_RecipeName.stringValue = null;
            so.ApplyModifiedProperties();

            AssetDatabase.Refresh();
        }
    }      
}
