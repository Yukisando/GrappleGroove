using DeTools.BasicMeshRenderer.Ads;
using UnityEditor;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Window
{
	/// <summary>
	/// The base of the editor tool here the window will be drawled from and also the correct size
	/// </summary>
	public class BasicMeshCombinerWindow : EditorWindow
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
        /// string of the tool name
        /// </summary>
        const string ToolName = "Basic Mesh Combiner";

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

		[MenuItem(window+deTools+ToolName)]
		private static void Init()
		{
			BasicMeshCombinerEditor window = GetWindow<BasicMeshCombinerEditor>();
			window.titleContent = new GUIContent(ToolName);
            window.maxSize = new Vector2(width * maxMultiplication, height * maxMultiplication);
            window.minSize = window.maxSize;
			window.minSize = new Vector2(width, height);
            window.Show();
            BasicMeshCombinerPopup .RollPopup();
        }

        [MenuItem("GameObject/Basic Mesh Combiner",false,0)]
        private static void MenuInit()
        {
            BasicMeshCombinerEditor window = GetWindow<BasicMeshCombinerEditor>();
            window.titleContent = new GUIContent(ToolName);
            window.maxSize = new Vector2(width * maxMultiplication, height * maxMultiplication);
            window.minSize = window.maxSize;
            window.minSize = new Vector2(width, height);
            window.Show();
            BasicMeshCombinerPopup.RollPopup();
        }
    }
}