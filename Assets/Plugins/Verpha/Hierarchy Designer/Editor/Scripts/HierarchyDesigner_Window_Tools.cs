#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Tools : EditorWindow
    {
        #region OnGUI Properties
        private Vector2 scrollPositionOuter;
        private GUIStyle customSettingsStyle;
        private GUIStyle contentLabelStyle;
        #endregion
        #region Main Properties
        private HierarchyDesigner_ToolCategory selectedCategory = HierarchyDesigner_ToolCategory.Counting;
        private int selectedActionIndex = 0;
        private List<string> availableActionNames = new List<string>();
        private List<MethodInfo> availableActionMethods = new List<MethodInfo>();
        private static Dictionary<HierarchyDesigner_ToolCategory, List<(string Name, MethodInfo Method)>> cachedActions = new Dictionary<HierarchyDesigner_ToolCategory, List<(string Name, MethodInfo Method)>>();
        private static bool cacheInitialized = false;
        #endregion

        [MenuItem("Hierarchy Designer/Hierarchy Helpers/Tools Master", false, 50)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_Tools window = GetWindow<HierarchyDesigner_Window_Tools>("Tools Master", true);
            window.minSize = new Vector2(200, 100);
        }

        private void OnEnable()
        {
            if (!cacheInitialized)
            {
                InitializeActionCache();
                cacheInitialized = true;
            }
            UpdateAvailableActions();
        }

        private static void InitializeActionCache()
        {
            MethodInfo[] methods = typeof(HierarchyDesigner_Utility_Tools).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (MethodInfo method in methods)
            {
                object[] toolAttributes = method.GetCustomAttributes(typeof(HierarchyDesigner_ToolAttribute), false);
                for (int i = 0; i < toolAttributes.Length; i++)
                {
                    HierarchyDesigner_ToolAttribute toolAttribute = toolAttributes[i] as HierarchyDesigner_ToolAttribute;
                    if (toolAttribute != null)
                    {
                        object[] menuItemAttributes = method.GetCustomAttributes(typeof(MenuItem), true);
                        for (int j = 0; j < menuItemAttributes.Length; j++)
                        {
                            MenuItem menuItemAttribute = menuItemAttributes[j] as MenuItem;
                            if (menuItemAttribute != null)
                            {
                                string rawActionName = menuItemAttribute.menuItem;
                                string actionName = ExtractActionNameForCategory(rawActionName, toolAttribute.Category);
                                if (!string.IsNullOrEmpty(actionName))
                                {
                                    if (!cachedActions.ContainsKey(toolAttribute.Category))
                                    {
                                        cachedActions[toolAttribute.Category] = new List<(string Name, MethodInfo Method)>();
                                    }
                                    cachedActions[toolAttribute.Category].Add((actionName, method));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void InitializeStyles()
        {
            customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            contentLabelStyle = HierarchyDesigner_Info_OnGUI.ContentLabelStyle;
        }

        private void OnGUI()
        {
            InitializeStyles();
            EditorGUILayout.BeginVertical(customSettingsStyle);

            GUILayout.Space(4);
            EditorGUILayout.LabelField("Tools Master", contentLabelStyle);
            GUILayout.Space(4);

            scrollPositionOuter = EditorGUILayout.BeginScrollView(scrollPositionOuter, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            #region Category and Action Fields
            HierarchyDesigner_ToolCategory previousCategory = selectedCategory;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select Category:", GUILayout.Width(100));
            selectedCategory = (HierarchyDesigner_ToolCategory)EditorGUILayout.EnumPopup(selectedCategory);
            EditorGUILayout.EndHorizontal();

            if (previousCategory != selectedCategory)
            {
                UpdateAvailableActions();
            }

            if (availableActionNames.Count == 0)
            {
                GUILayout.Label("No tools available for this category.");
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Action:", GUILayout.Width(100));
                selectedActionIndex = EditorGUILayout.Popup(selectedActionIndex, availableActionNames.ToArray());
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);
                if (GUILayout.Button("Apply Action", GUILayout.Height(30)))
                {
                    ExecuteSelectedMethod();
                }
            }
            #endregion

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void UpdateAvailableActions()
        {
            availableActionNames.Clear();
            availableActionMethods.Clear();
            if (cachedActions.TryGetValue(selectedCategory, out var actions))
            {
                foreach (var action in actions)
                {
                    availableActionNames.Add(action.Name);
                    availableActionMethods.Add(action.Method);
                }
            }
            selectedActionIndex = 0;
        }

        private static string ExtractActionNameForCategory(string menuItemPath, HierarchyDesigner_ToolCategory category)
        {
            string[] parts = menuItemPath.Split('/');
            if (category == HierarchyDesigner_ToolCategory.Counting || category == HierarchyDesigner_ToolCategory.Selecting)
            {
                if (parts.Length > 4)
                {
                    return parts[parts.Length - 2] + "/" + parts[parts.Length - 1];
                }
            }
            else
            {
                if (parts.Length > 3)
                {
                    return parts[parts.Length - 1];
                }
            }
            return null;
        }

        private void ExecuteSelectedMethod()
        {
            if (availableActionMethods.Count > selectedActionIndex && selectedActionIndex >= 0)
            {
                MethodInfo methodToInvoke = availableActionMethods[selectedActionIndex];
                methodToInvoke?.Invoke(null, null);
            }
        }
    }
}
#endif