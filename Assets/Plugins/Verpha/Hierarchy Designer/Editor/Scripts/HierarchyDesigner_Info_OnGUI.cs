#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Info_OnGUI
    {
        #region Styling Properties
        private static Dictionary<Color, GUIStyle> styleCache = new Dictionary<Color, GUIStyle>();
        private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();
        private static GUIStyle headerLabelStyle = null;
        private static GUIStyle contentLabelStyle = null;
        private static GUIStyle utilityLabelStyle = null;
        #endregion

        #region Styling Methods
        public static GUIStyle CreateCustomStyle(bool isSecondary = false)
        {
            Color backgroundColor;
            if (isSecondary)
            {
                backgroundColor = EditorGUIUtility.isProSkin ? HierarchyDesigner_Shared_ColorUtility.BackgroundColorSecondaryDarkTheme : HierarchyDesigner_Shared_ColorUtility.BackgroundColorSecondaryLightTheme;
            }
            else
            {
                backgroundColor = EditorGUIUtility.isProSkin ? HierarchyDesigner_Shared_ColorUtility.BackgroundColorDarkTheme : HierarchyDesigner_Shared_ColorUtility.BackgroundColorLightTheme;
            }

            if (styleCache.TryGetValue(backgroundColor, out var cachedStyle) && cachedStyle.normal.background != null)
            {
                Color cachedColor = cachedStyle.normal.background.GetPixel(0, 0);
                if (cachedColor == backgroundColor)
                {
                    return cachedStyle;
                }
            }

            GUIStyle newStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { background = GetOrCreateTexture(2, 2, backgroundColor) },
                stretchHeight = true
            };

            styleCache[backgroundColor] = newStyle;
            return newStyle;
        }

        private static Texture2D GetOrCreateTexture(int width, int height, Color color)
        {
            if (textureCache.TryGetValue(color, out Texture2D existingTexture) && existingTexture != null)
            {
                return existingTexture;
            }

            Texture2D newTexture = new Texture2D(width, height);
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }

            newTexture.SetPixels(pix);
            newTexture.Apply();

            textureCache[color] = newTexture;
            return newTexture;
        }

        public static GUIStyle HeaderLabelStyle
        {
            get
            {
                if (headerLabelStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        headerLabelStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 16,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft,
                        };
                    }
                }
                return headerLabelStyle;
            }
        }

        public static GUIStyle ContentLabelStyle
        {
            get
            {
                if (contentLabelStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        contentLabelStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 14,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft,
                        };
                    }
                }
                return contentLabelStyle;
            }
        }

        public static GUIStyle UtilityLabelStyle
        {
            get
            {
                if (utilityLabelStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        utilityLabelStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft,
                        };
                    }
                }
                return utilityLabelStyle;
            }
        }
        #endregion

        #region Utilities Methods
        public struct LabelWidth : IDisposable
        {
            private readonly float originalLabelWidth;

            public LabelWidth(float newLabelWidth)
            {
                originalLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = newLabelWidth;
            }

            public void Dispose()
            {
                EditorGUIUtility.labelWidth = originalLabelWidth;
            }
        }

        public static float CalculateMaxLabelWidth(IEnumerable<string> names)
        {
            float maxWidth = 25f;
            GUIStyle labelStyle = GUI.skin.label;
            foreach (string name in names)
            {
                GUIContent content = new GUIContent(name);
                Vector2 size = labelStyle.CalcSize(content);
                if (size.x > maxWidth) maxWidth = size.x;
            }
            return maxWidth;
        }
        #endregion
    }
}
#endif