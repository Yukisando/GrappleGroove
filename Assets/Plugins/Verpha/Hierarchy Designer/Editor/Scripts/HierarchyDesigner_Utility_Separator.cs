#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchySeparatorWindow : EditorWindow
    {
        #region OnGUI Properties
        private Vector2 scrollPositionOuter;
        private Vector2 scrollPositionInner;
        private static bool hasModifiedChanges = false;
        private GUIStyle customSettingsStyle;
        private GUIStyle customSettingsStyleSecondary;
        private GUIStyle headerLabelStyle;
        private GUIStyle contentLabelStyle;
        private GUIStyle utilityLabelStyle;
        #endregion
        #region Separator Fields
        private string newSeparatorName = "";
        private Color newSeparatorTextColor = Color.white;
        private Color newSeparatorBackgroundColor = Color.black;
        private FontStyle newFontStyle = FontStyle.Normal;
        private int newFontSize = 12;
        private readonly int[] fontSizeOptions = new int[15];
        private TextAnchor newTextAlignment = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Info_Separator.BackgroundImageType newBackgroundImageType = HierarchyDesigner_Info_Separator.BackgroundImageType.Classic;
        public static Dictionary<string, HierarchyDesigner_Info_Separator> separators = new Dictionary<string, HierarchyDesigner_Info_Separator>();
        #endregion
        #region Global Fields
        private Color tempGlobalTextColor = Color.white;
        private Color tempGlobalBackgroundColor = Color.black;
        private FontStyle tempGlobalFontStyle = FontStyle.Normal;
        private int tempGlobalFontSize = 12;
        private TextAnchor tempGlobalTextAlignment = TextAnchor.MiddleCenter;
        private HierarchyDesigner_Info_Separator.BackgroundImageType tempGlobalBackgroundImageType = HierarchyDesigner_Info_Separator.BackgroundImageType.Classic;
        #endregion

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Hierarchy Separator Manager")]
        public static void OpenWindow()
        {
            LoadSeparators();
            HierarchySeparatorWindow window = GetWindow<HierarchySeparatorWindow>("Hierarchy Separator Manager");
            window.minSize = new Vector2(400, 200);
        }

        private void OnEnable()
        {
            InitFontSizeOptions();
            EditorApplication.delayCall += CheckAndReloadData;
        }

        private void InitFontSizeOptions()
        {
            for (int i = 0; i < fontSizeOptions.Length; i++)
            {
                fontSizeOptions[i] = 7 + i;
            }
        }

        private static void LoadSeparators()
        {
            HierarchyDesigner_Data_Separator.LoadSeparators();
            separators = new Dictionary<string, HierarchyDesigner_Info_Separator>(HierarchyDesigner_Data_Separator.separators);
        }

        private static void CheckAndReloadData()
        {
            if (separators == null || separators.Count == 0) { LoadSeparators(); }
        }

        private void InitializeStyles()
        {
            customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            customSettingsStyleSecondary = HierarchyDesigner_Info_OnGUI.CreateCustomStyle(true);
            headerLabelStyle = HierarchyDesigner_Info_OnGUI.HeaderLabelStyle;
            contentLabelStyle = HierarchyDesigner_Info_OnGUI.ContentLabelStyle;
            utilityLabelStyle = HierarchyDesigner_Info_OnGUI.UtilityLabelStyle;
        }

        private void OnGUI()
        {
            InitializeStyles();
            EditorGUILayout.BeginVertical(customSettingsStyle);

            GUILayout.Space(4);
            EditorGUILayout.LabelField("Separators Manager", headerLabelStyle);
            GUILayout.Space(8);

            scrollPositionOuter = EditorGUILayout.BeginScrollView(scrollPositionOuter, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            #region Separator Creation Fields
            using (new HierarchyDesigner_Info_OnGUI.LabelWidth(150))
            {
                newSeparatorName = EditorGUILayout.TextField("Name", newSeparatorName);
                newSeparatorTextColor = EditorGUILayout.ColorField("Text Color", newSeparatorTextColor);
                newSeparatorBackgroundColor = EditorGUILayout.ColorField("Background Color", newSeparatorBackgroundColor);
                newFontStyle = (FontStyle)EditorGUILayout.EnumPopup("Font Style", newFontStyle);
                string[] newFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                int newFontSizeIndex = Array.IndexOf(fontSizeOptions, newFontSize);
                newFontSize = fontSizeOptions[EditorGUILayout.Popup("Font Size", newFontSizeIndex, newFontSizeOptionsStrings)];
                newTextAlignment = (TextAnchor)EditorGUILayout.EnumPopup("Text Alignment", newTextAlignment);
                newBackgroundImageType = (HierarchyDesigner_Info_Separator.BackgroundImageType)EditorGUILayout.EnumPopup("Background Image Type", newBackgroundImageType);
            }
            #endregion

            #region Add Separator
            GUILayout.Space(10);
            if (GUILayout.Button("Add Separator", GUILayout.Height(25)))
            {
                if (IsSeparatorNameValid(newSeparatorName))
                {
                    HierarchyDesigner_Info_Separator newSeparator = (new HierarchyDesigner_Info_Separator(newSeparatorName, newSeparatorTextColor, newSeparatorBackgroundColor, newFontStyle, newFontSize, newTextAlignment, newBackgroundImageType));
                    separators[newSeparatorName] = newSeparator;
                    newSeparatorName = "";
                    GUI.FocusControl(null);
                    hasModifiedChanges = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Separator Name", "Separator name is either duplicate or invalid.", "OK");
                }
            }
            GUILayout.Space(5);
            #endregion

            #region Separators List
            float maxWidth = HierarchyDesigner_Info_OnGUI.CalculateMaxLabelWidth(separators.Keys);

            if (separators.Count > 0)
            {
                #region Global Fields
                GUILayout.Space(2);
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Separators’ Global Fields", utilityLabelStyle, GUILayout.ExpandWidth(true));
                GUILayout.Space(6);

                EditorGUILayout.BeginHorizontal();
                #region Changes Check
                EditorGUI.BeginChangeCheck();
                tempGlobalTextColor = EditorGUILayout.ColorField(tempGlobalTextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (HierarchyDesigner_Info_Separator separator in separators.Values)
                    {
                        separator.TextColor = tempGlobalTextColor;
                    }
                    hasModifiedChanges = true;
                }

                EditorGUI.BeginChangeCheck();
                tempGlobalBackgroundColor = EditorGUILayout.ColorField(tempGlobalBackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (HierarchyDesigner_Info_Separator separator in separators.Values)
                    {
                        separator.BackgroundColor = tempGlobalBackgroundColor;
                    }
                    hasModifiedChanges = true;
                }

                EditorGUI.BeginChangeCheck();
                tempGlobalFontStyle = (FontStyle)EditorGUILayout.EnumPopup(tempGlobalFontStyle, GUILayout.Width(100));
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (HierarchyDesigner_Info_Separator separator in separators.Values)
                    {
                        separator.FontStyle = tempGlobalFontStyle;
                    }
                    hasModifiedChanges = true;
                }

                EditorGUI.BeginChangeCheck();
                string[] tempFontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                int tempFontSizeIndex = Array.IndexOf(fontSizeOptions, tempGlobalFontSize);
                tempGlobalFontSize = fontSizeOptions[EditorGUILayout.Popup(tempFontSizeIndex, tempFontSizeOptionsStrings, GUILayout.Width(40))];
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (HierarchyDesigner_Info_Separator separator in separators.Values)
                    {
                        separator.FontSize = tempGlobalFontSize;
                    }
                    hasModifiedChanges = true;
                }

                EditorGUI.BeginChangeCheck();
                tempGlobalTextAlignment = (TextAnchor)EditorGUILayout.EnumPopup(tempGlobalTextAlignment, GUILayout.Width(110));
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (HierarchyDesigner_Info_Separator separator in separators.Values)
                    {
                        separator.TextAlignment = tempGlobalTextAlignment;
                    }
                    hasModifiedChanges = true;
                }

                EditorGUI.BeginChangeCheck();
                tempGlobalBackgroundImageType = (HierarchyDesigner_Info_Separator.BackgroundImageType)EditorGUILayout.EnumPopup(tempGlobalBackgroundImageType, GUILayout.Width(100));
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (HierarchyDesigner_Info_Separator separator in separators.Values)
                    {
                        separator.ImageType = tempGlobalBackgroundImageType;
                    }
                    hasModifiedChanges = true;
                }
                #endregion
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUILayout.Space(10);
                #endregion

                EditorGUILayout.BeginVertical(customSettingsStyleSecondary);

                GUILayout.Space(2);
                EditorGUILayout.LabelField("Separators’ List", contentLabelStyle);
                GUILayout.Space(10);

                scrollPositionInner = EditorGUILayout.BeginScrollView(scrollPositionInner, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                foreach (var separatorEntry in separators)
                {
                    string key = separatorEntry.Key;
                    HierarchyDesigner_Info_Separator separator = separatorEntry.Value;

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(4);

                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.LabelField(separator.Name, GUILayout.Width(maxWidth));
                    GUILayout.Space(2);
                    separator.TextColor = EditorGUILayout.ColorField(separator.TextColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                    separator.BackgroundColor = EditorGUILayout.ColorField(separator.BackgroundColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                    separator.FontStyle = (FontStyle)EditorGUILayout.EnumPopup(separator.FontStyle, GUILayout.Width(100));
                    string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
                    int fontSizeIndex = Array.IndexOf(fontSizeOptions, separator.FontSize);
                    separator.FontSize = fontSizeOptions[EditorGUILayout.Popup(fontSizeIndex, fontSizeOptionsStrings, GUILayout.Width(40))];
                    separator.TextAlignment = (TextAnchor)EditorGUILayout.EnumPopup(separator.TextAlignment, GUILayout.Width(110));
                    separator.ImageType = (HierarchyDesigner_Info_Separator.BackgroundImageType)EditorGUILayout.EnumPopup(separator.ImageType, GUILayout.Width(100));
                    if (EditorGUI.EndChangeCheck())
                    {
                        hasModifiedChanges = true;
                    }

                    if (GUILayout.Button("Create", GUILayout.Width(60)))
                    {
                        HierarchyDesigner_Utility_Separator.CreateSeparator(separator);
                    }
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        separators.Remove(key);
                        hasModifiedChanges = true;
                        GUIUtility.ExitGUI();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(5);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                GUILayout.Space(2);
                if (GUILayout.Button("Update Separators", GUILayout.Height(30)))
                {
                    HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
                }
                GUILayout.Space(2);
                if (GUILayout.Button("Save Separators", GUILayout.Height(30)))
                {
                    SaveSeparators();
                    HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
                }
                GUILayout.Space(5);
            }
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private bool IsSeparatorNameValid(string separatorName)
        {
            return !string.IsNullOrEmpty(separatorName) && !separators.ContainsKey(separatorName);
        }

        public static void SaveSeparators()
        {
            HierarchyDesigner_Data_Separator.separators = new Dictionary<string, HierarchyDesigner_Info_Separator>(separators);
            HierarchyDesigner_Data_Separator.SaveSeparators();
            hasModifiedChanges = false;
        }

        private void OnDestroy()
        {
            if (hasModifiedChanges)
            {
                bool shouldSave = EditorUtility.DisplayDialog("Separator(s) Have Been Modified",
                    "Do you want to save the changes you made in the separators?",
                    "Save", "Don't Save");

                if (shouldSave)
                {
                    SaveSeparators();
                }
            }
            hasModifiedChanges = false;
            EditorApplication.delayCall -= CheckAndReloadData;
        }
    }

    public class HierarchyDesigner_Utility_Separator
    {
        private const string separatorPrefix = "//";
        private const string separatorName = "Separator";

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Create Default Separator", false, 2)]
        public static void CreateDefaultSeparator()
        {
            GameObject separator = new GameObject($"{separatorPrefix}{separatorName}");
            Undo.RegisterCreatedObjectUndo(separator, $"Create Default Separator");

            separator.tag = "EditorOnly";
            SetSeparatorState(separator, false);
            separator.SetActive(false);

            HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
        }

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Create All Separators", false, 1)]
        public static void CreateAllSeparatorsFromList()
        {
            foreach (HierarchyDesigner_Info_Separator separatorInfo in HierarchyDesigner_Data_Separator.separators.Values)
            {
                CreateSeparator(separatorInfo);
            }
        }

        [MenuItem("Hierarchy Designer/Hierarchy Separator/Create Missing Separators", false, 2)]
        public static void CreateMissingSeparators()
        {
            foreach (HierarchyDesigner_Info_Separator separatorInfo in HierarchyDesigner_Data_Separator.separators.Values)
            {
                if (!SeparatorExists(separatorInfo.Name))
                {
                    CreateSeparator(separatorInfo);
                }
            }
        }

        public static void CreateSeparator(HierarchyDesigner_Info_Separator separatorInfo)
        {
            GameObject separator = new GameObject($"{separatorPrefix}{separatorInfo.Name}");
            Undo.RegisterCreatedObjectUndo(separator, $"Create {separatorInfo.Name}");

            separator.tag = "EditorOnly";
            SetSeparatorState(separator, false);
            separator.SetActive(false);

            HierarchyDesigner_Visual_Separator.UpdateSeparatorVisuals();
        }

        private static bool SeparatorExists(string separatorName)
        {
            string fullSeparatorName = separatorPrefix + separatorName;
            #if UNITY_6000_0_OR_NEWER
            Transform[] allTransforms = GameObject.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            #else
            Transform[] allTransforms = UnityEngine.Object.FindObjectsOfType<Transform>(true);
            #endif

            foreach (Transform t in allTransforms)
            {
                if (t.gameObject.tag == "EditorOnly" && t.gameObject.name.Equals(fullSeparatorName))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject != null && gameObject.name.StartsWith(separatorPrefix) && gameObject.tag == "EditorOnly";
        }

        private static void SetSeparatorState(GameObject gameObject, bool editable)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component)
                {
                    component.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
                }
            }

            gameObject.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            EditorUtility.SetDirty(gameObject);
        }
    }
}
#endif