#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Manager_Buttons
    {
        #region Properties
        private static readonly string lockIconPath = "Hierarchy Designer Lock Icon";
        #endregion

        public static Texture2D LockIcon
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(lockIconPath); }
        }
    }
}
#endif