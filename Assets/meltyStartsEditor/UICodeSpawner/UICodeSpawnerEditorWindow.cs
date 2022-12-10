using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KuusouEngine
{
    public class UICodeSpawnerEditorWindow : EditorWindow
    {
        private static string m_CodePath = "meltyStarsMain/Generate/UI";
        private static string m_NameSpace = "MeltyStars";
        private static GameObject m_SelectedGameObject;
        private static bool m_isSpawnForWindow = true;
        private static bool m_isSpawnForWindowView = true;
        [MenuItem("meltyStars/UI/SpawnUICode")]
        private static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<UICodeSpawnerEditorWindow>();
            window.Show();
        }
        void OnGUI()
        {
            EditorGUILayout.LabelField("CodePath");
            m_CodePath = EditorGUILayout.TextField(m_CodePath);
            EditorGUILayout.LabelField("NameSpace");
            m_NameSpace = EditorGUILayout.TextField(m_NameSpace);
            EditorGUILayout.LabelField("选择UI物体以生成代码......");
            m_SelectedGameObject = EditorGUILayout.ObjectField("WindowObject", m_SelectedGameObject, typeof(GameObject), true) as GameObject;
            m_isSpawnForWindow = EditorGUILayout.ToggleLeft("SpawnForWindow", m_isSpawnForWindow);
            m_isSpawnForWindowView = EditorGUILayout.ToggleLeft("SpawnForWindowView", m_isSpawnForWindowView);
            if (GUILayout.Button("SpawnCodeForUI", GUILayout.Height(50)))
            {
                if (!m_SelectedGameObject) return;
                if (!m_SelectedGameObject.name.EndsWith("Window"))
                {
                    Debug.LogWarning("GameObject命名不规范，将不会生成UI代码。请尝试使用Window结尾命名!");
                    return;
                }
                UICodeSpawner.SpawnUICode(m_SelectedGameObject, m_NameSpace, m_CodePath, m_isSpawnForWindow, m_isSpawnForWindowView);
            }
        }
    }
}
