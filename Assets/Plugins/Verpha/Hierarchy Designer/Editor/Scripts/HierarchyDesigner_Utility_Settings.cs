#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Utility_Settings : EditorWindow
    {
        #region OnGUI Properties
        private Vector2 scrollPositionOuter;
        private Vector2 scrollPositionInner;
        private GUIStyle customSettingsStyle;
        private GUIStyle customSettingsStyleSecondary;
        private GUIStyle headerLabelStyle;
        private GUIStyle contentLabelStyle;
        #endregion
        #region Temporary Holder Properties
        private bool showMainIconOfGameObjectTemp;
        private bool showComponentIconsTemp;
        private bool showTransformComponentTemp;
        private bool showComponentIconsForFoldersTemp;
        private bool showTagTemp;
        private bool showLayerTemp;
        private bool showHierarchyTreeTemp;
        private bool showHierarchyButtonsTemp;
        private bool disableHierarchyDesignerDuringPlayModeTemp;
        private List<string> excludedTagsTemp;
        private List<string> excludedLayersTemp;
        #endregion
        #region Filtering Properties
        private int tagMask;
        private int layerMask;
        #endregion
        #region Styling Properties
        private Color newTagTextColor = Color.gray;
        private FontStyle newTagFontStyle = FontStyle.Normal;
        private int newTagFontSize = 10;
        private Color newLayerTextColor = Color.gray;
        private FontStyle newLayerFontStyle = FontStyle.Normal;
        private int newLayerFontSize = 10;
        private readonly int[] fontSizeOptions = new int[15];
        private Color newTreeColor = Color.white;
        private HierarchyDesigner_Info_Buttons.ButtonsPositionType newButtonsPosition = HierarchyDesigner_Info_Buttons.ButtonsPositionType.Docked;
        #endregion
        #region Shortcuts Properties
        private KeyCode enableDisableShortcutTemp;
        private KeyCode lockUnlockShortcutTemp;
        private KeyCode changeTagAndLayerShortcutTemp;
        private KeyCode renameGameObjectsShortcutTemp;
        #endregion

        [MenuItem("Hierarchy Designer/Hierarchy General Settings", false, 100)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Utility_Settings window = GetWindow<HierarchyDesigner_Utility_Settings>("Hierarchy Designer General Settings");
            window.minSize = new Vector2(400, 200);
        }

        private void OnEnable()
        {
            LoadSettings();
            InitFontSizeOptions();
            InitMasks();
        }

        private void LoadSettings()
        {
            showMainIconOfGameObjectTemp = HierarchyDesigner_Manager_Settings.ShowMainIconOfGameObject;
            showComponentIconsTemp = HierarchyDesigner_Manager_Settings.ShowComponentIcons;
            showTransformComponentTemp = HierarchyDesigner_Manager_Settings.ShowTransformComponent;
            showComponentIconsForFoldersTemp = HierarchyDesigner_Manager_Settings.ShowComponentIconsForFolders;
            showTagTemp = HierarchyDesigner_Manager_Settings.ShowTag;
            showLayerTemp = HierarchyDesigner_Manager_Settings.ShowLayer;
            showHierarchyTreeTemp = HierarchyDesigner_Manager_Settings.ShowHierarchyTree;
            showHierarchyButtonsTemp = HierarchyDesigner_Manager_Settings.ShowHierarchyButtons;
            disableHierarchyDesignerDuringPlayModeTemp = HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode;
            excludedTagsTemp = new List<string>(HierarchyDesigner_Manager_Settings.ExcludedTags);
            excludedLayersTemp = new List<string>(HierarchyDesigner_Manager_Settings.ExcludedLayers);
            newTagTextColor = HierarchyDesigner_Manager_Settings.TagTextColor;
            newTagFontStyle = HierarchyDesigner_Manager_Settings.TagFontStyle;
            newTagFontSize = HierarchyDesigner_Manager_Settings.TagFontSize;
            newLayerTextColor = HierarchyDesigner_Manager_Settings.LayerTextColor;
            newLayerFontStyle = HierarchyDesigner_Manager_Settings.LayerFontStyle;
            newLayerFontSize = HierarchyDesigner_Manager_Settings.LayerFontSize;
            newTreeColor = HierarchyDesigner_Manager_Settings.TreeColor;
            newButtonsPosition = HierarchyDesigner_Manager_Settings.ButtonsPosition;
            enableDisableShortcutTemp = HierarchyDesigner_Manager_Settings.EnableDisableShortcut;
            lockUnlockShortcutTemp = HierarchyDesigner_Manager_Settings.LockUnlockShortcut;
            changeTagAndLayerShortcutTemp = HierarchyDesigner_Manager_Settings.ChangeTagAndLayerShortcut;
            renameGameObjectsShortcutTemp = HierarchyDesigner_Manager_Settings.RenameGameObjectsShortcut;
        }

        private void InitFontSizeOptions()
        {
            for (int i = 0; i < fontSizeOptions.Length; i++)
            {
                fontSizeOptions[i] = 7 + i;
            }
        }

        #region Filtering Methods
        private void InitMasks()
        {
            tagMask = CreateMaskFromList(excludedTagsTemp, UnityEditorInternal.InternalEditorUtility.tags);
            layerMask = CreateMaskFromList(excludedLayersTemp, UnityEditorInternal.InternalEditorUtility.layers);
        }

        private int CreateMaskFromList(List<string> excludedItems, string[] allItems)
        {
            int mask = 0;
            for (int i = 0; i < allItems.Length; i++)
            {
                if (excludedItems.Contains(allItems[i])) mask |= (1 << i);
            }
            return mask;
        }

        private void UpdateExcludedFromMask(ref List<string> excludedItems, int mask, string[] allItems)
        {
            excludedItems.Clear();
            for (int i = 0; i < allItems.Length; i++)
            {
                if ((mask & (1 << i)) != 0) excludedItems.Add(allItems[i]);
            }
        }
        #endregion

        private void InitializeStyles()
        {
            customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            customSettingsStyleSecondary = HierarchyDesigner_Info_OnGUI.CreateCustomStyle(true);
            headerLabelStyle = HierarchyDesigner_Info_OnGUI.HeaderLabelStyle;
            contentLabelStyle = HierarchyDesigner_Info_OnGUI.ContentLabelStyle;
        }

        private void OnGUI()
        {
            InitializeStyles();
            EditorGUILayout.BeginVertical(customSettingsStyle);

            GUILayout.Space(4);
            EditorGUILayout.LabelField("General Settings", headerLabelStyle);
            GUILayout.Space(8);

            scrollPositionOuter = EditorGUILayout.BeginScrollView(scrollPositionOuter, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            #region General Settings Fields
            using (new HierarchyDesigner_Info_OnGUI.LabelWidth(100))
            {
                showMainIconOfGameObjectTemp = DrawSettingToggle("Show Main Icon", showMainIconOfGameObjectTemp);
                showComponentIconsTemp = DrawSettingToggle("Show Component Icons", showComponentIconsTemp);
                showTransformComponentTemp = DrawSettingToggle("Show Transform Component Icon", showTransformComponentTemp);
                showComponentIconsForFoldersTemp = DrawSettingToggle("Show Folder's Component Icons", showComponentIconsForFoldersTemp);
                showTagTemp = DrawSettingToggle("Show GameObjects' Tag", showTagTemp);
                showLayerTemp = DrawSettingToggle("Show GameObjects' Layer", showLayerTemp);
                showHierarchyTreeTemp = DrawSettingToggle("Show Hierarchy Tree", showHierarchyTreeTemp);
                showHierarchyButtonsTemp = DrawSettingToggle("Show Hierarchy Buttons", showHierarchyButtonsTemp);
                disableHierarchyDesignerDuringPlayModeTemp = DrawSettingToggle("Disable Hierarchy Designer During Play Mode", disableHierarchyDesignerDuringPlayModeTemp);
            }
            #endregion

            #region General Settings Buttons
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable All", GUILayout.Height(25)))
            {
                SetAllFeatures(true);
            }
            if (GUILayout.Button("Disable All", GUILayout.Height(25)))
            {
                SetAllFeatures(false);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            #endregion

            #region Filtering Fields
            EditorGUILayout.BeginVertical(customSettingsStyleSecondary);
            EditorGUILayout.LabelField("Filtering", contentLabelStyle);
            GUILayout.Space(5);

            scrollPositionInner = EditorGUILayout.BeginScrollView(scrollPositionInner, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Exclude Tags", GUILayout.MaxWidth(100));
            int newTagMask = EditorGUILayout.MaskField(tagMask, UnityEditorInternal.InternalEditorUtility.tags);
            if (newTagMask != tagMask)
            {
                tagMask = newTagMask;
                UpdateExcludedFromMask(ref excludedTagsTemp, tagMask, UnityEditorInternal.InternalEditorUtility.tags);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Exclude Layers", GUILayout.MaxWidth(100));
            int newLayerMask = EditorGUILayout.MaskField(layerMask, UnityEditorInternal.InternalEditorUtility.layers);
            if (newLayerMask != layerMask)
            {
                layerMask = newLayerMask;
                UpdateExcludedFromMask(ref excludedLayersTemp, layerMask, UnityEditorInternal.InternalEditorUtility.layers);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            #endregion

            #region Styling Fields
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Styling", contentLabelStyle);
            GUILayout.Space(5);

            #region Tag Fields
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Hierarchy Tag", GUILayout.MaxWidth(110));
            newTagTextColor = EditorGUILayout.ColorField(newTagTextColor, GUILayout.Height(20));
            newTagFontStyle = (FontStyle)EditorGUILayout.EnumPopup(newTagFontStyle, GUILayout.Height(20));
            string[] fontSizeOptionsStrings = Array.ConvertAll(fontSizeOptions, x => x.ToString());
            int newTagFontSizeIndex = Array.IndexOf(fontSizeOptions, newTagFontSize);
            newTagFontSize = fontSizeOptions[EditorGUILayout.Popup(newTagFontSizeIndex, fontSizeOptionsStrings, GUILayout.Height(20))];
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Layer Fields
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Hierarchy Layer", GUILayout.MaxWidth(110));
            newLayerTextColor = EditorGUILayout.ColorField(newLayerTextColor, GUILayout.Height(20));
            newLayerFontStyle = (FontStyle)EditorGUILayout.EnumPopup(newLayerFontStyle, GUILayout.Height(20));
            int newLayerFontSizeIndex = Array.IndexOf(fontSizeOptions, newLayerFontSize);
            newLayerFontSize = fontSizeOptions[EditorGUILayout.Popup(newLayerFontSizeIndex, fontSizeOptionsStrings, GUILayout.Height(20))];
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Tree Fields
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Hierarchy Tree", GUILayout.MaxWidth(110));
            newTreeColor = EditorGUILayout.ColorField(newTreeColor, GUILayout.ExpandWidth(true), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            #endregion

            #region Buttons Fields
            GUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Hierarchy Buttons", GUILayout.MaxWidth(110));
            newButtonsPosition = (HierarchyDesigner_Info_Buttons.ButtonsPositionType)EditorGUILayout.EnumPopup(newButtonsPosition, GUILayout.ExpandWidth(true), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();
            #endregion
            #endregion

            #region Shortcuts Fields
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Major Shortcuts", contentLabelStyle);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enable/Disable GameObject Shortcut", GUILayout.Width(225));
            enableDisableShortcutTemp = (KeyCode)EditorGUILayout.EnumPopup(enableDisableShortcutTemp);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Lock/Unlock GameObject Shortcut", GUILayout.Width(225));
            lockUnlockShortcutTemp = (KeyCode)EditorGUILayout.EnumPopup(lockUnlockShortcutTemp);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Change Tag and Layer Shortcut", GUILayout.Width(225));
            changeTagAndLayerShortcutTemp = (KeyCode)EditorGUILayout.EnumPopup(changeTagAndLayerShortcutTemp);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rename GameObjects Shortcut", GUILayout.Width(225));
            renameGameObjectsShortcutTemp = (KeyCode)EditorGUILayout.EnumPopup(renameGameObjectsShortcutTemp);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(2);
            if (GUILayout.Button("Update and Save Settings", GUILayout.Height(30)))
            {
                SaveSettings();
            }
            GUILayout.Space(5);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private bool DrawSettingToggle(string label, bool currentValue)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(280));
            bool newValue = EditorGUILayout.Toggle(currentValue);
            EditorGUILayout.EndHorizontal();
            return newValue;
        }

        private void SetAllFeatures(bool enable)
        {
            showMainIconOfGameObjectTemp = enable;
            showComponentIconsTemp = enable;
            showTransformComponentTemp = enable;
            showComponentIconsForFoldersTemp = enable;
            showTagTemp = enable;
            showLayerTemp = enable;
            showHierarchyTreeTemp = enable;
            showHierarchyButtonsTemp = enable;
            disableHierarchyDesignerDuringPlayModeTemp = enable;
        }

        private void SaveSettings()
        {
            bool tagLayerTagsChanged = !Equals(HierarchyDesigner_Manager_Settings.ExcludedTags, excludedTagsTemp);
            bool tagLayerLayersChanged = !Equals(HierarchyDesigner_Manager_Settings.ExcludedLayers, excludedLayersTemp);
            bool fontSizeChanged = (HierarchyDesigner_Manager_Settings.TagFontSize != newTagFontSize || HierarchyDesigner_Manager_Settings.LayerFontSize != newLayerFontSize);
            bool treeColorChanged = HierarchyDesigner_Manager_Settings.TreeColor != newTreeColor;

            HierarchyDesigner_Manager_Settings.ShowMainIconOfGameObject = showMainIconOfGameObjectTemp;
            HierarchyDesigner_Manager_Settings.ShowComponentIcons = showComponentIconsTemp;
            HierarchyDesigner_Manager_Settings.ShowTransformComponent = showTransformComponentTemp;
            HierarchyDesigner_Manager_Settings.ShowComponentIconsForFolders = showComponentIconsForFoldersTemp;
            HierarchyDesigner_Manager_Settings.ShowTag = showTagTemp;
            HierarchyDesigner_Manager_Settings.ShowLayer = showLayerTemp;
            HierarchyDesigner_Manager_Settings.ShowHierarchyTree = showHierarchyTreeTemp;
            HierarchyDesigner_Manager_Settings.ShowHierarchyButtons = showHierarchyButtonsTemp;
            HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode = disableHierarchyDesignerDuringPlayModeTemp;
            HierarchyDesigner_Manager_Settings.ExcludedTags = new List<string>(excludedTagsTemp);
            HierarchyDesigner_Manager_Settings.ExcludedLayers = new List<string>(excludedLayersTemp);
            HierarchyDesigner_Manager_Settings.TagTextColor = newTagTextColor;
            HierarchyDesigner_Manager_Settings.TagFontStyle = newTagFontStyle;
            HierarchyDesigner_Manager_Settings.TagFontSize = newTagFontSize;
            HierarchyDesigner_Manager_Settings.LayerTextColor = newLayerTextColor;
            HierarchyDesigner_Manager_Settings.LayerFontStyle = newLayerFontStyle;
            HierarchyDesigner_Manager_Settings.LayerFontSize = newLayerFontSize;
            HierarchyDesigner_Manager_Settings.TreeColor = newTreeColor;
            HierarchyDesigner_Manager_Settings.ButtonsPosition = newButtonsPosition;
            HierarchyDesigner_Manager_Settings.EnableDisableShortcut = enableDisableShortcutTemp;
            HierarchyDesigner_Manager_Settings.LockUnlockShortcut = lockUnlockShortcutTemp;
            HierarchyDesigner_Manager_Settings.ChangeTagAndLayerShortcut = changeTagAndLayerShortcutTemp;
            HierarchyDesigner_Manager_Settings.RenameGameObjectsShortcut = renameGameObjectsShortcutTemp;

            if (tagLayerTagsChanged || tagLayerLayersChanged)
            {
                HierarchyDesigner_Visual_GameObject.ClearExcludeTagLayerCache();
            }
            if (fontSizeChanged)
            {
                HierarchyDesigner_Visual_GameObject.RecalculateTagAndLayerSizes();
            }
            if (treeColorChanged)
            {
                HierarchyDesigner_Visual_GameObject.UpdateTreeColorCache();
            }

            EditorApplication.RepaintHierarchyWindow();
        }
    }
}
#endif