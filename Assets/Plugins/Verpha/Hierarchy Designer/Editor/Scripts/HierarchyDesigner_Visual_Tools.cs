#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Visual_Tools
    {
        #region Properties
        private static readonly float lockIconOffset = 20f;
        private static readonly float lockIconSize = 17f;
        private static readonly string lockedLabel = "(Locked)";
        private static readonly GUIStyle lockLabelStyle = new GUIStyle
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            fontSize = 11,
            normal = { textColor = Color.white }
        };
        private static Dictionary<int, GameObject> gameObjectCache = new Dictionary<int, GameObject>();
        #endregion

        static HierarchyDesigner_Visual_Tools()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

        private static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && HierarchyDesigner_Shared_EditorState.IsPlaying) return;
            if (Event.current.type != EventType.Repaint) return;

            if (!gameObjectCache.TryGetValue(instanceID, out GameObject gameObject))
            {
                gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
                if (gameObject == null) return;

                gameObjectCache[instanceID] = gameObject;
            }

            if (gameObject == null || !gameObject)
            {
                gameObjectCache.Remove(instanceID);
                return;
            }

            if (HierarchyDesigner_Utility_Separator.IsSeparator(gameObject)) return;

            if ((gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable)
            {
                DrawLockIcon(gameObject, selectionRect);
            }
        }

        private static void DrawLockIcon(GameObject gameObject, Rect selectionRect)
        {
            GUIStyle style = GUI.skin.label;
            GUIContent content = new GUIContent(gameObject.name);
            float textWidth = style.CalcSize(content).x;
            Rect lockRect = CalculateLockIconRect(selectionRect, textWidth);
            DrawLockLabel(lockRect);
        }

        private static Rect CalculateLockIconRect(Rect selectionRect, float textWidth)
        {
            float iconOffset = textWidth + lockIconOffset;
            float iconYPosition = selectionRect.y + (selectionRect.height - lockIconSize) / 2;
            return new Rect(selectionRect.x + iconOffset, iconYPosition, selectionRect.height, selectionRect.height);
        }

        private static void DrawLockLabel(Rect rect)
        {
            GUI.Label(rect, lockedLabel, lockLabelStyle);
        }

        public static void Cleanup()
        {
            gameObjectCache.Clear();
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}
#endif