#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_Info_Folder
    {
        #region Properties
        [SerializeField] private string name = "Folder";
        [SerializeField] private Color folderColor = Color.white;
        [SerializeField] private FolderImageType folderImageType = FolderImageType.Classic;
        #endregion

        #region Accessors
        public string Name
        {
            get => name;
            set => name = value;
        }

        public Color FolderColor
        {
            get => folderColor;
            set => folderColor = value;
        }

        public FolderImageType ImageType
        {
            get => folderImageType;
            set => folderImageType = value;
        }

        public enum FolderImageType
        {
            Classic,
            ClassicOutline,
            ClassicOutline2X,
            ModernI
        }
        #endregion

        public HierarchyDesigner_Info_Folder(string name, Color folderColor, FolderImageType folderImageType)
        {
            this.name = name;
            this.folderColor = folderColor;
            this.folderImageType = folderImageType;
        }
    }
}
#endif