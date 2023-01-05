using UnityEditor;
using UnityEngine;
using Kitchen;
using UnityEngine.PlayerLoop;
using System.IO;
using static RecipeData;
using System.Xml.Linq;
using System.Collections.Generic;
using System;

public class IngredientsNRecipesCreatorWindow : EditorWindow
{
    string[] tabs = { "Ingredients", "Recipes", "Illnesses" };
    int selectedTab = 0;

    [SerializeField]  List<IngredientData> ingredients = new List<IngredientData>();
    protected SerializedProperty m_list = null;
    protected bool[] foldsBools;
    protected Vector2 scrollPos = Vector2.zero;

    ScriptableObject target;
    SerializedObject so =null;

    private static string IngredientsSOPath = Path.Combine("Assets", "Kitchen", "Elements", "Ingredient", "Data");

    [MenuItem("DevWindows/RecipesManager", false, 51)]
    public static void ShowWindow() =>
        GetWindow<IngredientsNRecipesCreatorWindow>("RecipesManager");

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabs, GUILayout.Height(30f)) ;
        GUILayout.Space(20f);
        switch (selectedTab)
        {
            case 0:
                Ingredients();

                break;
        }
    }


    private void OnEnable()
    {
        target = this;
        so = new SerializedObject(target);
        m_list = so.FindProperty("ingredients");
        UpdateList();
        ingredients = GetAssetsList<IngredientData>(IngredientsSOPath);
    }

    void Ingredients()
    {
        so.Update();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i=0; i<ingredients.Count;i++)
        {
            SerializedObject currentObject = new SerializedObject( m_list.GetArrayElementAtIndex(i).objectReferenceValue);
            currentObject.Update();

            EditorGUILayout.BeginVertical(GUI.skin.FindStyle("Badge"));
            foldsBools[i] = EditorGUILayout.Foldout(foldsBools[i],  Enum.GetName(typeof(IngredientName) ,currentObject.FindProperty("ingredientName").enumValueFlag));
            if (foldsBools[i])
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(currentObject.FindProperty("ingredientName"));
                EditorGUILayout.PropertyField(currentObject.FindProperty("necessaryCookingTool"), new GUIContent("Cooking Tool"));

                if (ingredients[i].necessaryCookingTool.HasFlag(CookingToolName.Stove))
                {
                    EditorGUILayout.PropertyField(currentObject.FindProperty("rawState"));
                    EditorGUILayout.PropertyField(currentObject.FindProperty("cookedState"));
                    EditorGUILayout.PropertyField(currentObject.FindProperty("burntState"));
                }

                if (ingredients[i].necessaryCookingTool.HasFlag(CookingToolName.Mortar))
                {
                    EditorGUILayout.PropertyField(currentObject.FindProperty("entireState"));
                    EditorGUILayout.PropertyField(currentObject.FindProperty("crushedState"));
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10f);
            currentObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndScrollView();
        so.ApplyModifiedProperties();
    }

    List<T> GetAssetsList<T>(string folder) where T: ScriptableObject
    {
        string[] dataFiles = Directory.GetFiles(folder, "*.asset");
        List<T> list = new List<T>();

        foreach (string dataFile in dataFiles)
        {
            string assetPath = dataFile.Replace(Application.dataPath, "").Replace('\\', '/');
            T sourceAsset = (T)AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));

            list.Add(sourceAsset);
        }

        return list;
    }
    protected void UpdateList()
    {
        foldsBools = new bool[ingredients.Count];
        System.Array.Fill(foldsBools, true);
    }

}
