#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Utility_Documentation
    {
        private const string DocumentationFileName = "Hierarchy Designer Documentation.pdf";

        [MenuItem("Hierarchy Designer/Hierarchy Designer Documentation", false, 150)]
        private static void OpenDocumentation()
        {
            string assetsPath = Application.dataPath;
            string[] allFiles = Directory.GetFiles(assetsPath, DocumentationFileName, SearchOption.AllDirectories);

            string documentationFile = null;
            foreach (string file in allFiles)
            {
                if (file.Contains("Hierarchy Designer") &&
                    file.Contains("Editor") &&
                    file.Contains("Documentation") &&
                    Path.GetFileName(file) == DocumentationFileName)
                {
                    documentationFile = file;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(documentationFile))
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = documentationFile, UseShellExecute = true });
                }
                catch (System.Exception ex)
                {
                    EditorUtility.DisplayDialog("Error", $"Failed to open documentation: {ex.Message}", "OK");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Documentation Not Found", $"Documentation file not found: {DocumentationFileName}", "OK");
            }
        }
    }
}
#endif