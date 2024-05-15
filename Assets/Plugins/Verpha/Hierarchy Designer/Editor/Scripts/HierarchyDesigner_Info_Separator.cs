#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_Info_Separator
    {
        #region Properties
        [SerializeField] private string name = "";
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private Color backgroundColor = Color.gray;
        [SerializeField] private FontStyle fontStyle = FontStyle.Normal;
        [SerializeField] private int fontSize = 12;
        [SerializeField] private TextAnchor textAlignment = TextAnchor.MiddleCenter;
        [SerializeField] private BackgroundImageType backgroundImageType = BackgroundImageType.Classic;
        #endregion

        #region Accessors
        public string Name
        {
            get => name;
            set => name = value;
        }

        public Color TextColor
        {
            get => textColor;
            set => textColor = value;
        }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        public FontStyle FontStyle
        {
            get => fontStyle;
            set => fontStyle = value;
        }

        public int FontSize
        {
            get => fontSize;
            set => fontSize = value;
        }

        public TextAnchor TextAlignment
        {
            get => textAlignment; 
            set => textAlignment = value; 
        }

        public BackgroundImageType ImageType
        {
            get => backgroundImageType; 
            set => backgroundImageType = value; 
        }

        public enum BackgroundImageType 
        { 
            Classic,
            ClassicFadedTop,
            ClassicFadedLeft,
            ClassicFadedRight,
            ClassicFadedBottom,
            ClassicFadedLeftAndRight,
            ModernI,
            ModernII,
            ModernIII,
        }
        #endregion

        public HierarchyDesigner_Info_Separator(string name, Color textColor, Color backgroundColor, FontStyle fontStyle, int fontSize, TextAnchor textAlignment, BackgroundImageType backgroundImageType)
        {
            this.name = name;
            this.textColor = textColor;
            this.backgroundColor = backgroundColor;
            this.fontStyle = fontStyle;
            this.fontSize = fontSize;
            this.textAlignment = textAlignment;
            this.backgroundImageType = backgroundImageType;
        }
    }
}
#endif