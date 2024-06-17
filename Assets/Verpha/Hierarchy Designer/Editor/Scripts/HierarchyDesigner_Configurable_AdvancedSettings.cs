#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_AdvancedSettings
    {
        #region Properties
        [System.Serializable]
        private class HierarchyDesigner_AdvancedSettings
        {
            public bool EnableDynamicChangesCheckForGameObjectMainIcon = true;
            public bool EnableDynamicBackgroundForGameObjectMainIcon = true;
            public bool EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = true;
            public bool EnableCustomizationForGameObjectComponentIcons = true;
            public bool EnableTooltipOnComponentIconHovered = true;
            public bool EnableActiveStateEffectForComponentIcons = true;
            public bool DisableComponentIconsForInactiveGameObjects = true;
        }
        private static HierarchyDesigner_AdvancedSettings advancedSettings = new HierarchyDesigner_AdvancedSettings();
        private const string settingsFileName = "HierarchyDesigner_SavedData_AdvancedSettings.json";
        #endregion

        #region Initialization Methods
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            HierarchyDesigner_Manager_GameObject.EnableDynamicChangesCheckForGameObjectMainIconCache = EnableDynamicChangesCheckForGameObjectMainIcon;
            HierarchyDesigner_Manager_GameObject.EnableDynamicBackgroundForGameObjectMainIconCache = EnableDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Manager_GameObject.EnablePreciseRectForDynamicBackgroundForGameObjectMainIconCache = EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            HierarchyDesigner_Manager_GameObject.DisableComponentIconsForInactiveGameObjects = DisableComponentIconsForInactiveGameObjects;
            HierarchyDesigner_Manager_GameObject.EnableCustomizationForGameObjectComponentIconsCache = EnableCustomizationForGameObjectComponentIcons;
            HierarchyDesigner_Manager_GameObject.EnableTooltipOnComponentIconHoveredCache = EnableTooltipOnComponentIconHovered;
            HierarchyDesigner_Manager_GameObject.EnableActiveStateEffectForComponentIconsCache = EnableActiveStateEffectForComponentIcons;
        }
        #endregion

        #region Accessors
        public static bool EnableDynamicChangesCheckForGameObjectMainIcon
        {
            get => advancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon;
            set
            {
                if (advancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon != value)
                {
                    advancedSettings.EnableDynamicChangesCheckForGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnableDynamicChangesCheckForGameObjectMainIconCache = value;
                }
            }
        }

        public static bool EnableDynamicBackgroundForGameObjectMainIcon
        {
            get => advancedSettings.EnableDynamicBackgroundForGameObjectMainIcon;
            set
            {
                if (advancedSettings.EnableDynamicBackgroundForGameObjectMainIcon != value)
                {
                    advancedSettings.EnableDynamicBackgroundForGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnableDynamicBackgroundForGameObjectMainIconCache = value;
                }
            }
        }

        public static bool EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon
        {
            get => advancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon;
            set
            {
                if (advancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon != value)
                {
                    advancedSettings.EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = value;
                    HierarchyDesigner_Manager_GameObject.EnablePreciseRectForDynamicBackgroundForGameObjectMainIconCache = value;
                }
            }
        }

        public static bool EnableCustomizationForGameObjectComponentIcons
        {
            get => advancedSettings.EnableCustomizationForGameObjectComponentIcons;
            set
            {
                if (advancedSettings.EnableCustomizationForGameObjectComponentIcons != value)
                {
                    advancedSettings.EnableCustomizationForGameObjectComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.EnableCustomizationForGameObjectComponentIconsCache = value;
                }
            }
        }

        public static bool EnableTooltipOnComponentIconHovered
        {
            get => advancedSettings.EnableTooltipOnComponentIconHovered;
            set
            {
                if (advancedSettings.EnableTooltipOnComponentIconHovered != value)
                {
                    advancedSettings.EnableTooltipOnComponentIconHovered = value;
                    HierarchyDesigner_Manager_GameObject.EnableTooltipOnComponentIconHoveredCache = value;
                }
            }
        }

        public static bool EnableActiveStateEffectForComponentIcons
        {
            get => advancedSettings.EnableActiveStateEffectForComponentIcons;
            set
            {
                if (advancedSettings.EnableActiveStateEffectForComponentIcons != value)
                {
                    advancedSettings.EnableActiveStateEffectForComponentIcons = value;
                    HierarchyDesigner_Manager_GameObject.EnableActiveStateEffectForComponentIconsCache = value;
                }
            }
        }

        public static bool DisableComponentIconsForInactiveGameObjects
        {
            get => advancedSettings.DisableComponentIconsForInactiveGameObjects;
            set
            {
                if (advancedSettings.DisableComponentIconsForInactiveGameObjects != value)
                {
                    advancedSettings.DisableComponentIconsForInactiveGameObjects = value;
                    HierarchyDesigner_Manager_GameObject.DisableComponentIconsForInactiveGameObjects = value;
                }
            }
        }
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            string json = JsonUtility.ToJson(advancedSettings, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_AdvancedSettings loadedSettings = JsonUtility.FromJson<HierarchyDesigner_AdvancedSettings>(json);
                advancedSettings = loadedSettings;
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            advancedSettings = new HierarchyDesigner_AdvancedSettings()
            {
                EnableDynamicChangesCheckForGameObjectMainIcon = true,
                EnableDynamicBackgroundForGameObjectMainIcon = true,
                EnablePreciseRectForDynamicBackgroundForGameObjectMainIcon = true,
                EnableCustomizationForGameObjectComponentIcons = true,
                EnableTooltipOnComponentIconHovered = true,
                EnableActiveStateEffectForComponentIcons = true,
                DisableComponentIconsForInactiveGameObjects = true
            };
        }
        #endregion
    }
}
#endif