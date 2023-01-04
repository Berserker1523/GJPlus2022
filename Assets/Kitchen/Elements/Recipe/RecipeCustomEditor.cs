using FMOD.Studio;
using Kitchen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    new SerializedProperty name;
    SerializedProperty diseases;
    SerializedProperty sprite;
    SerializedProperty array ;
    //List<int> popUp = new List<int>();
    //RecipeData recipe;

    private int _currentToolbarSubType;

    private void OnEnable()
    {
        //recipe = (RecipeData)target;
        ingredients = serializedObject.FindProperty("ingredients");
        name = serializedObject.FindProperty("name");
        diseases = serializedObject.FindProperty("diseasesItCures");
        sprite = serializedObject.FindProperty("sprite");
        array = serializedObject.FindProperty("popUp");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(name, new GUIContent("Recipe Name"));
        EditorGUILayout.PropertyField(diseases, new GUIContent("Desease(s) it cures"));
        EditorGUILayout.PropertyField(sprite);

        EditorGUILayout.Space(20f);
        EditorGUILayout.LabelField("Ingredients List");

        for (int i = 0; i < ingredients.arraySize; i++)
        {
            SerializedProperty ingredient = ingredients.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(ingredient, new GUIContent("Ingredient " + (i + 1)));
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

                        array.GetArrayElementAtIndex(i).intValue = EditorGUILayout.Popup("Cooking tool", array.GetArrayElementAtIndex(i).intValue, options.ToArray());           
                       ingredient.FindPropertyRelative("cookingToolName").enumValueFlag = (int)Enum.Parse(typeof(CookingToolName), options[array.GetArrayElementAtIndex(i).intValue]);
                    }
                EditorGUILayout.EndVertical();

                if (ingredient.isExpanded)
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

            EditorGUILayout.Space(10f);
        }
            if (GUILayout.Button(new GUIContent("Add ingredient"), GUILayout.Height(30f)))
            {
                ingredients.arraySize++;
                array.arraySize++;
            }

            serializedObject.ApplyModifiedProperties();     
    }
}

