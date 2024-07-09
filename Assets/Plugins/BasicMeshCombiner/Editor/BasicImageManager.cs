using UnityEditor;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// Keeps track of al images that will be used with the basic mesh combiner tool.
    /// </summary>
    public class BasicImageManager
    {
        /// <summary>
        /// The titel Image of the basic mesh combiner.
        /// </summary>
        public static Texture TitelImage { get; private set; }

        /// <summary>
        /// The advertismentImage that will be shown in the popup.
        /// </summary>
        public static Texture AdvertismentImage { get; private set; }

        /// <summary>
        /// Gets al images that need to be set.
        /// </summary>
        public static void GetImages()
        {
            TitelImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/BasicMeshCombiner/Demo/Images/TitelImage.png", typeof(Texture));
            AdvertismentImage = (Texture)AssetDatabase.LoadAssetAtPath("Assets/BasicMeshCombiner/Demo/Images/advanced icon.png", typeof(Texture));
        }
    }
}
