using DevTools.PopUps;
using Kitchen;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DevTools
{
    public class IngredientsNRecipesCreatorWindow : EditorWindow
    {
        string[] tabs = { "Ingredients", "Recipes", "Illnesses" };
        int selectedTab = 0;

        [SerializeField]  List<IngredientData> ingredients = new List<IngredientData>();
        [SerializeField]  List<RecipeData> recipes = new List<RecipeData>();
        protected SerializedProperty m_list = null;
        protected SerializedProperty r_list = null;
        protected bool[] ingredientsBools;
        protected bool[] recipesBools;
        protected Vector2 scrollPos = Vector2.zero;

        ScriptableObject target;
        SerializedObject so =null;

        public static string IngredientsSOPath = Path.Combine("Assets", "Kitchen", "Elements", "Ingredient", "Data");
        public static string RecipesSOPath = Path.Combine("Assets", "Kitchen", "Elements", "Recipe", "Data");

        [MenuItem("DevWindows/RecipesManager", false, 51)]
        public static void ShowWindow() => GetWindow<IngredientsNRecipesCreatorWindow>("RecipesManager");

        private void OnGUI()
        {
            selectedTab = GUILayout.Toolbar(selectedTab, tabs, GUILayout.Height(30f)) ;
            GUILayout.Space(20f);

            so.Update();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            switch (selectedTab)
            {
                case 0:
                    Ingredients();
                    break;
                case 1:
                    Recipes();
                    break;
            }

            EditorGUILayout.EndScrollView();
            so.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            target = this;
            so = new SerializedObject(target);
            m_list = so.FindProperty("ingredients");
            r_list = so.FindProperty("recipes");
            ingredientsBools = UpdateList(ingredients.Count);
            recipesBools = UpdateList(recipes.Count);
            ingredients = GetAssetsList<IngredientData>(IngredientsSOPath);
            recipes = GetAssetsList<RecipeData>(RecipesSOPath);
        }

        void Ingredients()
        {      
            if(ingredientsBools.Length != ingredients.Count)
                ingredientsBools = UpdateList(ingredients.Count);

            for (int i=0; i<ingredients.Count;i++)
            {
                SerializedObject currentObject = new SerializedObject( m_list.GetArrayElementAtIndex(i).objectReferenceValue);
                currentObject.Update();

                EditorGUILayout.BeginVertical(GUI.skin.FindStyle("Badge"));
                ingredientsBools[i] = EditorGUILayout.Foldout(ingredientsBools[i],  Enum.GetName(typeof(IngredientName) ,currentObject.FindProperty("ingredientName").enumValueFlag));
                if (ingredientsBools[i])
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

            if(GUILayout.Button("Add new Ingredient", GUILayout.Height(30f)))      
               GetWindow<NewIngredientPopUP>("Create New Ingredient");      
        }

        void Recipes() 
        {
            if (recipesBools.Length != recipes.Count)
                recipesBools = UpdateList(recipes.Count);
            for (int i = 0; i < recipes.Count; i++)
            {
                SerializedObject currentObject = new SerializedObject(r_list.GetArrayElementAtIndex(i).objectReferenceValue);
                currentObject.Update();

                EditorGUILayout.BeginVertical(GUI.skin.FindStyle("Badge"));
                    //ingredientsBools[i] = EditorGUILayout.Foldout(ingredientsBools[i], Enum.GetName(typeof(C), currentObject.FindProperty("ingredientName").enumValueFlag));
                    EditorGUILayout.PropertyField(currentObject.FindProperty("name"), new GUIContent("Recipe Name"));
                    EditorGUILayout.PropertyField(currentObject.FindProperty("diseasesItCures"), new GUIContent("Disease(s) it cures"));
                    EditorGUILayout.PropertyField(currentObject.FindProperty("sprite"));

                    SerializedProperty ingredients = currentObject.FindProperty("ingredients");
                    SerializedProperty array = currentObject.FindProperty("popUp");
                
                    for(int j=0; j< ingredients.arraySize; j++)
                    {
                        SerializedProperty ingredient = ingredients.GetArrayElementAtIndex(j);

                        EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.BeginVertical();
                                EditorGUILayout.PropertyField(ingredient, new GUIContent("Ingredient " + (j + 1)));
                                if (ingredient.FindPropertyRelative("ingredient").objectReferenceValue != null && ingredient.isExpanded)
                                {
                                    List<string> options = new List<string>();
                                    SerializedObject ingredientSO = new SerializedObject(ingredient.FindPropertyRelative("ingredient").objectReferenceValue);

                                    if ((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.Stove) != 0 && !options.Contains("Stove"))
                                        options.Add("Stove");

                                    if ((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.Mortar) != 0 && !options.Contains("Mortar"))
                                        options.Add("Mortar");

                                    if ((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.None) != 0 && !options.Contains("None"))
                                        options.Add("None");

                                    array.GetArrayElementAtIndex(j).intValue = EditorGUILayout.Popup("Cooking tool", array.GetArrayElementAtIndex(j).intValue, options.ToArray());
                                    ingredient.FindPropertyRelative("cookingToolName").enumValueFlag = (int)Enum.Parse(typeof(CookingToolName), options[array.GetArrayElementAtIndex(j).intValue]);
                                }
                            EditorGUILayout.EndVertical();
                    if(ingredient.isExpanded)
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.Space(20f);
                        if (GUILayout.Button(new GUIContent("Delete"), GUILayout.Height(30f)))
                        {
                            ingredients.DeleteArrayElementAtIndex(i);
                            array.DeleteArrayElementAtIndex(i);
                        }
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndHorizontal();
                    }

                EditorGUILayout.Space(10f);
                if (GUILayout.Button(new GUIContent("Add ingredient"), GUILayout.Height(30f)))
                {
                    ingredients.arraySize++;
                    array.arraySize++;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(10f);
                currentObject.ApplyModifiedProperties();
            }

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
        protected bool[] UpdateList(int newSize)
        {
            bool[] foldsBools = new bool[newSize];
            Array.Fill(foldsBools, true);
            return foldsBools;
        }
    }
}

