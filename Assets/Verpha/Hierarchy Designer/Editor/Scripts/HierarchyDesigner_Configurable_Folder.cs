#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Configurable_Folder
    {
        #region Properties
        [System.Serializable]
        public class HierarchyDesigner_FolderData
        {
            public string Name = "Folder";
            public Color Color = Color.white;
            public FolderImageType ImageType = FolderImageType.Default;
        }
        public enum FolderImageType { Default, DefaultOutline, DefaultOutline2X, ModernI }
        private const string settingsFileName = "HierarchyDesigner_SavedData_Folders.json";
        private static Dictionary<string, HierarchyDesigner_FolderData> folders = new Dictionary<string, HierarchyDesigner_FolderData>();
        #endregion

        #region Initialization Methods
        public static void Initialize()
        {
            LoadSettings();
            LoadHierarchyDesignerManagerGameObjectCaches();
        }

        private static void LoadHierarchyDesignerManagerGameObjectCaches()
        {
            var folderCache = new Dictionary<int, (Color folderColor, FolderImageType folderImageType)>();
            foreach (var folder in folders)
            {
                int instanceID = folder.Key.GetHashCode();
                folderCache[instanceID] = (folder.Value.Color, folder.Value.ImageType);
            }
            HierarchyDesigner_Manager_GameObject.FolderCache = folderCache;
        }
        #endregion

        #region Accessors
        public static void SetFoldersData(string folderName, Color color, FolderImageType imageType)
        {
            if (folders.ContainsKey(folderName))
            {
                folders[folderName].Color = color;
                folders[folderName].ImageType = imageType;
            }
            else
            {
                folders[folderName] = new HierarchyDesigner_FolderData
                {
                    Name = folderName,
                    Color = color,
                    ImageType = imageType
                };
            }
            SaveSettings();
            HierarchyDesigner_Manager_GameObject.ClearFolderCache();
        }

        public static HierarchyDesigner_FolderData GetFoldersData(string folderName)
        {
            if (folders.TryGetValue(folderName, out var folderData))
            {
                return folderData;
            }
            return null;
        }

        public static void RemoveFoldersData(string folderName)
        {
            if (folders.ContainsKey(folderName))
            {
                folders.Remove(folderName);
                SaveSettings();
            }
        }

        public static Dictionary<string, HierarchyDesigner_FolderData> GetAllFoldersData()
        {
            return new Dictionary<string, HierarchyDesigner_FolderData>(folders);
        }
        #endregion

        #region Save and Load
        public static void SaveSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData> serializableList = new HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData>(new List<HierarchyDesigner_FolderData>(folders.Values));
            string json = JsonUtility.ToJson(serializableList, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSettings()
        {
            string dataFilePath = HierarchyDesigner_Manager_Data.GetDataFilePath(settingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData> loadedFolders = JsonUtility.FromJson<HierarchyDesigner_Shared_SerializableList<HierarchyDesigner_FolderData>>(json);
                folders.Clear();
                foreach (HierarchyDesigner_FolderData folder in loadedFolders.items)
                {
                    folder.ImageType = HierarchyDesigner_Shared_EnumFilter.ParseEnum(folder.ImageType.ToString(), FolderImageType.Default);
                    folders[folder.Name] = folder;
                }
            }
            else
            {
                SetDefaultSettings();
            }
        }

        private static void SetDefaultSettings()
        {
            folders = new Dictionary<string, HierarchyDesigner_FolderData>();
        }
        #endregion
    }
}
#endif