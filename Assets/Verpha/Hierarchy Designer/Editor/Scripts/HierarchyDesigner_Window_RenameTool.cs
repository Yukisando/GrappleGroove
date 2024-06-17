#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_RenameTool : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private const float fieldsWidth = 110;
        #endregion
        #region Rename Tool Values
        private static string newName;
        [SerializeField] private List<GameObject> selectedGameObjects = new List<GameObject>();
        private static bool automaticIndexing = true;
        private ReorderableList reorderableList;
        #endregion
        #endregion

        #region Window
        public static void OpenWindow(List<GameObject> gameObjects, bool autoIndex = true)
        {
            HierarchyDesigner_Window_RenameTool window = GetWindow<HierarchyDesigner_Window_RenameTool>("Rename Tool");
            Vector2 size = new Vector2(400, 200);
            window.minSize = size;
            
            automaticIndexing = autoIndex;
            window.selectedGameObjects = gameObjects ?? new List<GameObject>();
            newName = "";
            window.InitializeReorderableList();
        }
        #endregion

        #region Initialization
        private void InitializeReorderableList()
        {
            reorderableList = new ReorderableList(selectedGameObjects, typeof(GameObject), true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Selected GameObjects");
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    selectedGameObjects[index] = (GameObject)EditorGUI.ObjectField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), selectedGameObjects[index], typeof(GameObject), true);
                },
                onAddCallback = (ReorderableList list) =>
                {
                    selectedGameObjects.Add(null);
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    selectedGameObjects.RemoveAt(list.index);
                }
            };
        }
        #endregion

        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out GUIStyle _, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Rename Tool", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(fieldsWidth))
            {
                newName = EditorGUILayout.TextField("New Name", newName);
                automaticIndexing = HierarchyDesigner_Shared_GUI.DrawToggle("Use Auto-Index", fieldsWidth, automaticIndexing);
            }
            GUILayout.Space(4);
            if (reorderableList != null) { reorderableList.DoLayoutList(); }
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            #region Footer
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reassign Selected GameObjects", GUILayout.Height(25)))
            {
                ReassignSelectedGameObjects();
            }
            if (GUILayout.Button("Clear Selected GameObjects", GUILayout.Height(25)))
            {
                ClearSelectedGameObjects();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Rename Selected GameObjects", GUILayout.Height(30)))
            {
                RenameSelectedGameObjects();
                Close();
            }
            #endregion

            EditorGUILayout.EndVertical();
        }

        private void OnDestroy()
        {
            newName = null;
            selectedGameObjects = null;
            reorderableList = null;
        }

        #region Operations Methods
        private void ReassignSelectedGameObjects()
        {
            selectedGameObjects = new List<GameObject>(Selection.gameObjects);
            InitializeReorderableList();
        }

        private void ClearSelectedGameObjects()
        {
            selectedGameObjects.Clear();
            InitializeReorderableList();
        }

        private void RenameSelectedGameObjects()
        {
            if (selectedGameObjects == null) return;

            for (int i = 0; i < selectedGameObjects.Count; i++)
            {
                if (selectedGameObjects[i] != null)
                {
                    Undo.RecordObject(selectedGameObjects[i], "Rename GameObject");
                    string objectName = automaticIndexing ? $"{newName} ({i + 1})" : newName;
                    selectedGameObjects[i].name = objectName;
                    EditorUtility.SetDirty(selectedGameObjects[i]);
                }
            }
        }
        #endregion
    }
}
#endif