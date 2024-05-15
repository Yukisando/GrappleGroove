#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Visual_Folder
    {
        #region Cached Properties
        private static readonly Texture2D backgroundImage = HierarchyDesigner_Manager_Background.BackgroundImage;
        private static readonly (Color folderColor, HierarchyDesigner_Info_Folder.FolderImageType folderImageType) defaultFolderInfo = (Color.white, HierarchyDesigner_Info_Folder.FolderImageType.Classic);
        private static Dictionary<int, WeakReference<GameObject>> folderCache = new Dictionary<int, WeakReference<GameObject>>();
        private static Dictionary<string, (Color folderColor, HierarchyDesigner_Info_Folder.FolderImageType folderImageType)> folderInfoCache = new Dictionary<string, (Color, HierarchyDesigner_Info_Folder.FolderImageType)>();
        #endregion

        static HierarchyDesigner_Visual_Folder()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
            UpdateFolderVisuals();
        }

        private static void HierarchyItemCB(int instanceID, Rect selectionRect)
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && HierarchyDesigner_Shared_EditorState.IsPlaying) return;
            if (Event.current.type != EventType.Repaint) return;
            if (!TryGetGameObject(instanceID, out GameObject gameObject)) return;

            (Color folderColor, HierarchyDesigner_Info_Folder.FolderImageType folderImageType) = GetFolderInfo(gameObject.name);
            DrawFolderIcon(selectionRect, folderColor, folderImageType, instanceID);
        }

        private static bool TryGetGameObject(int instanceID, out GameObject gameObject)
        {
            if (folderCache.TryGetValue(instanceID, out WeakReference<GameObject> weakRef) && weakRef.TryGetTarget(out gameObject))
            {
                return IsFolder(gameObject);
            }
            gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null && IsFolder(gameObject))
            {
                folderCache[instanceID] = new WeakReference<GameObject>(gameObject);
                return true;
            }
            return false;
        }

        public static bool IsFolder(GameObject gameObject)
        {
            if (gameObject == null) return false;
            return gameObject.GetComponent<HierarchyDesignerFolder>() != null;
        }

        private static void DrawFolderIcon(Rect selectionRect, Color folderColor, HierarchyDesigner_Info_Folder.FolderImageType folderImageType, int instanceID)
        {
            bool isHovering = Event.current.type == EventType.Repaint && selectionRect.Contains(Event.current.mousePosition);

            GUI.color = HierarchyDesigner_Shared_ColorUtility.GetBackgroundColor(isHovering, instanceID);
            if (backgroundImage != null)
            {
                float iconSize = selectionRect.height * 1.0f;
                float iconYPosition = selectionRect.y + (selectionRect.height - iconSize) / 2;
                Rect backgroundRect = new Rect(selectionRect.x, iconYPosition, iconSize, iconSize);
                GUI.DrawTexture(backgroundRect, backgroundImage);
            }
            GUI.color = Color.white;

            Texture2D folderIcon = HierarchyDesigner_Manager_Folder.GetFolderIcon(folderImageType);
            if (folderIcon != null)
            {
                GUI.color = folderColor;
                Rect iconRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.height, selectionRect.height);
                GUI.DrawTexture(iconRect, folderIcon);
                GUI.color = Color.white;
            }
        }

        private static (Color folderColor, HierarchyDesigner_Info_Folder.FolderImageType folderImageType) GetFolderInfo(string folderName)
        {
            if (!folderInfoCache.TryGetValue(folderName, out var info))
            {
                if (HierarchyFolderWindow.folders.TryGetValue(folderName, out HierarchyDesigner_Info_Folder folder))
                {
                    info = (folder.FolderColor, folder.ImageType);
                    folderInfoCache[folderName] = info;
                }
                else
                {
                    info = defaultFolderInfo;
                }
            }
            return info;
        }

        public static void UpdateFolderVisuals()
        {
            if (HierarchyDesigner_Data_Folder.folders.Count > 0)
            {
                folderInfoCache.Clear();
                foreach (var folder in HierarchyDesigner_Data_Folder.folders)
                {
                    folderInfoCache[folder.Key] = (folder.Value.FolderColor, folder.Value.ImageType);
                }
                EditorApplication.RepaintHierarchyWindow();
            }
        }
    }
}
#endif