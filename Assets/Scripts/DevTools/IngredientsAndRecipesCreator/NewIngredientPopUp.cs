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
        [SerializeField] CookingToolName necessaryCookingTool;
        [SerializeField] Sprite rawSprite;
        [SerializeField] Sprite dishSprite;

        [Header("Stove Sprites")]
        [SerializeField] Sprite stoveRawSprite;
        [SerializeField] Sprite stoveCookedSprite;
        [SerializeField] Sprite stoveBurntSprite;

        [Header("Mortar Sprites")]
        [SerializeField] Sprite mortarRawSprite;
        [SerializeField] Sprite mortarCrushedSprite;

        bool stoveChossed = false;
        bool mortarChossed = false;

        static SerializedProperty m_newIngredientName = null;
        static SerializedProperty m_cookingTool = null;
        static SerializedProperty m_rawSprite = null;
        static SerializedProperty m_dishSprite = null;

        static SerializedProperty m_stoveRawSprite = null;
        static SerializedProperty m_stoveCookedSprite = null;
        static SerializedProperty m_stoveBurntSprite = null;

        static SerializedProperty m_mortarRawSprite = null;
        static SerializedProperty m_mortarCrushedSprite = null;
        protected override string enumFolder => Path.Combine("Kitchen", "Elements", "Ingredient", "Scripts");
        protected override string enumFile => "IngredientName.cs";

        public static UnityAction assetCreated;

        private new void OnEnable()
        {
            base.OnEnable();
            m_newIngredientName = so.FindProperty(nameof(name));
            m_cookingTool = so.FindProperty(nameof(necessaryCookingTool));
            m_rawSprite = so.FindProperty(nameof(rawSprite));
            m_dishSprite = so.FindProperty(nameof(dishSprite));
            m_stoveRawSprite = so.FindProperty(nameof(stoveRawSprite));
            m_stoveCookedSprite = so.FindProperty(nameof(stoveCookedSprite));
            m_stoveBurntSprite = so.FindProperty(nameof(stoveBurntSprite));
            m_mortarRawSprite = so.FindProperty(nameof(mortarRawSprite));
            m_mortarCrushedSprite = so.FindProperty(nameof(mortarCrushedSprite));
        }

        protected override void DisplayProperties()
        {
            EditorGUILayout.PropertyField(m_newIngredientName);
            EditorGUILayout.PropertyField(m_cookingTool);
            EditorGUILayout.PropertyField(m_rawSprite);
            EditorGUILayout.PropertyField(m_dishSprite);

            if ((m_cookingTool.enumValueFlag & (int)CookingToolName.Stove) != 0)
            {
                stoveChossed = true;
                EditorGUILayout.PropertyField(m_stoveRawSprite);
                EditorGUILayout.PropertyField(m_stoveCookedSprite);
                EditorGUILayout.PropertyField(m_stoveBurntSprite);
            }
            else
                stoveChossed = false;

            if ((m_cookingTool.enumValueFlag & (int)CookingToolName.Mortar) != 0)
            {
                mortarChossed = true;
                EditorGUILayout.PropertyField(m_mortarRawSprite);
                EditorGUILayout.PropertyField(m_mortarCrushedSprite);
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
                else if ((stoveChossed && (m_stoveRawSprite.objectReferenceValue == null || m_stoveCookedSprite.objectReferenceValue == null || m_stoveBurntSprite.objectReferenceValue == null))
                        || (mortarChossed && (m_mortarRawSprite.objectReferenceValue == null || m_mortarCrushedSprite.objectReferenceValue == null)))
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
            if (typeof(IngredientData) != null && m_newIngredientName != null && !String.IsNullOrEmpty(m_newIngredientName.stringValue))
            {
                IngredientData SO = (IngredientData)ScriptableObject.CreateInstance(typeof(IngredientData));
                SO.ingredientName = (IngredientName)Enum.Parse(typeof(IngredientName), m_newIngredientName.stringValue);
                SO.necessaryCookingTool = (CookingToolName)m_cookingTool.enumValueFlag;
                SO.rawSprite = (Sprite)m_rawSprite.objectReferenceValue;
                SO.dishSprite = (Sprite)m_dishSprite.objectReferenceValue;
                SO.stoveRawSprite = (Sprite)m_stoveRawSprite.objectReferenceValue;
                SO.stoveCookedSprite = (Sprite)m_stoveCookedSprite.objectReferenceValue;
                SO.stoveBurntSprite = (Sprite)m_stoveBurntSprite.objectReferenceValue;
                SO.mortarRawSprite = (Sprite)m_mortarRawSprite.objectReferenceValue;
                SO.mortarCrushedSprite = (Sprite)m_mortarCrushedSprite.objectReferenceValue;

                string scriptSOFolder = IngredientsNRecipesCreatorWindow.IngredientsSOPath;
                string scriptSOFile = m_newIngredientName.stringValue;
                AssetDatabase.CreateAsset(SO, Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));
                AssetDatabase.ImportAsset(Path.Combine(scriptSOFolder, scriptSOFile + ".asset"));

                so.Update();
                m_newIngredientName.stringValue = null;
                m_cookingTool.enumValueFlag = 0;
                m_rawSprite.objectReferenceValue = null;
                m_dishSprite.objectReferenceValue = null;
                m_stoveRawSprite.objectReferenceValue = null;
                m_stoveCookedSprite.objectReferenceValue = null;
                m_stoveBurntSprite.objectReferenceValue = null;
                m_mortarRawSprite.objectReferenceValue = null;
                m_mortarCrushedSprite.objectReferenceValue = null;
                so.ApplyModifiedProperties();

                assetCreated?.Invoke();
            }
        }
    }
}