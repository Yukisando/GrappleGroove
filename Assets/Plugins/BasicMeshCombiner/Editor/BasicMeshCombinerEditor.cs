using DeTools.BasicMeshRenderer.Data;
using DeTools.BasicMeshRenderer.Panel;
using UnityEditor;
using UnityEngine;

namespace DeTools.BasicMeshRenderer.Window
{
    /// <summary>
    /// The editor of the basic mesh combiner.
    /// </summary>
    public class BasicMeshCombinerEditor : EditorWindow
    {
        /// <summary>
        /// Email adress of support.
        /// </summary>
        const string email = "detoolsassetstore@gmail.com";

        /// <summary>
        /// Front size of header
        /// </summary>
        const int headerSize = 13;
        /// <summary>
        /// Front size of content
        /// </summary>
        const int contentSize = 11;

        /// <summary>
        /// position of the current scroll position
        /// </summary>
        private Vector2 scrollPos = Vector2.zero;

        private void OnEnable()
        {
            BasicMeshCombinerFronts.GetFronts();
            BasicImageManager.GetImages();
            BasicMeshCombinerPreferences.SetSaves();
            Selection.selectionChanged += Repaint;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
        }

        public void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            GUILayout.BeginVertical();
            DrawTitel();
            AddHorizontalLine(Color.black);
            BasicMeshCombinerMain.DrawGUI();
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }


        /// <summary>
        /// Draws the titel of the editor tool.
        /// </summary>
        private void DrawTitel()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Width(665));
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(550), GUILayout.Height(50));
            GUILayout.BeginVertical();
            GUIStyle noGameObjectsStats = new GUIStyle();
            noGameObjectsStats.alignment = TextAnchor.MiddleCenter;
            GUILayout.BeginHorizontal();
            GUILayout.Box(BasicImageManager.TitelImage, GUILayout.Width(25), GUILayout.Height(25));
            GUILayout.Label("Basic Mesh Combiner");
            GUILayout.EndHorizontal();
            GUILayout.Label("Having trouble? or found an error or rare ocaition Email me at " + email);
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws an horizontal line in the given color
        /// </summary>
        public static void AddHorizontalLine(Color lineColor)
        {
            GUI.backgroundColor = lineColor;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUI.backgroundColor = Color.white;
        }

        /// <summary>
        /// Draws an vertical line in the given color
        /// </summary>
        public static void AddVerticalLine(Color lineColor)
        {
            GUI.backgroundColor = lineColor;
            EditorGUILayout.LabelField("", GUI.skin.verticalSlider);
            GUI.backgroundColor = Color.white;
        }

        /// <summary>
        /// Draws an Header text
        /// </summary>
        public static void DrawHeader(string headerText)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.fontSize = headerSize;
            GUILayout.Label(headerText);
            GUILayout.EndHorizontal();
            GUI.skin.label.fontSize = contentSize;
        }
    }
}