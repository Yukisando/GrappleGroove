using DeTools.Ads;
using DeTools.BasicMeshRenderer.Data;
using UnityEditor;
using UnityEngine;


namespace DeTools.BasicMeshRenderer.Ads
{
    /// <summary>
    /// An class that manages the advertisment popup.
    /// </summary>
    public class BasicMeshCombinerPopup : EditorWindow
    {
        /// <summary>
        /// String of Window name witch is the main tab
        /// </summary>
        const string window = "Tools/";
        /// <summary>
        /// String of detools name witch is company name
        /// </summary>
        const string deTools = "DeTools/";
        /// <summary>
        /// String of PopUp name witch is PopUp
        /// </summary>
        const string PopUpString = "PopUp/";
        /// <summary>
        /// string of the tool name
        /// </summary>
        const string popUpName = "PopUPMeshCombiner";

        /// <summary>
        /// height of the tool that may be drawn this is the min the max will be calculated with the maxMultiplication
        /// </summary>
        const int height = 550;

        /// <summary>
        /// Width of the tool that may be drawn this is the min the max will be calculated with the maxMultiplication
        /// </summary>
        const int width = 450;

        /// <summary>
        /// maxMultiplication is the multiplication factor for the max rect size of the tool window
        /// </summary>
        const float maxMultiplication = 1.5f;

        /// <summary>
        /// The advertisment link that will be an rederect if button is pressed.
        /// </summary>
        const string advertismentUrl = "https://assetstore.unity.com/packages/slug/252290";

        /// <summary>
        /// The chance of the popup to spawn now is set at 20%.
        /// </summary>
        private const int chanceToSpawn = 20;

        /// <summary>
        /// Static variable of BasicMeshCombinerPopup so that the window also can be closed.
        /// </summary>
        private static BasicMeshCombinerPopup currentOpenWindow;

        /// <summary>
        /// Void that rolls the chances of the popup showing.
        /// </summary>
        public static void RollPopup()
        {
            if (advertismentUrl == string.Empty||!BasicAds.RunAds)
                return;


            int randomNumber = Random.Range(0, 100);

            if (randomNumber <= chanceToSpawn)
            {
                PopUp();
            }
        }

        /// <summary>
        /// Show the popup window.
        /// </summary>
        [MenuItem(window + deTools + PopUpString + popUpName, true)]
        public static void PopUp()
        {
            if (currentOpenWindow != null)
            {
                currentOpenWindow.Close();
            }
            currentOpenWindow = GetWindow<BasicMeshCombinerPopup>();
            currentOpenWindow.titleContent = new GUIContent("Pop Up");
            currentOpenWindow.maxSize = new Vector2(width * maxMultiplication, height * maxMultiplication);
            currentOpenWindow.minSize = currentOpenWindow.maxSize;
            currentOpenWindow.minSize = new Vector2(width, height);
            currentOpenWindow.Show();
        }

        /// <summary>
        /// Basic gui.
        /// </summary>
        private void OnGUI()
        {
            DrawAdvertisment();
        }

        /// <summary>
        /// Function that draw the popup.
        /// </summary>
        private void DrawAdvertisment()
        {
            GUILayout.Space(10);
            GUILayout.Label("Pop Up", BasicMeshCombinerFronts.SubTitulo);
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.Box(BasicImageManager.AdvertismentImage, GUILayout.Width(750), GUILayout.Height(400));
            GUILayout.EndVertical();
            if (GUILayout.Button("Open Store Page", GUILayout.Height(70)))
            {
                if (advertismentUrl != string.Empty)
                    Application.OpenURL(advertismentUrl);

            }
            GUILayout.FlexibleSpace();
            GUILayout.Label("Sorry for popup if you never want to have a popup ever open the script BasicAds and set run Ads to fals", BasicMeshCombinerFronts.MidelText);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", GUILayout.Height(30)))
            {
                if (currentOpenWindow != null)
                {
                    currentOpenWindow.Close();
                }
            }
        }
    }

}