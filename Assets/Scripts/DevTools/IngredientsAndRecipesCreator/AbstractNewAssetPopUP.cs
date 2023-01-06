using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DevTools.PopUps
{
    public abstract class AbstractNewAssetPopUP<T> : EditorWindow where T: Enum
    {
        [SerializeField] string name;

        protected ScriptableObject target;
        protected static SerializedObject so = null;

        protected Vector2 scrollPos = Vector2.zero;
        protected string[] enumValues;
        protected string warningLabel = "";
        protected GUIStyle style = new GUIStyle();

        protected abstract string enumFolder { get;  }
        protected abstract string enumFile { get;  }

        protected abstract void DisplayProperties();
        protected abstract void AddToEnum();

        protected virtual void OnEnable()
        {
            target = this;
            so = new SerializedObject(target);
            style.normal.textColor = Color.red;
            style.fontStyle = FontStyle.Bold;
            enumValues = Enum.GetValues(typeof(T)).OfType<T>().Select(o => o.ToString()).ToArray();
        }

        protected virtual void OnGUI()
        {
            so.Update();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DisplayProperties();
            EditorGUILayout.LabelField(warningLabel, style);
            EditorGUILayout.EndScrollView();
            so.ApplyModifiedProperties();
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
            AssetDatabase.ImportAsset(Path.Combine("Assets", filePath), ImportAssetOptions.ForceUpdate);
        }
        protected void CheckAndCreate(string folder, string file, StringBuilder sb)
        {
            CheckDirectory(folder);
            CheckFile(file);
            CreateAutogneratedFile(file, sb);
        }
    }
}
