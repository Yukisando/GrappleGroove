#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_GUI
    {
        #region Properties
        private const float initialLabelWidth = 25f;
        private static GUIStyle headerGUIStyle = null;
        private static GUIStyle contentGUIStyle = null;
        private static GUIStyle messageGUIStyle = null;
        private static GUIStyle inspectorHeaderGUIStyleGUIStyle = null;
        private static GUIStyle inspectorContentGUIStyleGUIStyle = null;
        private static GUIStyle inspectorMessageItalicGUIStyle = null;
        private static GUIStyle inspectorMessageBoldGUIStyle = null;
        private static GUIStyle inactiveLabelGUIStyle = null;
        private static Dictionary<Color, GUIStyle> guiStyleCache = new Dictionary<Color, GUIStyle>();
        private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();
        #endregion

        #region Label
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
            float labelWidth = initialLabelWidth;
            GUIStyle labelStyle = GUI.skin.label;
            foreach (string name in names)
            {
                GUIContent content = new GUIContent(name);
                Vector2 size = labelStyle.CalcSize(content);
                if (size.x > labelWidth) labelWidth = size.x;
            }
            return labelWidth;
        }

        public static float CalculateMaxLabelWidth(Transform parent)
        {
            float maxWidth = 0;
            GatherChildNamesAndCalculateMaxWidth(parent, ref maxWidth);
            return maxWidth + 18f;
        }

        private static void GatherChildNamesAndCalculateMaxWidth(Transform parent, ref float maxWidth)
        {
            GUIStyle labelStyle = GUI.skin.label;
            foreach (Transform child in parent)
            {
                GUIContent content = new GUIContent(child.name);
                Vector2 size = labelStyle.CalcSize(content);
                if (size.x > maxWidth) maxWidth = size.x;
                GatherChildNamesAndCalculateMaxWidth(child, ref maxWidth);
            }
        }
        #endregion

        #region GUIStyles
        public static GUIStyle HeaderGUIStyle
        {
            get
            {
                if (headerGUIStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        headerGUIStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 18,
                            fontStyle = FontStyle.Normal,
                            alignment = TextAnchor.MiddleCenter,
                            fixedHeight = 27,
                            normal = { background = GetOrCreateTexture(2, 2, HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorBackgroundColor()) }
                        };
                    }
                }
                return headerGUIStyle;
            }
        }

        public static GUIStyle ContentGUIStyle
        {
            get
            {
                if (contentGUIStyle == null)
                {
                    if (EditorStyles.boldLabel != null)
                    {
                        contentGUIStyle = new GUIStyle(EditorStyles.boldLabel)
                        {
                            fontSize = 16,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft,
                        };
                    }
                }
                return contentGUIStyle;
            }
        }

        public static GUIStyle MessageGUIStyle
        {
            get
            {
                if (messageGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        messageGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 13,
                            fontStyle = FontStyle.Italic,
                        };
                    }
                }
                return messageGUIStyle;
            }
        }

        public static GUIStyle InspectorHeaderGUIStyle
        {
            get
            {
                if (inspectorHeaderGUIStyleGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorHeaderGUIStyleGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 16,
                            fontStyle = FontStyle.Normal,
                            alignment = TextAnchor.MiddleLeft
                        };
                    }
                }
                return inspectorHeaderGUIStyleGUIStyle;
            }
        }

        public static GUIStyle InspectorContentGUIStyle
        {
            get
            {
                if (inspectorContentGUIStyleGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorContentGUIStyleGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 14,
                            fontStyle = FontStyle.Bold,
                            alignment = TextAnchor.MiddleLeft
                        };
                    }
                }
                return inspectorContentGUIStyleGUIStyle;
            }
        }

        public static GUIStyle InspectorMessageBoldGUIStyle
        {
            get
            {
                if (inspectorMessageBoldGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorMessageBoldGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Bold,
                        };
                    }
                }
                return inspectorMessageBoldGUIStyle;
            }
        }

        public static GUIStyle InspectorMessageItalicGUIStyle
        {
            get
            {
                if (inspectorMessageItalicGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inspectorMessageItalicGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                            fontStyle = FontStyle.Italic,
                        };
                    }
                }
                return inspectorMessageItalicGUIStyle;
            }
        }

        public static GUIStyle InactiveLabelGUIStyle
        {
            get
            {
                if (inactiveLabelGUIStyle == null)
                {
                    if (EditorStyles.label != null)
                    {
                        inactiveLabelGUIStyle = new GUIStyle(EditorStyles.label)
                        {
                            fontSize = 12,
                        };
                        Color textColor = inactiveLabelGUIStyle.normal.textColor;
                        textColor.a = 0.5f;
                        inactiveLabelGUIStyle.normal.textColor = textColor;
                    }
                }
                return inactiveLabelGUIStyle;
            }
        }

        public static void DrawGUIStyles(out GUIStyle headerGUIStyle, out GUIStyle contentGUIStyle, out GUIStyle messageGUIStyle,  out GUIStyle outerBackgroundGUIStyle, out GUIStyle innerBackgroundGUIStyle, out GUIStyle contentBackgroundGUIStyle)
        {
            headerGUIStyle = HeaderGUIStyle;
            contentGUIStyle = ContentGUIStyle;
            messageGUIStyle = MessageGUIStyle;
            outerBackgroundGUIStyle = CreateCustomStyle(0);
            innerBackgroundGUIStyle = CreateCustomStyle(1, new RectOffset(4, 4, 4, 4), new RectOffset(5, 5, 5, 5));
            contentBackgroundGUIStyle = CreateCustomStyle(2, new RectOffset(2, 2, 2, 2), new RectOffset(5, 5, 5, 5));
        }

        public static GUIStyle CreateCustomStyle(int backgroundNumber = 0, RectOffset margin = null, RectOffset padding = null)
        {
            Color backgroundColor;
            switch (backgroundNumber)
            {
                case 0:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetOuterGUIColor();
                    break;
                case 1:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetInnerGUIColor();
                    break;
                case 2:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetContentGUIColor();
                    break;
                default:
                    backgroundColor = HierarchyDesigner_Shared_ColorUtility.GetDefaultEditorBackgroundColor();
                    break;
            }

            if (guiStyleCache.TryGetValue(backgroundColor, out GUIStyle cachedStyle) && cachedStyle.normal.background != null)
            {
                Color cachedColor = cachedStyle.normal.background.GetPixel(0, 0);
                if (cachedColor == backgroundColor)
                {
                    return cachedStyle;
                }
            }

            margin ??= new RectOffset(4, 4, 4, 4);
            padding ??= new RectOffset(2, 2, 4, 4);

            GUIStyle newStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { background = GetOrCreateTexture(2, 2, backgroundColor) },
                stretchHeight = true,
                margin = margin,
                padding = padding
            };

            guiStyleCache[backgroundColor] = newStyle;
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

            newTexture.hideFlags = HideFlags.DontSave;
            textureCache[color] = newTexture;
            return newTexture;
        }
        #endregion

        #region GUILayout
        public static bool DrawToggle(string label, float labelWidth, bool currentValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            bool newValue = EditorGUILayout.Toggle(currentValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static T DrawEnumPopup<T>(string label, float labelWidth, T selectedValue) where T : Enum
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            T newValue = (T)EditorGUILayout.EnumPopup(selectedValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        public static int DrawMaskField(string label, float labelWidth, int mask, string[] options)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(105));
            int newMask = EditorGUILayout.MaskField(mask, options);
            EditorGUILayout.EndHorizontal();
            return newMask;
        }
        #endregion
    }
}
#endif