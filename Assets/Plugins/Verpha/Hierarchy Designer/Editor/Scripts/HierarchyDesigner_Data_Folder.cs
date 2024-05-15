#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Data_Folder
    {
        #region Properties
        private const string SettingsFileName = "HierarchyDesigner_SavedData_Folders.json";
        public static Dictionary<string, HierarchyDesigner_Info_Folder> folders = new Dictionary<string, HierarchyDesigner_Info_Folder>();
        #endregion

        static HierarchyDesigner_Data_Folder()
        {
            LoadFolders();
        }

        public static void SaveFolders()
        {
            string dataFilePath = HierarchyDesigner_Shared_Data.GetDataFilePath(SettingsFileName);
            HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Folder> serializableList = new HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Folder>(new List<HierarchyDesigner_Info_Folder>(folders.Values));
            string json = JsonUtility.ToJson(serializableList, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadFolders()
        {
            string dataFilePath = HierarchyDesigner_Shared_Data.GetDataFilePath(SettingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Folder> loadedFolders = JsonUtility.FromJson<HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Folder>>(json);
                folders.Clear();
                foreach (HierarchyDesigner_Info_Folder folder in loadedFolders.items)
                {
                    folder.ImageType = HierarchyDesigner_Shared_EnumFilter.ParseEnum(folder.ImageType.ToString(), HierarchyDesigner_Info_Folder.FolderImageType.Classic);
                    folders[folder.Name] = folder;
                }
            }
        }
    }
}
#endif