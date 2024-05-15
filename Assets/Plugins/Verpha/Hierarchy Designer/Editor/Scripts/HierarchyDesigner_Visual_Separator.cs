#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Visual_Separator
    {
        #region Cached Properties
        private static readonly (Color textColor, Color backgroundColor, FontStyle fontStyle, int fontSize, TextAnchor textAlignment, HierarchyDesigner_Info_Separator.BackgroundImageType imageType) defaultSeparatorInfo = (Color.white, Color.gray, FontStyle.Normal, 12, TextAnchor.MiddleCenter, HierarchyDesigner_Info_Separator.BackgroundImageType.Classic);
        private static Dictionary<int, WeakReference<GameObject>> separatorCache = new Dictionary<int, WeakReference<GameObject>>();
        private static Dictionary<string, (Color textColor, Color backgroundColor, FontStyle fontStyle, int fontSize, TextAnchor textAlignment, HierarchyDesigner_Info_Separator.BackgroundImageType imageType)> separatorInfoCache = new Dictionary<string, (Color, Color, FontStyle, int, TextAnchor, HierarchyDesigner_Info_Separator.BackgroundImageType)>();
        #endregion

        static HierarchyDesigner_Visual_Separator()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
            UpdateSeparatorVisuals();
        }

        private static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && HierarchyDesigner_Shared_EditorState.IsPlaying) return;
            if (Event.current.type != EventType.Repaint) return;
            if (!TryGetGameObject(instanceID, out GameObject gameObject)) return;

            string separatorKey = gameObject.name.Replace("//", "").Trim();
            (Color textColor, Color backgroundColor, FontStyle fontStyle, int fontSize, TextAnchor textAlignment, HierarchyDesigner_Info_Separator.BackgroundImageType imageType) separatorInfo = GetSeparatorInfo(separatorKey);
            DrawSeparator(selectionRect, separatorInfo, instanceID, separatorKey);
        }

        private static bool TryGetGameObject(int instanceID, out GameObject gameObject)
        {
            if (separatorCache.TryGetValue(instanceID, out WeakReference<GameObject> weakRef) && weakRef.TryGetTarget(out gameObject))
            {
                return IsSeparator(gameObject);
            }
            gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null && IsSeparator(gameObject))
            {
                separatorCache[instanceID] = new WeakReference<GameObject>(gameObject);
                return true;
            }
            return false;
        }

        private static bool IsSeparator(GameObject gameObject)
        {
            if (gameObject == null) return false;
            return gameObject.tag == "EditorOnly" && gameObject.name.StartsWith("//");
        }

        private static void DrawSeparator(Rect selectionRect, (Color textColor, Color backgroundColor, FontStyle fontStyle, int fontSize, TextAnchor textAlignment, HierarchyDesigner_Info_Separator.BackgroundImageType imageType) separatorInfo, int instanceID, string separatorKey)
        {
            GUI.color = HierarchyDesigner_Shared_ColorUtility.GetEditorBackgroundColor();
            GUI.DrawTexture(new Rect(32, selectionRect.y, EditorGUIUtility.currentViewWidth, selectionRect.height), EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;

            selectionRect.x = 32;
            selectionRect.width = EditorGUIUtility.currentViewWidth;

            if (!separatorInfoCache.TryGetValue(separatorKey, out var info))
            {
                info = defaultSeparatorInfo;
            }

            GUIStyle textStyle = new GUIStyle
            {
                alignment = info.textAlignment,
                fontSize = info.fontSize,
                fontStyle = info.fontStyle,
                normal = { textColor = info.textColor }
            };

            Texture2D backgroundTexture = HierarchyDesigner_Manager_Background.GetBackgroundImage(info.imageType);

            GUI.color = info.backgroundColor;
            GUI.DrawTexture(selectionRect, backgroundTexture);
            GUI.color = Color.white;

            Rect textRect = AdjustRect(selectionRect, info.textAlignment);
            GUI.Label(textRect, separatorKey, textStyle);
        }

        private static Rect AdjustRect(Rect rect, TextAnchor textAlignment)
        {
            switch (textAlignment)
            {
                case TextAnchor.MiddleLeft:
                case TextAnchor.UpperLeft:
                case TextAnchor.LowerLeft:
                    rect.x += 3;
                    break;
                case TextAnchor.MiddleRight:
                case TextAnchor.UpperRight:
                case TextAnchor.LowerRight:
                    rect.x -= 36;
                    break;
            }
            return rect;
        }

        private static (Color textColor, Color backgroundColor, FontStyle fontStyle, int fontSize, TextAnchor textAlignment, HierarchyDesigner_Info_Separator.BackgroundImageType imageType) GetSeparatorInfo(string separatorName)
        {
            if (!separatorInfoCache.TryGetValue(separatorName, out var info))
            {
                if (HierarchySeparatorWindow.separators.TryGetValue(separatorName, out HierarchyDesigner_Info_Separator separator))
                {
                    info = (separator.TextColor, separator.BackgroundColor, separator.FontStyle, separator.FontSize, separator.TextAlignment, separator.ImageType);
                    separatorInfoCache[separatorName] = info;
                }
                else
                {
                    info = defaultSeparatorInfo;
                }
            }
            return info;
        }

        public static void UpdateSeparatorVisuals()
        {
            if (HierarchyDesigner_Data_Separator.separators.Count > 0)
            {
                separatorInfoCache.Clear();
                foreach (var separator in HierarchyDesigner_Data_Separator.separators)
                {
                    separatorInfoCache[separator.Key] = (separator.Value.TextColor, separator.Value.BackgroundColor, separator.Value.FontStyle, separator.Value.FontSize, separator.Value.TextAlignment, separator.Value.ImageType);
                }
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}
#endif