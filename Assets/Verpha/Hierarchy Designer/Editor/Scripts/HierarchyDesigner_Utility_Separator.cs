#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Utility_Separator
    {
        #region Menu Items
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Create All Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateAllSeparators()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in HierarchyDesigner_Configurable_Separator.GetAllSeparatorData())
            {
                CreateSeparator(separator.Key);
            }
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Create Default Separator", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateDefaultSeparator()
        {
            CreateSeparator("Separator");
        }

        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Create Missing Separators", false, HierarchyDesigner_Shared_MenuItems.LayerZero)]
        public static void CreateMissingSeparators()
        {
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in HierarchyDesigner_Configurable_Separator.GetAllSeparatorData())
            {
                if (!SeparatorExists(separator.Key))
                {
                    CreateSeparator(separator.Key);
                }
            }
        }
        #endregion

        #region Methods
        private static void CreateSeparator(string separatorName)
        {
            GameObject separator = new GameObject($"//{separatorName}");
            separator.tag = "EditorOnly";
            SetSeparatorState(separator, false);
            separator.SetActive(false);

            Texture2D inspectorIcon = HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(separator, inspectorIcon);
            }

            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorName}");
        }

        public static void SetSeparatorState(GameObject gameObject, bool editable)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component) 
                {
                    component.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable; 
                }
            }

            gameObject.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            gameObject.transform.hideFlags = HideFlags.HideInInspector;
            EditorUtility.SetDirty(gameObject);
        }

        private static bool SeparatorExists(string separatorName)
        {
            string fullSeparatorName = "//" + separatorName;
            #if UNITY_6000_0_OR_NEWER
            Transform[] allTransforms = GameObject.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            #else
            Transform[] allTransforms = Object.FindObjectsOfType<Transform>(true);
            #endif

            foreach (Transform t in allTransforms)
            {
                if (t.gameObject.CompareTag("EditorOnly") && t.gameObject.name.Equals(fullSeparatorName))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
#endif