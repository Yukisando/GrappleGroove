#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Manager_Tree
    {
        private static readonly string branchIcon_I_Path = "Hierarchy Designer Branch Icon I";
        private static readonly string branchIcon_L_Path = "Hierarchy Designer Branch Icon L";
        private static readonly string branchIcon_T_Path = "Hierarchy Designer Branch Icon T";
        private static readonly string branchIcon_End_Path = "Hierarchy Designer Branch Icon End";

        public static Texture2D BranchIcon_I
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(branchIcon_I_Path); }
        }

        public static Texture2D BranchIcon_L
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(branchIcon_L_Path); }
        }

        public static Texture2D BranchIcon_T
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(branchIcon_T_Path); }
        }

        public static Texture2D BranchIcon_End
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(branchIcon_End_Path); }
        }
    }
}
#endif