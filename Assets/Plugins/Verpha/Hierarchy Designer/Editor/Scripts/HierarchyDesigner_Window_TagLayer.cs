#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_TagLayer : EditorWindow
    {
        #region OnGUI Properties
        private GUIStyle customSettingsStyle;
        private GUIStyle contentLabelStyle;
        private string windowLabel;
        #endregion
        #region TagLayer Properties
        private bool isTag;
        private GameObject gameObject;
        #endregion

        public static void OpenWindow(GameObject gameObject, bool isTag, Vector2 position)
        {
            HierarchyDesigner_Window_TagLayer window = GetWindow<HierarchyDesigner_Window_TagLayer>("Hierarchy Designer Tag and Layer");
            window.minSize = new Vector2(300, 65);
            window.position = new Rect(position, new Vector2(200, 50));
            window.gameObject = gameObject;
            window.isTag = isTag;
            window.windowLabel = isTag ? "Hierarchy Tag" : "Hierarchy Layer";
        }

        private void InitializeStyles()
        {
            customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            contentLabelStyle = HierarchyDesigner_Info_OnGUI.ContentLabelStyle;
        }

        private void OnGUI()
        {
            InitializeStyles();
            bool cancelLayout = false;
            EditorGUILayout.BeginVertical(customSettingsStyle);
            
            GUILayout.Space(4);
            EditorGUILayout.LabelField(windowLabel, contentLabelStyle);
            GUILayout.Space(8);

            #region Tag and Layer 
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            if (isTag)
            {
                EditorGUILayout.LabelField("Choose a Tag",EditorStyles.boldLabel, GUILayout.Width(90));
                string newTag = EditorGUILayout.TagField(gameObject.tag);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(gameObject, "Change Tag");
                    gameObject.tag = newTag;
                    Close();
                }
            }
            else
            {
                EditorGUILayout.LabelField("Choose a Layer", EditorStyles.boldLabel, GUILayout.Width(100));
                int newLayer = EditorGUILayout.LayerField(gameObject.layer);
                if (EditorGUI.EndChangeCheck())
                {
                    bool shouldChangeLayer = true;

                    if (gameObject.transform.childCount > 0)
                    {
                        int result = AskToChangeChildrenLayers(gameObject, newLayer);
                        if (result == 2)
                        {
                            shouldChangeLayer = false;
                            cancelLayout = true;
                        }
                    }

                    if (shouldChangeLayer)
                    {
                        Undo.RecordObject(gameObject, "Change Layer");
                        gameObject.layer = newLayer;
                        Close();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            #endregion
            
            EditorGUILayout.EndVertical();

            if (cancelLayout) { return; }
        }

        private static int AskToChangeChildrenLayers(GameObject obj, int newLayer)
        {
            int option = EditorUtility.DisplayDialogComplex(
                           "Change Layer",
                           $"Do you want to set the layer to '{LayerMask.LayerToName(newLayer)}' for all child objects as well?",
                           "Yes, change children",
                           "No, this object only",
                           "Cancel"
                       );

            if (option == 0)
            {
                SetLayerRecursively(obj, newLayer);
            }

            return option;
        }

        private static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            foreach (Transform child in obj.transform)
            {
                Undo.RecordObject(child.gameObject, "Change Layer");
                child.gameObject.layer = newLayer;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
#endif