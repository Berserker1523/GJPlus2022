using FMOD.Studio;
using Kitchen;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static RecipeData;

[CustomEditor(typeof(RecipeData))]
public class RecipeCustomEditor : Editor
{

    SerializedProperty ingredients;
    SerializedProperty cookingTools;
    RecipeData recipe;


    private string[] cokingToolOnlyNone = new string[1] { "None" };
    private string[] cookingToolOnlyStove = new string[1] { "Stove" };
    private string[] cookingToolOnlyMortar = new string[1] { "Mortar" };
    private string[] cookingToolBoth = new string[2] { "Mortar", "Stove" };


    private int _currentToolbarSubType;

    private void OnEnable()
    {
        recipe = (RecipeData)target;
        ingredients = serializedObject.FindProperty("ingredients");
        cookingTools = serializedObject.FindProperty("cookingTool");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < ingredients.arraySize; i++)
        {

            SerializedProperty ingredient = ingredients.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(ingredient, new GUIContent("Ingredient " + (i + 1)));


            if (ingredient != null)
            {
                SerializedObject ingredientSO = new SerializedObject(ingredient.FindPropertyRelative("ingredient").objectReferenceValue);
                if ((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.Stove)  != 0 
                    && (ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.Mortar) != 0)
              
                    EditorGUILayout.PropertyField(ingredient.FindPropertyRelative("cookingToolBoth"), new GUIContent("Cooking Tool"));

                else if((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.Stove) != 0)              
                    EditorGUILayout.PropertyField(ingredient.FindPropertyRelative("cookingToolOnlyStove"), new GUIContent("Cooking Tool"));

                else if((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.Mortar) != 0)              
                    EditorGUILayout.PropertyField(ingredient.FindPropertyRelative("cookingToolOnlyMortar"), new GUIContent("Cooking Tool"));

                else if((ingredientSO.FindProperty("necessaryCookingTool").enumValueFlag & (int)CookingToolName.None) != 0)              
                    EditorGUILayout.PropertyField(ingredient.FindPropertyRelative("cookingToolOnlyNone"), new GUIContent("Cooking Tool"));

                    EditorGUILayout.EndVertical();

                if (GUILayout.Button(new GUIContent("Delete"),GUILayout.Height ( 30f)))
                {
                    ingredients.DeleteArrayElementAtIndex(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(10f);

            if (GUILayout.Button(new GUIContent("Add ingredient"), GUILayout.Height(30f)))
            {
                ingredients.arraySize++;
            }


            serializedObject.ApplyModifiedProperties();

        }
    }
}
