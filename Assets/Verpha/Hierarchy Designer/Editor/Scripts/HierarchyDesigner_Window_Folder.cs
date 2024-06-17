#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_Folder : EditorWindow
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
        private const float folderCreationLabelWidth = 100;
        private const float extraFolderLabelWidthOffset = 10;
        #endregion
        #region Temporary Values
        private Dictionary<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> tempFolders;
        private static bool hasModifiedChanges = false;
        #endregion
        #region Folder Creation Values
        private string newFolderName = "";
        private Color newFolderIconColor = Color.white;
        private HierarchyDesigner_Configurable_Folder.FolderImageType newFolderImageType = HierarchyDesigner_Configurable_Folder.FolderImageType.Default;
        #endregion
        #region Folder Global Fields Values
        private Color tempGlobalFolderIconColor = Color.white;
        private HierarchyDesigner_Configurable_Folder.FolderImageType tempGlobalFolderImageType = HierarchyDesigner_Configurable_Folder.FolderImageType.Default;
        #endregion
        #endregion

        #region Menu Item
        [MenuItem(HierarchyDesigner_Shared_MenuItems.Group_Folder + "/Folder Manager Window", false, HierarchyDesigner_Shared_MenuItems.LayerTwo)]
        public static void OpenWindow()
        {
            HierarchyDesigner_Window_Folder window = GetWindow<HierarchyDesigner_Window_Folder>("Hierarchy Folder Manager");
            window.minSize = new Vector2(400, 200);
        }
        #endregion

        #region Initialization
        private void OnEnable()
        {
            LoadTempValues();
        }

        private void LoadTempValues()
        {
            tempFolders = HierarchyDesigner_Configurable_Folder.GetAllFoldersData();
        }
        #endregion

        private void OnGUI()
        {
            HierarchyDesigner_Shared_GUI.DrawGUIStyles(out headerGUIStyle, out contentGUIStyle, out messageGUIStyle, out outerBackgroundGUIStyle, out innerBackgroundGUIStyle, out contentBackgroundGUIStyle);

            #region Header
            EditorGUILayout.BeginVertical(outerBackgroundGUIStyle);
            EditorGUILayout.LabelField("Folders Manager", headerGUIStyle);
            GUILayout.Space(8);
            #endregion

            outerScroll = EditorGUILayout.BeginScrollView(outerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical(innerBackgroundGUIStyle);

            #region Main
            #region Folder Creation
            EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
            EditorGUILayout.LabelField("Folder Creation:", contentGUIStyle);
            GUILayout.Space(4);
            using (new HierarchyDesigner_Shared_GUI.LabelWidth(folderCreationLabelWidth))
            {
                newFolderName = EditorGUILayout.TextField("Name", newFolderName);
                newFolderIconColor = EditorGUILayout.ColorField("Color", newFolderIconColor);
                newFolderImageType = (HierarchyDesigner_Configurable_Folder.FolderImageType)EditorGUILayout.EnumPopup("Image Type", newFolderImageType);
            }
            GUILayout.Space(4);
            if (GUILayout.Button("Create Folder", GUILayout.Height(25)))
            {
                if (IsFolderNameValid(newFolderName))
                {
                    CreateFolder(newFolderName, newFolderIconColor, newFolderImageType);
                }
                else
                {
                    EditorUtility.DisplayDialog("Invalid Folder Name", "Folder name is either duplicate or invalid.", "OK");
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
            #endregion

            if (tempFolders.Count > 0)
            {
                #region Global Fields
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Folders' Global Fields", contentGUIStyle);
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                tempGlobalFolderIconColor = EditorGUILayout.ColorField(tempGlobalFolderIconColor, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalFolderIconColor(tempGlobalFolderIconColor);
                }
                EditorGUI.BeginChangeCheck();
                tempGlobalFolderImageType = (HierarchyDesigner_Configurable_Folder.FolderImageType)EditorGUILayout.EnumPopup(tempGlobalFolderImageType, GUILayout.Width(150));
                if (EditorGUI.EndChangeCheck())
                {
                    UpdateGlobalFolderImageType(tempGlobalFolderImageType);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                #endregion

                #region Folder List
                EditorGUILayout.BeginVertical(contentBackgroundGUIStyle);
                EditorGUILayout.LabelField("Folders' List", contentGUIStyle);
                innerScroll = EditorGUILayout.BeginScrollView(innerScroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                GUILayout.Space(10);
                foreach (var folder in tempFolders)
                {
                    EditorGUI.BeginChangeCheck();
                    DrawFolders(folder.Key, folder.Value);
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
                EditorGUILayout.LabelField("No folders found. Please create a new folder.", messageGUIStyle);
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
                bool shouldSave = EditorUtility.DisplayDialog("Folder(s) Have Been Modified",
                    "Do you want to save the changes you made to the folders?",
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
            HierarchyDesigner_Configurable_Folder.SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearFolderCache();
            hasModifiedChanges = false;
        }

        #region Operations
        private bool IsFolderNameValid(string folderName)
        {
            return !string.IsNullOrEmpty(folderName) && !tempFolders.ContainsKey(folderName);
        }

        private void CreateFolder(string folderName, Color color, HierarchyDesigner_Configurable_Folder.FolderImageType imageType)
        {
            HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData newFolderData = new HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData
            {
                Name = folderName,
                Color = color,
                ImageType = imageType
            };
            tempFolders[folderName] = newFolderData;
            HierarchyDesigner_Configurable_Folder.SetFoldersData(folderName, color, imageType);
            tempFolders = HierarchyDesigner_Configurable_Folder.GetAllFoldersData();
            newFolderName = "";
            newFolderIconColor = Color.white;
            newFolderImageType = HierarchyDesigner_Configurable_Folder.FolderImageType.Default;
            hasModifiedChanges = true;
            GUI.FocusControl(null);
        }

        private void DrawFolders(string key, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData)
        {
            float folderLabelWidth = HierarchyDesigner_Shared_GUI.CalculateMaxLabelWidth(tempFolders.Keys);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(folderData.Name, GUILayout.Width(folderLabelWidth + extraFolderLabelWidthOffset));
            folderData.Color = EditorGUILayout.ColorField(folderData.Color, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));
            folderData.ImageType = (HierarchyDesigner_Configurable_Folder.FolderImageType)EditorGUILayout.EnumPopup(folderData.ImageType, GUILayout.Width(150));

            if (GUILayout.Button("Create", GUILayout.Width(60)))
            {
                CreateFolderGameObject(folderData);
            }
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                RemoveFolder(key);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void CreateFolderGameObject(HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folderData)
        {
            GameObject folder = new GameObject(folderData.Name);
            folder.AddComponent<HierarchyDesignerFolder>();

            Undo.RegisterCreatedObjectUndo(folder, $"Create {folderData.Name}");

            Texture2D inspectorIcon = HierarchyDesigner_Shared_Resources.FolderInspectorIcon;
            if (inspectorIcon != null)
            {
                EditorGUIUtility.SetIconForObject(folder, inspectorIcon);
            }
        }

        private void RemoveFolder(string folderName)
        {
            if (tempFolders.ContainsKey(folderName))
            {
                tempFolders.Remove(folderName);
                HierarchyDesigner_Configurable_Folder.RemoveFoldersData(folderName);
                GUIUtility.ExitGUI();
            }
        }

        private void UpdateGlobalFolderIconColor(Color color)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.Color = color;
            }
            hasModifiedChanges = true;
        }

        private void UpdateGlobalFolderImageType(HierarchyDesigner_Configurable_Folder.FolderImageType imageType)
        {
            foreach (HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData folder in tempFolders.Values)
            {
                folder.ImageType = imageType;
            }
            hasModifiedChanges = true;
        }
        #endregion
    }
}
#endif