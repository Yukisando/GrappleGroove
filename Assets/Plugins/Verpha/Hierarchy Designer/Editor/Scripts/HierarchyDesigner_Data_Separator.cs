#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Data_Separator
    {
        #region Properties
        private const string SettingsFileName = "HierarchyDesigner_SavedData_Separators.json";
        public static Dictionary<string, HierarchyDesigner_Info_Separator> separators = new Dictionary<string, HierarchyDesigner_Info_Separator>();
        #endregion

        static HierarchyDesigner_Data_Separator()
        {
            LoadSeparators();
        }

        public static void SaveSeparators()
        {
            string dataFilePath = HierarchyDesigner_Shared_Data.GetDataFilePath(SettingsFileName);
            HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Separator> serializableList = new HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Separator>(new List<HierarchyDesigner_Info_Separator>(separators.Values));
            string json = JsonUtility.ToJson(serializableList, true);
            File.WriteAllText(dataFilePath, json);
            AssetDatabase.Refresh();
        }

        public static void LoadSeparators()
        {
            string dataFilePath = HierarchyDesigner_Shared_Data.GetDataFilePath(SettingsFileName);
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Separator> loadedSeparators = JsonUtility.FromJson<HierarchyDesigner_SerializableList<HierarchyDesigner_Info_Separator>>(json);
                separators.Clear();
                foreach (HierarchyDesigner_Info_Separator separator in loadedSeparators.items)
                {
                    separator.ImageType = HierarchyDesigner_Shared_EnumFilter.ParseEnum(separator.ImageType.ToString(), HierarchyDesigner_Info_Separator.BackgroundImageType.Classic);
                    separators[separator.Name] = separator;
                }
            }
        }
    }
}
#endif