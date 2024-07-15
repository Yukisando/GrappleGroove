#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [CustomEditor(typeof(HierarchyDesignerFolder))]
    public class HierarchyDesigner_Inspector_Folder : Editor
    {
        #region Properties
        private GUIStyle contentBackgroundGUIStyle;
        private bool showChildren = true;
        private float maxWidth = 0;
        #endregion

        #region Main
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (contentBackgroundGUIStyle == null) { contentBackgroundGUIStyle = HierarchyDesigner_Shared_GUI.CreateCustomStyle(2, new RectOffset(2, 2, 2, 2), new RectOffset(4, 4, 4, 4)); }
            float originalLabelWidth = EditorGUIUtility.labelWidth;
            float originalFieldWidth = EditorGUIUtility.fieldWidth;

            #region Runtime
            EditorGUIUtility.labelWidth = 90;
            EditorGUIUtility.fieldWidth = 100;

            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Hierarchy Designer's Folder", HierarchyDesigner_Shared_GUI.HeaderGUIStyle);
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("(Runtime)", HierarchyDesigner_Shared_GUI.InspectorHeaderGUIStyle);
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Settings:", HierarchyDesigner_Shared_GUI.InspectorContentGUIStyle);
            EditorGUILayout.Space(2);

            SerializedProperty flattenFolderProp = serializedObject.FindProperty("flattenFolder");
            EditorGUILayout.PropertyField(flattenFolderProp);
            if (flattenFolderProp.boolValue)
            {
                SerializedProperty flattenEventProp = serializedObject.FindProperty("flattenEvent");
                EditorGUILayout.PropertyField(flattenEventProp);
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Events:", HierarchyDesigner_Shared_GUI.InspectorContentGUIStyle);
                EditorGUILayout.Space(2);
                SerializedProperty onFlattenEventProp = serializedObject.FindProperty("OnFlattenEvent");
                EditorGUILayout.PropertyField(onFlattenEventProp);
                EditorGUILayout.Space(2);
                SerializedProperty onFolderDestroyProp = serializedObject.FindProperty("OnFolderDestroy");
                EditorGUILayout.PropertyField(onFolderDestroyProp);
            }
            EditorGUIUtility.labelWidth = originalLabelWidth;
            EditorGUIUtility.fieldWidth = originalFieldWidth;

            EditorGUILayout.EndVertical();
            #endregion

            #region Editor
            if (HierarchyDesigner_Configurable_AdvancedSettings.IncludeEditorUtilitiesForHierarchyDesignerRuntimeFolder)
            {
                EditorGUILayout.Space(2);

                HierarchyDesignerFolder folder = (HierarchyDesignerFolder)target;
                int totalChildCount = GetTotalChildCount(folder.transform);
                maxWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(folder.transform);

                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("(Editor)", HierarchyDesigner_Shared_GUI.InspectorHeaderGUIStyle);
                EditorGUILayout.Space(4);
                EditorGUILayout.LabelField("Folder Stats:", HierarchyDesigner_Shared_GUI.InspectorContentGUIStyle);
                EditorGUILayout.Space(2);
                EditorGUILayout.LabelField("This folder contains:", HierarchyDesigner_Shared_GUI.InspectorMessageBoldGUIStyle);
                EditorGUILayout.LabelField("- '" + totalChildCount.ToString() + "' gameObject children.", HierarchyDesigner_Shared_GUI.InspectorMessageItalicGUIStyle);
                EditorGUILayout.Space(2);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(11);
                if (totalChildCount > 0) { showChildren = EditorGUILayout.Foldout(showChildren, "GameObject's Children List"); }
                EditorGUILayout.EndHorizontal();
                if (showChildren)
                {
                    EditorGUI.indentLevel++;
                    DisplayChildNames(folder.transform);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
            }
            #endregion

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Editor Operations
        private int GetTotalChildCount(Transform parent)
        {
            int count = parent.childCount;
            foreach (Transform child in parent)
            {
                count += GetTotalChildCount(child);
            }
            return count;
        }

        private void DisplayChildNames(Transform parent)
        {
            foreach (Transform child in parent)
            {
                EditorGUILayout.BeginHorizontal();

                GUIStyle currentStyle = child.gameObject.activeSelf ? EditorStyles.label : HierarchyDesigner_Shared_GUI.InactiveLabelGUIStyle;
                EditorGUILayout.LabelField(child.name, currentStyle, GUILayout.Width(maxWidth));

                if (GUILayout.Button("Toggle", GUILayout.MinWidth(55), GUILayout.ExpandWidth(true)))
                {
                    Undo.RecordObject(child.gameObject, "Toggle Active State");
                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                }
                if (GUILayout.Button("Select", GUILayout.MinWidth(55), GUILayout.ExpandWidth(true)))
                {
                    EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
                    Selection.activeGameObject = child.gameObject;
                }
                if (GUILayout.Button("View In Scene", GUILayout.MinWidth(100), GUILayout.ExpandWidth(true)))
                {
                    GameObject originalSelection = Selection.activeGameObject;
                    Selection.activeGameObject = child.gameObject;
                    SceneView.FrameLastActiveSceneView();
                    Selection.activeGameObject = originalSelection;
                }
                if (GUILayout.Button("Delete", GUILayout.MinWidth(55), GUILayout.ExpandWidth(true)))
                {
                    Undo.DestroyObjectImmediate(child.gameObject);
                    EditorGUIUtility.ExitGUI();
                }

                EditorGUILayout.EndHorizontal();
                DisplayChildNames(child);
            }
        }
        #endregion
    }
}
#endif