#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Separator : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private Vector2 innerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle messageGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private readonly int[] fontSizeOptions = new int[15];
        private const float separatorCreationLabelWidth = 130;
        private const float extraSeparatorLabelWidthOffset = 10;
        #endregion
        #region Temporary Values
        private Dictionary<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> tempSeparators;
        private static bool hasModifiedChanges = false;
        #endregion
        #region Separator Creation Values
        private string newSeparatorName = "";
        private Color newTextColor = Color.white;
        private Color newBackgroundColor = Color.gray;
        private int newFontSize = 12;
        private FontStyle newFontStyle = FontStyle.Normal;
        private TextAnchor newTextAnchor = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType newImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
        #endregion
        #region Separator Global Fields Values
        private Color tempGlobalTextColor = Color.white;
        private Color tempGlobalBackgroundColor = Color.gray;
        private int tempGlobalFontSize = 12;
        private FontStyle tempGlobalFontStyle = FontStyle.Normal;
        private TextAnchor tempGlobalTextAnchor = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Configurable_Separator.SeparatorImageType tempGlobalImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
        #endregion
        #endregion

        #region Menu Item
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Separator + "/Separator Manager Window", false, HierarchyDesigner_Shared_MenuItems.LayerTwo)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_Separator window = GetWindow<HierarchyDesigner_Window_Separator>("Hierarchy Separator Manager");
            window.minSize = new Vector2(400, 200);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            InitFontSizeOptions();
            LoadTempValues();
        }

        private void InitFontSizeOptions()
        {
            for (int i = 0; i < fontSizeOptions.Length; i++)
            {
                fontSizeOptions[i] = 7 + i;
            }
        }

        private void LoadTempValues()
        {
            tempSeparators = HierarchyDesigner_Configurable_Separator.GetAllSeparatorData();
        }
        #endregion

        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out messageGUIStyle, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Separators Manager", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Separator Creation
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Separator Creation:", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(separatorCreationLabelWidth))
            {
                newSeparatorName = EditorGUILayout.TextField("Name", newSeparatorName);
                newTextColor = EditorGUILayout.ColorField("Text Color", newTextColor);
                newBackgroundColor = EditorGUILayout.ColorField("Background Color", newBackgroundColor);
                string[] newFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                int newFontSizeIndex = Array.IndexOf(fontSizeOptions, newFontSize);
                newFontSize = fontSizeOptions[EditorGUILayout.Popup("Font Size", newFontSizeIndex, newFontSizeOptionsStrings)];
                newFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", newFontStyle);
                newTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup("Text Anchor", newTextAnchor);
                newImageType = (HierarchyDesigner_Configurable_Separator.SeparatorImageType)EditorGUILayout.EnumPopup("Background Type", newImageType);
            }
            GUILayout.Space(4);
            if (GUILayout.Button("Create Separator", GUILayout.Height(25)))
            {
                if (IsSeparatorNameValid(newSeparatorName))
                {
                    CreateSeparator(newSeparatorName, newTextColor, newBackgroundColor, newFontSize, newFontStyle, newTextAnchor, newImageType);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Separator Name", "Separator name is either duplicate or invalid.", "OK");
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            if (tempSeparators.Count > 0)
            {
                #region Global Fields
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Separators' Global Fields", contentGUIStyle);
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                tempGlobalTextColor = EditorGUILayout.ColorField(tempGlobalTextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalSeparatorTextColor(tempGlobalTextColor);
                }
                EditorGUI.BeginChangeCheck();
                tempGlobalBackgroundColor = EditorGUILayout.ColorField(tempGlobalBackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalSeparatorBackgroundColor(tempGlobalBackgroundColor);
                }
                EditorGUI.BeginChangeCheck();
                string[] tempFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                int tempFontSizeIndex = Array.IndexOf(fontSizeOptions, tempGlobalFontSize);
                tempGlobalFontSize = fontSizeOptions[EditorGUILayout.Popup(tempFontSizeIndex, tempFontSizeOptionsStrings, GUILayout.Width(50))];
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalSeparatorFontSize(tempGlobalFontSize);
                }
                EditorGUI.BeginChangeCheck();
                tempGlobalFontStyle = (FontStyle)EditorGUILayout.EnumPopup(tempGlobalFontStyle, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalSeparatorFontStyle(tempGlobalFontStyle);
                }
                EditorGUI.BeginChangeCheck();
                tempGlobalTextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(tempGlobalTextAnchor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalSeparatorTextAnchor(tempGlobalTextAnchor);
                }
                EditorGUI.BeginChangeCheck();
                tempGlobalImageType = (HierarchyDesigner_Configurable_Separator.SeparatorImageType)EditorGUILayout.EnumPopup(tempGlobalImageType, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalSeparatorBackgroundType(tempGlobalImageType);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                #endregion

                #region Separator List
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Separators' List", contentGUIStyle);
                innerScroll = EditorGUILayout.BeginScrollView(innerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                GUILayout.Space(10);
                foreach (var separator in tempSeparators)
                {
                    EditorGUI.BeginChangeCheck();
                    DrawSeparator(separator.Key, separator.Value);
                    if (EditorGUI.EndChangeCheck())
                    {
                        hasModifiedChanges = true;
                    }
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                #endregion
            }
            else
            {
                EditorGUILayout.LabelField("No separators found. Please create a new separator.", messageGUIStyle);
            }
            EditorGUILayout.EndVertical();
            #endregion

            #region Footer
            if (GUILayout.Button("Update and Save Settings", GUILayout.Height(30)))
            {
                SaveSettings();
            }
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void OnDestroy()
        {
            if (hasModifiedChanges)
            {
                bool shouldSave = EditorUtility.DisplayDialog("Separator(s) Have Been Modified",
                    "Do you want to save the changes you made to the separators?",
                    "Save", "Don't Save");

                if (shouldSave)
                {
                    SaveSettings();
                }
            }
            hasModifiedChanges = false;
        }

        private void SaveSettings()
        {
            HierarchyDesigner_Configurable_Separator.SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearSeparatorCache();
            hasModifiedChanges = false;
        }

        #region Operations
        private bool IsSeparatorNameValid(string separatorName)
        {
            return !string.IsNullOrEmpty(separatorName) && !tempSeparators.ContainsKey(separatorName);
        }

        private void CreateSeparator(string separatorName, Color textColor, Color backgroundColor, int fontSize, FontStyle fontStyle, TextAnchor textAnchor, HierarchyDesigner_Configurable_Separator.SeparatorImageType imageType)
        {
            HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData newSeparatorData = new HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData
            {
                Name = separatorName,
                TextColor = textColor,
                BackgroundColor = backgroundColor,
                FontSize = fontSize,
                FontStyle = fontStyle,
                TextAnchor = textAnchor,
                ImageType = imageType,

            };
            tempSeparators[separatorName] = newSeparatorData;
            HierarchyDesigner_Configurable_Separator.SetSeparatorData(separatorName, textColor, backgroundColor, fontSize, fontStyle, textAnchor, imageType);
            tempSeparators = HierarchyDesigner_Configurable_Separator.GetAllSeparatorData();
            newSeparatorName = "";
            newTextColor = Color.white;
            newBackgroundColor = Color.gray;
            newFontSize = 12;
            newFontStyle = FontStyle.Normal;
            newTextAnchor = TextAnchor.MiddleCenter;
            newImageType = HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default;
            hasModifiedChanges = true;
            GUI.FocusControl(null);
        }

        private void DrawSeparator(string key, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData)
        {
            float separatorLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(tempSeparators.Keys);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(separatorData.Name, GUILayout.Width(separatorLabelWidth + extraSeparatorLabelWidthOffset));
            separatorData.TextColor = EditorGUILayout.ColorField(separatorData.TextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            separatorData.BackgroundColor = EditorGUILayout.ColorField(separatorData.BackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int fontSizeIndex = Array.IndexOf(fontSizeOptions, separatorData.FontSize);
            if (fontSizeIndex == -1) { fontSizeIndex = 5; }
            separatorData.FontSize = fontSizeOptions[EditorGUILayout.Popup(fontSizeIndex, fontSizeOptionsStrings, GUILayout.Width(50))];
            separatorData.FontStyle = (FontStyle)EditorGUILayout.EnumPopup(separatorData.FontStyle, GUILayout.Width(100));
            separatorData.TextAnchor = (TextAnchor)EditorGUILayout.EnumPopup(separatorData.TextAnchor, GUILayout.Width(125));
            separatorData.ImageType = (HierarchyDesigner_Configurable_Separator.SeparatorImageType)EditorGUILayout.EnumPopup(separatorData.ImageType, GUILayout.Width(150));

            if (GUILayout.Button("Create", GUILayout.Width(60)))
            {
                CreateSeparatorGameObject(separatorData);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemoveSeparator(key);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CreateSeparatorGameObject(HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separatorData)
        {
            GameObject separator = new GameObject($"//{separatorData.Name}");
            separator.tag = "EditorOnly";
            HierarchyDesigner_Utility_Separator.SetSeparatorState(separator, false);
            separator.SetActive(false);
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorData.Name}");

            Texture2D inspectorIcon = HierarchyDesigner_Shared_Resources.SeparatorInspectorIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(separator, inspectorIcon);
            }
        }

        private void RemoveSeparator(string separatorName)
        {
            if (tempSeparators.ContainsKey(separatorName))
            {
                tempSeparators.Remove(separatorName);
                HierarchyDesigner_Configurable_Separator.RemoveSeparatorData(separatorName);
                GUIUtility.ExitGUI();
            }
        }

        private void UpdateGlobalSeparatorTextColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.TextColor = color;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorBackgroundColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.BackgroundColor = color;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorFontSize(int size)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.FontSize = size;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorFontStyle(FontStyle style)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.FontStyle = style;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorTextAnchor(TextAnchor anchor)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.TextAnchor = anchor;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalSeparatorBackgroundType(HierarchyDesigner_Configurable_Separator.SeparatorImageType imageType)
        {
            foreach (HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData separator in tempSeparators.Values)
            {
                separator.ImageType = imageType;
            }
            hasModifiedChanges = true;
        }
        #endregion
    }
}
#endif