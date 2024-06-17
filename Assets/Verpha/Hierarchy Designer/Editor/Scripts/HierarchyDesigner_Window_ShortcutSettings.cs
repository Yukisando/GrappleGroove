#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_ShortcutSettings : EditorWindow
    {
        #region Properties
        #region GUI
        private Vector2 outerScroll;
        private GUIStyle headerGUIStyle;
        private GUIStyle contentGUIStyle;
        private GUIStyle messageGUIStyle;
        private GUIStyle outerBackgroundGUIStyle;
        private GUIStyle innerBackgroundGUIStyle;
        private GUIStyle contentBackgroundGUIStyle;
        #endregion
        #region Const
        private const float enumToggleLabelWidth = 260;
        private const float minorShortcutCommandLabelWidth = 220;
        private const float minorShortcutLabelWidth = 300;
        #endregion
        #region Minor Shortcuts
        private List<string> minorShortcutIdentifiers = new List<string>
        {
            "Hierarchy Designer/Open Folder Manager Window",
            "Hierarchy Designer/Open Separator Manager Window",
            "Hierarchy Designer/Open Presets Window",
            "Hierarchy Designer/Open Tools Master Window",
            "Hierarchy Designer/Open General Settings Window",
            "Hierarchy Designer/Open Design Settings Window",
            "Hierarchy Designer/Open Shortcut Settings Window",
            "Hierarchy Designer/Open Advanced Settings Window",
            "Hierarchy Designer/Create All Folders",
            "Hierarchy Designer/Create Default Folder",
            "Hierarchy Designer/Create Missing Folders",
            "Hierarchy Designer/Create All Separators",
            "Hierarchy Designer/Create Default Separator",
            "Hierarchy Designer/Create Missing Separators",
        };
        #endregion
        #region Temporary Values
        private KeyCode tempToggleGameObjectActiveStateKeyCode;
        private KeyCode tempToggleLockStateKeyCode;
        private KeyCode tempChangeTagLayerKeyCode;
        private KeyCode tempRenameSelectedGameObjectsKeyCode;
        #endregion
        #endregion

        #region Menu Item
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Configurations + "/Shortcut Settings", false, HierarchyDesigner_Shared_MenuItems.LayerEleven)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_ShortcutSettings editorWindow = GetWindow<HierarchyDesigner_Window_ShortcutSettings>("Shortcut Settings");
            editorWindow.minSize = new Vector2(500, 300);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            LoadTempValues();
        }

        private void LoadTempValues()
        {
            tempToggleGameObjectActiveStateKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.ToggleGameObjectActiveStateKeyCode;
            tempToggleLockStateKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.ToggleLockStateKeyCode;
            tempChangeTagLayerKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.ChangeTagLayerKeyCode;
            tempRenameSelectedGameObjectsKeyCode = HierarchyDesigner_Configurable_ShortcutsSettings.RenameSelectedGameObjectsKeyCode;
        }
        #endregion

        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out messageGUIStyle, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Shortcut Settings", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);
            
            #region Main
            #region Major Shortcuts
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Major Shortcuts", contentGUIStyle);
            GUILayout.Space(4);
            tempToggleGameObjectActiveStateKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Toggle GameObject Active State Key Code", enumToggleLabelWidth, tempToggleGameObjectActiveStateKeyCode);
            tempToggleLockStateKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Toggle GameObject Lock State Key Code", enumToggleLabelWidth, tempToggleLockStateKeyCode);
            tempChangeTagLayerKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Change Selected Tag, Layer Key Code", enumToggleLabelWidth, tempChangeTagLayerKeyCode);
            tempRenameSelectedGameObjectsKeyCode = HierarchyDesigner_Shared_GUI.DrawEnumPopup("Rename Selected GameObjects Key Code", enumToggleLabelWidth, tempRenameSelectedGameObjectsKeyCode);
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            #region Minor Shortcuts
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Minor Shortcuts", contentGUIStyle);
            GUILayout.Space(4);
            foreach (string shortcutId in minorShortcutIdentifiers)
            {
                ShortcutBinding currentBinding = ShortcutManager.instance.GetShortcutBinding(shortcutId);
                string commandName = shortcutId.Split('/').Last();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(commandName + ":", GUILayout.Width(minorShortcutCommandLabelWidth));
                if (currentBinding.keyCombinationSequence.Any())
                {
                    string keyCombos = string.Join(" + ", currentBinding.keyCombinationSequence.Select(kc => kc.ToString()));
                    GUILayout.Label(keyCombos, GUILayout.MinWidth(minorShortcutLabelWidth));
                }
                else
                {
                    GUILayout.Label("unassigned shortcut", messageGUIStyle, GUILayout.MinWidth(minorShortcutLabelWidth));
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.Space(2);
            EditorGUILayout.HelpBox("To modify minor shortcuts, please go to: Edit/Shortcuts.../Hierarchy Designer.\nYou can click the button below for quick access, then in the category section, search for Hierarchy Designer.", MessageType.Info);
            GUILayout.Space(2);
            EditorGUILayout.EndVertical();
            #endregion
            #endregion

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            #region Footer
            if (GUILayout.Button("Open Shortcut Manager", GUILayout.Height(30)))
            {
                EditorApplication.ExecuteMenuItem("Edit/Shortcuts...");
            }
            if (GUILayout.Button("Update and Save Settings", GUILayout.Height(30)))
            {
                SaveSettings();
            }
            EditorGUILayout.EndVertical();
            #endregion
        }

        private void SaveSettings()
        {
            HierarchyDesigner_Configurable_ShortcutsSettings.ToggleGameObjectActiveStateKeyCode = tempToggleGameObjectActiveStateKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.ToggleLockStateKeyCode = tempToggleLockStateKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.ChangeTagLayerKeyCode = tempChangeTagLayerKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.RenameSelectedGameObjectsKeyCode = tempRenameSelectedGameObjectsKeyCode;
            HierarchyDesigner_Configurable_ShortcutsSettings.SaveSettings();
        }
    }
}
#endif