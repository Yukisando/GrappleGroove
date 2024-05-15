#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public static class HierarchyDesigner_Manager_Settings
    {
        #region Properties
        private const string SettingsFileName = "HierarchyDesigner_SavedData_Settings.json";
        #endregion

        #region Data
        [System.Serializable]
        private class HierarchyDesigner_SettingsData
        {
            public bool showMainIconOfGameObject = true;
            public bool showComponentIcons = true;
            public bool showTransformComponent = true;
            public bool showComponentIconsForFolders = true;
            public bool showTag = true;
            public bool showLayer = true;
            public bool showHierarchyTree = true;
            public bool showHierarchyButtons = true;
            public bool disableHierarchyDesignerDuringPlayMode = true;
            public List<string> excludedTags = new List<string>();
            public List<string> excludedLayers = new List<string>();
            public Color tagTextColor = Color.gray;
            public FontStyle tagFontStyle = FontStyle.BoldAndItalic;
            public int tagFontSize = 9;
            public Color layerTextColor = Color.gray;
            public FontStyle layerFontStyle = FontStyle.BoldAndItalic;
            public int layerFontSize = 9;
            public Color treeColor = Color.white;
            public HierarchyDesigner_Info_Buttons.ButtonsPositionType buttonsPosition = HierarchyDesigner_Info_Buttons.ButtonsPositionType.Docked;
            public KeyCode enableDisableShortcut = KeyCode.Mouse2;
            public KeyCode lockUnlockShortcut = KeyCode.F1;
            public KeyCode changeTagAndLayerShortcut = KeyCode.F2;
            public KeyCode renameGameObjectsShortcut = KeyCode.F3;
        }

        private static HierarchyDesigner_SettingsData settings = new HierarchyDesigner_SettingsData();
        #endregion

        #region Accessors
        public static bool ShowMainIconOfGameObject
        {
            get => settings.showMainIconOfGameObject;
            set
            {
                if (settings.showMainIconOfGameObject != value)
                {
                    settings.showMainIconOfGameObject = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowComponentIcons
        {
            get => settings.showComponentIcons;
            set
            {
                if (settings.showComponentIcons != value)
                {
                    settings.showComponentIcons = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowTransformComponent
        {
            get => settings.showTransformComponent;
            set
            {
                if (settings.showTransformComponent != value)
                {
                    settings.showTransformComponent = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowComponentIconsForFolders
        {
            get => settings.showComponentIconsForFolders;
            set
            {
                if (settings.showComponentIconsForFolders != value)
                {
                    settings.showComponentIconsForFolders = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowTag
        {
            get => settings.showTag;
            set
            {
                if (settings.showTag != value)
                {
                    settings.showTag = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowLayer
        {
            get => settings.showLayer;
            set
            {
                if (settings.showLayer != value)
                {
                    settings.showLayer = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowHierarchyTree
        {
            get => settings.showHierarchyTree;
            set
            {
                if (settings.showHierarchyTree != value)
                {
                    settings.showHierarchyTree = value;
                    SaveSettings();
                }
            }
        }

        public static bool ShowHierarchyButtons
        {
            get => settings.showHierarchyButtons;
            set
            {
                if (settings.showHierarchyButtons != value)
                {
                    settings.showHierarchyButtons = value;
                    SaveSettings();
                }
            }
        }

        public static bool DisableHierarchyDesignerDuringPlayMode
        {
            get => settings.disableHierarchyDesignerDuringPlayMode;
            set
            {
                if (settings.disableHierarchyDesignerDuringPlayMode != value)
                {
                    settings.disableHierarchyDesignerDuringPlayMode = value;
                    SaveSettings();
                }
            }
        }

        public static List<string> ExcludedTags
        {
            get => settings.excludedTags;
            set
            {
                if (settings.excludedTags != value)
                {
                    settings.excludedTags = value;
                    SaveSettings();
                }
            }
        }

        public static List<string> ExcludedLayers
        {
            get => settings.excludedLayers;
            set
            {
                if (settings.excludedLayers != value)
                {
                    settings.excludedLayers = value;
                    SaveSettings();
                }
            }
        }

        public static Color TagTextColor
        {
            get => settings.tagTextColor;
            set
            {
                if (settings.tagTextColor != value)
                {
                    settings.tagTextColor = value;
                    SaveSettings();
                }
            }
        }

        public static FontStyle TagFontStyle
        {
            get => settings.tagFontStyle;
            set
            {
                if (settings.tagFontStyle != value)
                {
                    settings.tagFontStyle = value;
                    SaveSettings();
                }
            }
        }

        public static int TagFontSize
        {
            get => settings.tagFontSize;
            set
            {
                if (settings.tagFontSize != value)
                {
                    settings.tagFontSize = value;
                    SaveSettings();
                }
            }
        }

        public static Color LayerTextColor
        {
            get => settings.layerTextColor;
            set
            {
                if (settings.layerTextColor != value)
                {
                    settings.layerTextColor = value;
                    SaveSettings();
                }
            }
        }

        public static FontStyle LayerFontStyle
        {
            get => settings.layerFontStyle;
            set
            {
                if (settings.layerFontStyle != value)
                {
                    settings.layerFontStyle = value;
                    SaveSettings();
                }
            }
        }

        public static int LayerFontSize
        {
            get => settings.layerFontSize;
            set
            {
                if (settings.layerFontSize != value)
                {
                    settings.layerFontSize = value;
                    SaveSettings();
                }
            }
        }

        public static Color TreeColor
        {
            get => settings.treeColor;
            set
            {
                if (settings.treeColor != value)
                {
                    settings.treeColor = value;
                    SaveSettings();
                }
            }
        }

        public static HierarchyDesigner_Info_Buttons.ButtonsPositionType ButtonsPosition
        {
            get => settings.buttonsPosition;
            set
            {
                if (settings.buttonsPosition != value)
                {
                    settings.buttonsPosition = value;
                    SaveSettings();
                }
            }
        }

        public static KeyCode EnableDisableShortcut
        {
            get => settings.enableDisableShortcut;
            set
            {
                if (settings.enableDisableShortcut != value)
                {
                    settings.enableDisableShortcut = value;
                    SaveSettings();
                }
            }
        }

        public static KeyCode LockUnlockShortcut
        {
            get => settings.lockUnlockShortcut;
            set
            {
                if (settings.lockUnlockShortcut != value)
                {
                    settings.lockUnlockShortcut = value;
                    SaveSettings();
                }
            }
        }

        public static KeyCode ChangeTagAndLayerShortcut
        {
            get => settings.changeTagAndLayerShortcut;
            set
            {
                if (settings.changeTagAndLayerShortcut != value)
                {
                    settings.changeTagAndLayerShortcut = value;
                    SaveSettings();
                }
            }
        }

        public static KeyCode RenameGameObjectsShortcut
        {
            get => settings.renameGameObjectsShortcut;
            set
            {
                if (settings.renameGameObjectsShortcut != value)
                {
                    settings.renameGameObjectsShortcut = value;
                    SaveSettings();
                }
            }
        }
        #endregion

        static HierarchyDesigner_Manager_Settings()
        {
            LoadSettings();
        }

        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Shared_Data.GetDataFilePath(SettingsFileName);
            string json = JsonUtility.ToJson(settings, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Shared_Data.GetDataFilePath(SettingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_SettingsData loadedSettings = JsonUtility.FromJson<HierarchyDesigner_SettingsData>(json);
                loadedSettings.buttonsPosition = HierarchyDesigner_Shared_EnumFilter.ParseEnum(loadedSettings.buttonsPosition.ToString(), HierarchyDesigner_Info_Buttons.ButtonsPositionType.Docked);
                settings = loadedSettings;
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            settings = new HierarchyDesigner_SettingsData()
            {
                showMainIconOfGameObject = true,
                showComponentIcons = true,
                showTransformComponent = true,
                showComponentIconsForFolders = true,
                showTag = true,
                showLayer = true,
                showHierarchyTree = true,
                showHierarchyButtons = true,
                disableHierarchyDesignerDuringPlayMode = true,
                excludedTags = new List<string>(),
                excludedLayers = new List<string>(),
                tagTextColor = Color.gray,
                tagFontStyle = FontStyle.Normal,
                tagFontSize = 10,
                layerTextColor = Color.gray,
                layerFontStyle = FontStyle.Normal,
                layerFontSize = 10,
                treeColor = Color.white,
                buttonsPosition = HierarchyDesigner_Info_Buttons.ButtonsPositionType.AfterProperties,
                enableDisableShortcut = KeyCode.Mouse2,
                lockUnlockShortcut = KeyCode.F1,
                changeTagAndLayerShortcut = KeyCode.F2,
                renameGameObjectsShortcut = KeyCode.F3
            };
        }
    }
}
#endif