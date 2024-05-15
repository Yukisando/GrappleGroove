#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_Info_Layer
    {
        #region Properties
        private Color textColor = Color.gray;
        private FontStyle fontStyle = FontStyle.BoldAndItalic;
        private int fontSize = 9;
        #endregion

        #region Accessors
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set { fontStyle = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion

        public HierarchyDesigner_Info_Layer(Color textColor, FontStyle fontStyle, int fontSize)
        {
            this.textColor = textColor;
            this.fontStyle = fontStyle;
            this.fontSize = fontSize;
        }
    }
}
#endif