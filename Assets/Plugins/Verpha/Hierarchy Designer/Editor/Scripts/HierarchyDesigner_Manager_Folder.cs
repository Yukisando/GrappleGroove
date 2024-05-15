#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Manager_Folder
    {
        #region Properties
        private static readonly string inspectorFolderIconPath = "Hierarchy Designer Folder Icon Inspector";
        #endregion

        public static Texture2D GetFolderIcon(HierarchyDesigner_Info_Folder.FolderImageType folderImageType)
        {
            switch (folderImageType)
            {
                case HierarchyDesigner_Info_Folder.FolderImageType.ClassicOutline:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Folder Icon Classic Outline");
                case HierarchyDesigner_Info_Folder.FolderImageType.ClassicOutline2X:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Folder Icon Classic Outline Double");
                case HierarchyDesigner_Info_Folder.FolderImageType.ModernI:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Folder Icon Modern I");
                default:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Folder Icon Classic");
            }
        }

        public static Texture2D InspectorFolderIcon
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(inspectorFolderIconPath); }
        }

    }
}
#endif