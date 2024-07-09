using UnityEngine;

namespace DeTools.BasicMeshRenderer.Data
{
    /// <summary>
    /// Gets al fronts for the editor.
    /// </summary>
    public class BasicMeshCombinerFronts
    {
        /// <summary>
        /// Front SubTilulo1.
        /// </summary>
        public static GUIStyle SubTitulo { get; private set; }

        /// <summary>
        /// Front SubTilulo2.
        /// </summary>
        public static GUIStyle SubTitulo2 { get; private set; }

        /// <summary>
        /// Front SubTilulo3.
        /// </summary>
        public static GUIStyle SubTitulo3 { get; private set; }


        /// <summary>
        /// Front SubTilulo4.
        /// </summary>
        public static GUIStyle SubTitulo4 { get; private set; }


        /// <summary>
        /// Text that alines Left.
        /// </summary>
        public static GUIStyle LeftText { get; private set; }


        /// <summary>
        /// Text that alines midelCenter.
        /// </summary>
        public static GUIStyle MidelText { get; private set; }

        /// <summary>
        /// Text that alines Left.
        /// </summary>
        public static GUIStyle RightText { get; private set; }

        /// <summary>
        /// Gets al fronts for the Editor.
        /// </summary>
        public static void GetFronts()
        {
            SubTitulo = new GUIStyle();
            SubTitulo.fontSize = 20;
            SubTitulo.fontStyle = FontStyle.Bold;
            SubTitulo.alignment = TextAnchor.MiddleCenter;
            SubTitulo.normal.textColor = Color.white;

            SubTitulo2 = new GUIStyle();
            SubTitulo2.fontSize = 15;
            SubTitulo2.fontStyle = FontStyle.Bold;
            SubTitulo2.alignment = TextAnchor.MiddleCenter;
            SubTitulo2.normal.textColor = Color.white;


            SubTitulo3 = new GUIStyle();
            SubTitulo3.fontSize = 12;
            SubTitulo3.fontStyle = FontStyle.Bold;
            SubTitulo3.alignment = TextAnchor.MiddleCenter;
            SubTitulo3.normal.textColor = Color.white;


            SubTitulo4 = new GUIStyle();
            SubTitulo4.fontSize = 11;
            SubTitulo4.fontStyle = FontStyle.Bold;
            SubTitulo4.alignment = TextAnchor.MiddleCenter;
            SubTitulo4.normal.textColor = Color.white;




            LeftText = new GUIStyle();
            LeftText.alignment = TextAnchor.MiddleLeft;
            LeftText.normal.textColor = Color.white;

            MidelText = new GUIStyle();
            MidelText.alignment = TextAnchor.MiddleCenter;
            MidelText.normal.textColor = Color.white;

            RightText = new GUIStyle();
            RightText.alignment = TextAnchor.MiddleRight;
            RightText.normal.textColor = Color.white;
        }
    }
}