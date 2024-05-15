#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Override_Inspector : Editor
    {
        #region Cached Properties
        private static Dictionary<GameObject, bool> separatorCache = new Dictionary<GameObject, bool>();
        private static readonly string separatorMessage = "Separators are EditorOnly, meaning they will not be present in your game's build. If you want a GameObject parent to organize your GameObjects, use a folder instead.";
        private static readonly string lockedGameObjectMessage = "This gameObject is locked, components are not available for editing.";
        #endregion

        static HierarchyDesigner_Override_Inspector()
        {
            finishedDefaultHeaderGUI -= OnPostHeaderGUI;
            finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static void OnPostHeaderGUI(Editor editor)
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && EditorApplication.isPlaying) return;
            if (!(editor.target is GameObject gameObject)) return;

            bool isSeparator = IsSeparatorCached(gameObject);
            bool isGameObjectLocked = HierarchyDesigner_Utility_Tools.IsGameObjectLocked(gameObject);

            if (isSeparator)
            {
                HandleSeparator(gameObject);
            }
            else if (isGameObjectLocked)
            {
                HandleLockedGameObject(gameObject);
            }
        }

        private static bool IsSeparatorCached(GameObject gameObject)
        {
            if (!separatorCache.TryGetValue(gameObject, out bool isSeparator))
            {
                isSeparator = HierarchyDesigner_Utility_Separator.IsSeparator(gameObject);
                separatorCache[gameObject] = isSeparator;
            }
            return isSeparator;
        }

        private static void HandleSeparator(GameObject gameObject)
        {
            EditorGUILayout.HelpBox(separatorMessage, MessageType.Info, true);
            gameObject.transform.hideFlags = HideFlags.HideInInspector;
        }

        private static void HandleLockedGameObject(GameObject gameObject)
        {
            EditorGUILayout.HelpBox(lockedGameObjectMessage, MessageType.Info, true);
        }
    }
}   
#endif