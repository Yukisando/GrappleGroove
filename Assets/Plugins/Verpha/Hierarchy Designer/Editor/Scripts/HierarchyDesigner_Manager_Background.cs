#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Manager_Background
    {
        private static readonly string backgroundImagePath = "Hierarchy Designer Background Image";

        public static Texture2D BackgroundImage
        {
            get { return HierarchyDesigner_Shared_TextureLoader.LoadTexture(backgroundImagePath); }
        }

        public static Texture2D GetBackgroundImage(HierarchyDesigner_Info_Separator.BackgroundImageType backgroundImageType)
        {
            switch (backgroundImageType)
            {
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedTop:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Classic Faded Top");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedLeft:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Classic Faded Left");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedRight:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Classic Faded Right");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedBottom:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Classic Faded Bottom");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedLeftAndRight:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Classic Faded Left Right");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ModernI:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Modern I");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ModernII:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Modern II");
                case HierarchyDesigner_Info_Separator.BackgroundImageType.ModernIII:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image Modern III");
                default:
                    return HierarchyDesigner_Shared_TextureLoader.LoadTexture("Hierarchy Designer Background Image");
            }
        }
    }
}
#endif