#if UNITY_EDITOR
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    static class HierarchyDesigner_Utility_MinorShortcuts
    {
        #region Windows
        [Shortcut("Hierarchy Designer/Folder Manager Window", KeyCode.Alpha1, ShortcutModifiers.Alt)]
        private static void OpenHierarchyFolderManager()
        {
            HierarchyFolderWindow.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Separator Manager Window", KeyCode.Alpha2, ShortcutModifiers.Alt)]
        private static void OpenHierarchySeparatorManager()
        {
            HierarchySeparatorWindow.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Presets Window", KeyCode.Alpha3, ShortcutModifiers.Alt)]
        private static void OpenHierarchyPresets()
        {
            HierarchyDesigner_Window_Presets.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/Tools Master Window", KeyCode.Alpha4, ShortcutModifiers.Alt)]
        private static void OpenHierarchyToolsMaster()
        {
            HierarchyDesigner_Window_Tools.OpenWindow();
        }

        [Shortcut("Hierarchy Designer/General Settings Window")]
        private static void OpenHierarchyGeneralSettings()
        {
            HierarchyDesigner_Utility_Settings.OpenWindow();
        }
        #endregion

        #region Create
        [Shortcut("Hierarchy Designer/Create All Folders")]
        private static void CreateAllHierarchyFolders()
        {
            HierarchyDesigner_Utility_Folder.CreateAllFoldersFromList();
        }

        [Shortcut("Hierarchy Designer/Create Default Folder")]
        private static void CreateDefaultHierarchyFolder()
        {
            HierarchyDesigner_Utility_Folder.CreateDefaultFolder();
        }

        [Shortcut("Hierarchy Designer/Create Missing Folders")]
        private static void CreateMissingHierarchyFolders()
        {
            HierarchyDesigner_Utility_Folder.CreateMissingFolders();
        }

        [Shortcut("Hierarchy Designer/Create All Separators")]
        private static void CreateAllHierarchySeparators()
        {
            HierarchyDesigner_Utility_Separator.CreateAllSeparatorsFromList();
        }

        [Shortcut("Hierarchy Designer/Create Default Separator")]
        private static void CreateDefaultHierarchySeparator()
        {
            HierarchyDesigner_Utility_Separator.CreateDefaultSeparator();
        }

        [Shortcut("Hierarchy Designer/Create Missing Separators")]
        private static void CreateMissingHierarchySeparators()
        {
            HierarchyDesigner_Utility_Separator.CreateMissingSeparators();
        }
        #endregion
    }
}
#endif