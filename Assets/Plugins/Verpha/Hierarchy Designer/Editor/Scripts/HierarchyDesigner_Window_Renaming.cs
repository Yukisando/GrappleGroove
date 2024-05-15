#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Renaming : EditorWindow
    {
        #region OnGUI Properties
        private Vector2 scrollPositionOuter;
        private static Vector2 lastMainWindowSize;
        private GUIStyle customSettingsStyle;
        private GUIStyle headerLabelStyle;
        #endregion
        #region General Properties
        private static string newName;
        private static GameObject[] selectedGameObjects;
        private static bool autoIncrementation = true;
        #endregion

        public static void OpenWindow(GameObject[] gameObjects, Vector2 position, bool autoIndex = true)
        {
            HierarchyDesigner_Window_Renaming window = GetWindow<HierarchyDesigner_Window_Renaming>("Hierarchy Designer Renaming");
            Vector2 size = new Vector2(350, 118);
            window.minSize = size;

            autoIncrementation = autoIndex;
            Rect mainWindowRect = EditorGUIUtility.GetMainWindowPosition();
            if (mainWindowRect.size != lastMainWindowSize)
            {
                lastMainWindowSize = mainWindowRect.size;
                Vector2 centerPoint = new Vector2(
                    (mainWindowRect.width - size.x) * 0.5f,
                    (mainWindowRect.height - size.y) * 0.5f
                );
                window.position = new Rect(centerPoint, size);
            }
            selectedGameObjects = gameObjects;
            newName = "";
        }

        private void InitializeStyles()
        {
            customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            headerLabelStyle = HierarchyDesigner_Info_OnGUI.HeaderLabelStyle;
        }

        private void OnGUI()
        {
            InitializeStyles();
            EditorGUILayout.BeginVertical(customSettingsStyle);

            GUILayout.Space(4);
            EditorGUILayout.LabelField("Rename Selected GameObjects", headerLabelStyle);
            GUILayout.Space(8);

            scrollPositionOuter = EditorGUILayout.BeginScrollView(scrollPositionOuter, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            #region Properties Fields
            autoIncrementation = EditorGUILayout.Toggle("Use Auto Index", autoIncrementation);
            newName = EditorGUILayout.TextField("New Name", newName);
            #endregion

            GUILayout.Space(5);
            if (GUILayout.Button("Rename", GUILayout.Height(25)))
            {
                ApplyNewNameToSelectedObjects();
                Close();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private static void ApplyNewNameToSelectedObjects()
        {
            for (int i = 0; i < selectedGameObjects.Length; i++)
            {
                if (selectedGameObjects[i] != null)
                {
                    Undo.RecordObject(selectedGameObjects[i], "Rename GameObject");
                    string objectName = autoIncrementation ? $"{newName} ({i + 1})" : newName;
                    selectedGameObjects[i].name = objectName;
                    EditorUtility.SetDirty(selectedGameObjects[i]);
                }
            }
        }

        private void OnDestroy()
        {
            newName = null;
            selectedGameObjects = null;
        }
    }
}
#endif