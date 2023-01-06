using Kitchen;
using System.IO;
using System.Text;
using System;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace DevTools.PopUps
{
    public class NewIngredientPopUP : AbstractNewAssetPopUP<IngredientName>
    {
        [SerializeField] CookingToolName cookingTool;

        [Header("Stove Sprites")]
        [SerializeField] Sprite rawState;
        [SerializeField] Sprite cookedState;
        [SerializeField] Sprite burntState;

        [Header("Mortar Sprites")]
        [SerializeField] Sprite entireState;
        [SerializeField] Sprite crushedState;

        bool stoveChossed = false;
        bool mortarChossed = false;

        static SerializedProperty m_newIngredientName =null;
        static SerializedProperty m_cookingTool = null;

        static SerializedProperty m_rawState = null;
        static SerializedProperty m_cookedState = null;
        static SerializedProperty m_burntState = null;

        static SerializedProperty m_entireState = null;
        static SerializedProperty m_crushedState = null;
        protected override string enumFolder  => Path.Combine("Kitchen", "Elements", "Ingredient", "Scripts");
        protected override string enumFile => "IngredientName.cs";

        public static UnityAction assetCreated;

        private new void OnEnable()
        {
            base.OnEnable();
            m_newIngredientName = so.FindProperty("name");
            m_cookingTool = so.FindProperty("cookingTool");
            m_rawState = so.FindProperty("rawState");
            m_cookedState = so.FindProperty("cookedState");
            m_burntState = so.FindProperty("burntState");
            m_entireState = so.FindProperty("entireState");
            m_crushedState = so.FindProperty("crushedState");
        }

        protected override void DisplayProperties()
        {
            EditorGUILayout.PropertyField(m_newIngredientName);
            EditorGUILayout.PropertyField(m_cookingTool);

            if ((m_cookingTool.enumValueFlag & (int)CookingToolName.Stove) != 0)
            {
                stoveChossed = true;
                EditorGUILayout.PropertyField(m_rawState);
                EditorGUILayout.PropertyField(m_cookedState);
                EditorGUILayout.PropertyField(m_burntState);
            }
            else
                stoveChossed = false;

            if ((m_cookingTool.enumValueFlag & (int)CookingToolName.Mortar) != 0)
            {
                mortarChossed = true;
                EditorGUILayout.PropertyField(m_entireState);
                EditorGUILayout.PropertyField(m_crushedState);
            }
            else
                mortarChossed = false;

            if (GUILayout.Button("Create new Ingredient", GUILayout.Height(30f)))
            {
                if (String.IsNullOrEmpty(m_newIngredientName.stringValue))
                    warningLabel = "Ingredient Name cannot be empty";

                else if (Array.Exists<string>(enumValues, element => element == m_newIngredientName.stringValue))
                    warningLabel = "Ingredient Name already exists";

                else if (m_cookingTool.enumValueFlag == 0)
                    warningLabel = "Must Select At least One Cooking Tool";
                else if ((stoveChossed && (m_rawState.objectReferenceValue == null || m_cookedState.objectReferenceValue == null || m_burntState.objectReferenceValue == null))
                        || (mortarChossed && (m_entireState.objectReferenceValue == null || m_crushedState.objectReferenceValue == null)))
                    warningLabel = "Must set all sprites for selected(s) cooking tool";
                else
                {
                    AddToEnum();
                    warningLabel = "";
                    Debug.Log("Ingredient " + m_newIngredientName.stringValue + " created");
                }
            }
        }

        protected override void AddToEnum()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace Kitchen");
            sb.AppendLine("{");
            sb.AppendLine("    public enum IngredientName");
            sb.AppendLine("    {");
            sb.Append("        ");
            foreach (var ingredient in enumValues)
                sb.Append(ingredient + ", ");
            sb.AppendLine(m_newIngredientName.stringValue);
            sb.AppendLine("    }");
            sb.AppendLine("}");

            string filePath = Path.Combine(enumFolder, enumFile);
            CheckAndCreate(enumFolder, filePath, sb); 
        }

        [DidReloadScripts]
        protected static void InstantiateScriptableObject()
        {
            if (typeof(IngredientData) != null && m_newIngredientName!=null && !String.IsNullOrEmpty(m_newIngredientName.stringValue))
            {
                IngredientData SO = (IngredientData)ScriptableObject.CreateInstance(typeof(IngredientData));
                SO.ingredientName = (IngredientName)Enum.Parse(typeof(IngredientName), m_newIngredientName.stringValue);
                SO.necessaryCookingTool = (CookingToolName)m_cookingTool.enumValueFlag;
                SO.rawState = (Sprite)m_rawState.objectReferenceValue;
                SO.cookedState = (Sprite)m_cookedState.objectReferenceValue;
                SO.burntState = (Sprite)m_burntState.objectReferenceValue;
                SO.entireState = (Sprite)m_entireState.objectReferenceValue;
                SO.crushedState = (Sprite)m_crushedState.objectReferenceValue;

                string scriptSOFolder = IngredientsNRecipesCreatorWindow.IngredientsSOPath;
                string scriptSOFile = m_newIngredientName.stringValue;
                AssetDatabase.CreateAsset(SO, Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));
                AssetDatabase.ImportAsset(Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));

                so.Update();
                m_newIngredientName.stringValue = null;
                m_cookingTool.enumValueFlag = 0;
                m_rawState.objectReferenceValue = null;
                m_cookedState.objectReferenceValue = null;
                m_burntState.objectReferenceValue = null;
                m_entireState.objectReferenceValue = null;
                m_crushedState.objectReferenceValue = null;
                so.ApplyModifiedProperties();

                assetCreated?.Invoke();
            }
        }
    }
}