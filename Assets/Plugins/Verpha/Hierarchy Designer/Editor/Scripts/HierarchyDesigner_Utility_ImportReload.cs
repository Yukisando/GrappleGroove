#if UNITY_EDITOR
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public class HierarchyDesigner_Utility_ImportReload : AssetPostprocessor
    {
        #region Properties
        static string ReloadedAssetsSessionStateName = "HierarchyDesigner_ImportReload";
        static string SpecificScriptToReimport = "HierarchyDesigner_Shared_TextureLoader";
        static string ReimportAttemptCountKey = "HierarchyDesigner_ReimportAttemptCount";
        static int MaxReimportAttempts = 2;
        #endregion

        static HierarchyDesigner_Utility_ImportReload()
        {
            EditorApplication.delayCall += CheckAndReloadTexturesOnStartup;
        }

        private static void CheckAndReloadTexturesOnStartup()
        {
            if (!TexturesLoaded())
            {
                int attemptCount = SessionState.GetInt(ReimportAttemptCountKey, 0);
                if (attemptCount < MaxReimportAttempts)
                {
                    PerformReimport();
                    SessionState.SetInt(ReimportAttemptCountKey, ++attemptCount);
                }
                else
                {
                    UnityEngine.Debug.Log("HierarchyDesigner: Max texture reimport attempts reached. Please check texture files in the resources folder.");
                }
            }
            else
            {
                SessionState.SetInt(ReimportAttemptCountKey, 0);
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (SessionState.GetBool(ReloadedAssetsSessionStateName, false) || TexturesLoaded()) { return; }
            foreach (string asset in importedAssets)
            {
                if (asset.Contains(SpecificScriptToReimport))
                {
                    PerformReimport();
                    SessionState.SetBool(ReloadedAssetsSessionStateName, true);
                    break;
                }
            }
        }

        private static bool TexturesLoaded()
        {
            return HierarchyDesigner_Shared_TextureLoader.IsTextureLoaded("Hierarchy Designer Branch Icon I") ||
            HierarchyDesigner_Shared_TextureLoader.IsTextureLoaded("Hierarchy Designer Branch Icon L") ||
            HierarchyDesigner_Shared_TextureLoader.IsTextureLoaded("Hierarchy Designer Branch Icon T") ||
            HierarchyDesigner_Shared_TextureLoader.IsTextureLoaded("Hierarchy Designer Branch Icon End") ||
            HierarchyDesigner_Shared_TextureLoader.IsTextureLoaded("Hierarchy Designer Background Image");
        }

        private static void PerformReimport()
        {
            string[] guids = AssetDatabase.FindAssets(SpecificScriptToReimport + " t:Script");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                AssetDatabase.ImportAsset(path);
            }
        }
    }
}
#endif