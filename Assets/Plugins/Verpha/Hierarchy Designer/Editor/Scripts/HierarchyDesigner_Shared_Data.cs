#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_Data
    {
        #region Path Properties
        private const string DataFolderName = "Hierarchy Designer";
        private const string EditorFolderName = "Editor";
        private const string SavedDataFolderName = "Saved Data";
        #endregion

        public static string GetDataFilePath(string fileName)
        {
            string rootPath = FindHierarchyDesignerRootPath();
            string fullPath = Path.Combine(rootPath, EditorFolderName, SavedDataFolderName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                AssetDatabase.Refresh();
            }
            return Path.Combine(fullPath, fileName);
        }

        private static string FindHierarchyDesignerRootPath()
        {
            string[] guids = AssetDatabase.FindAssets(DataFolderName);
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Directory.Exists(path) && path.EndsWith(DataFolderName))
                {
                    return path;
                }
            }

            return Path.Combine(Application.dataPath, DataFolderName);
        }
    }
}
#endif