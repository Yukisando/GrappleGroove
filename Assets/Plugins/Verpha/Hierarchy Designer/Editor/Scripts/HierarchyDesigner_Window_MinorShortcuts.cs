#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_MinorShortcuts : EditorWindow
    {
        #region OnGUI Properties
        private Vector2 scrollPositionOuter;
        private Vector2 scrollPositionInner;
        private GUIStyle customSettingsStyle;
        private GUIStyle customSettingsStyleSecondary;
        private GUIStyle headerLabelStyle;
        private GUIStyle contentLabelStyle;
        #endregion
        #region Minor Shortcuts Properties
        private List<string> shortcutIdentifiers = new List<string> 
        {
            "Hierarchy Designer/Folder Manager Window",
            "Hierarchy Designer/Separator Manager Window",
            "Hierarchy Designer/Presets Window",
            "Hierarchy Designer/Tools Master Window",
            "Hierarchy Designer/General Settings Window",
            "Hierarchy Designer/Create All Folders",
            "Hierarchy Designer/Create Default Folder",
            "Hierarchy Designer/Create Missing Folders",
            "Hierarchy Designer/Create All Separators",
            "Hierarchy Designer/Create Default Separator",
            "Hierarchy Designer/Create Missing Separators",
        };
        #endregion

        [MenuItem("Hierarchy Designer/Hierarchy Minor Shortcuts", false, 100)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_MinorShortcuts window = GetWindow<HierarchyDesigner_Window_MinorShortcuts>("Hierarchy Designer Minor Shortcuts");
            window.minSize = new Vector2(300, 150);
        }

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
            EditorGUILayout.LabelField("Minor Shortcuts", headerLabelStyle);
            GUILayout.Space(8);

            scrollPositionOuter = EditorGUILayout.BeginScrollView(scrollPositionOuter, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            #region Minor Shortcuts Ids
            EditorGUILayout.BeginVertical(customSettingsStyleSecondary);
            EditorGUILayout.LabelField("Minor Shortcuts' List", contentLabelStyle);
            scrollPositionInner = EditorGUILayout.BeginScrollView(scrollPositionInner, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.Space(5);

            foreach (string shortcutId in shortcutIdentifiers)
            {
                ShortcutBinding currentBinding = ShortcutManager.instance.GetShortcutBinding(shortcutId);
                string commandName = shortcutId.Split('/').Last();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(commandName + ":", GUILayout.Width(165));
                if (currentBinding.keyCombinationSequence.Any())
                {
                    string keyCombos = string.Join(" + ", currentBinding.keyCombinationSequence.Select(kc => kc.ToString()));
                    GUILayout.Label(keyCombos, GUILayout.MinWidth(300));
                }
                else
                {
                    GUILayout.Label("unassigned shortcut", GUILayout.MinWidth(300));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            #endregion

            GUILayout.Space(2);
            EditorGUILayout.HelpBox("To modify minor shortcuts, please go to: Edit/Shortcuts.../Hierarchy Designer.\nYou can click the button below for quick access, then in the category section, search for Hierarchy Designer.", MessageType.Info);
            GUILayout.Space(5);

            if (GUILayout.Button("Open Shortcut Manager", GUILayout.Height(30)))
            {
                EditorApplication.ExecuteMenuItem("Edit/Shortcuts...");
            }
            GUILayout.Space(5);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}
#endif