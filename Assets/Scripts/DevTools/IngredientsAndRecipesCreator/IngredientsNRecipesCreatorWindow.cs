using UnityEditor;
using UnityEngine;
using Kitchen;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEditor.Callbacks;

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

    public static string IngredientsSOPath = Path.Combine("Assets", "Kitchen", "Elements", "Ingredient", "Data");

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
        if(foldsBools.Length != ingredients.Count)
            UpdateList();

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

        if(GUILayout.Button("Add new Ingredient", GUILayout.Height(30f)))      
           GetWindow<NewIngredientPopUP>("Create New Ingredient");
        


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

public class NewIngredientPopUP: EditorWindow
{
    static NewIngredientPopUP popUp_instance;
    ScriptableObject target;
    SerializedObject so = null;

    [SerializeField] string newIngredientName;
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

    protected Vector2 scrollPos = Vector2.zero;

    static SerializedProperty m_newIngredientName;
   static SerializedProperty m_cookingTool;

   static SerializedProperty m_rawState;
   static SerializedProperty m_cookedState;
   static SerializedProperty m_burntState;

   static SerializedProperty m_entireState;
   static SerializedProperty m_crushedState;

    string[] enumValues;
    private string enumFolder = Path.Combine("Kitchen", "Elements", "Ingredient", "Scripts");
    private string enumFile ="IngredientName.cs";

    string warningLabel = "";
    GUIStyle style = new GUIStyle();

    private void OnEnable()
    {
        target = this;
        so = new SerializedObject(target);

        m_newIngredientName = so.FindProperty("newIngredientName");
        m_cookingTool = so.FindProperty("cookingTool");
        m_rawState = so.FindProperty("rawState");
        m_cookedState = so.FindProperty("cookedState");
        m_burntState = so.FindProperty("burntState");
        m_entireState = so.FindProperty("entireState");
        m_crushedState = so.FindProperty("crushedState");
        enumValues = Enum.GetValues(typeof(IngredientName)).OfType<IngredientName>().Select(o => o.ToString()).ToArray();

        style.normal.textColor = Color.red; 
        style.fontStyle = FontStyle.Bold;
    }

    private void OnGUI()
    {
        so.Update();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

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
            else if((stoveChossed && (m_rawState.objectReferenceValue == null || m_cookedState.objectReferenceValue == null || m_burntState.objectReferenceValue == null))             
                    || (mortarChossed && (m_entireState.objectReferenceValue == null || m_crushedState.objectReferenceValue == null)))
                warningLabel = "Must set all sprites for selected(s) cooking tool";               
            else
            {
                AddToEnum();
                warningLabel = "";
                Debug.Log("Ingredient " + m_newIngredientName.stringValue + " created");
            }
        }
        EditorGUILayout.LabelField(warningLabel, style);
        EditorGUILayout.EndScrollView();

        so.ApplyModifiedProperties();
    }

    void AddToEnum()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("namespace Kitchen");
        sb.AppendLine("{");
        sb.AppendLine("    public enum IngredientName");
        sb.AppendLine("    {");
        sb.Append("        ");
        foreach (var ingredient in enumValues)
            sb.Append(ingredient +", ");
        sb.AppendLine(m_newIngredientName.stringValue);
        sb.AppendLine("    }");
        sb.AppendLine("}");

        string filePath = Path.Combine(enumFolder, enumFile);
        CheckAndCreate(enumFolder, filePath, sb);
    }
    [DidReloadScripts]
    static void InstantiateScriptableObject()
    {
        if (typeof(IngredientData) != null)
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
        }
    }

     void CheckDirectory(string folder)
    {
        if (!Directory.Exists(Path.Combine(Application.dataPath, folder)))
            Directory.CreateDirectory(Path.Combine(Application.dataPath, folder));
    }
     void CheckFile(string file)
    {
        if (File.Exists(Path.Combine(Application.dataPath, file)))
            File.Delete(Path.Combine(Application.dataPath, file));
    }
     void CreateAutogneratedFile(string filePath, StringBuilder sb)
    { 
        File.WriteAllText(Path.Combine(Application.dataPath, filePath), sb.ToString(), Encoding.UTF8);
        AssetDatabase.ImportAsset(Path.Combine("Assets",filePath), ImportAssetOptions.ForceUpdate);

    }

    void CheckAndCreate(string folder, string file, StringBuilder sb)
    {
        CheckDirectory(folder); 
        CheckFile(file);
        CreateAutogneratedFile(file, sb);
    }
}
