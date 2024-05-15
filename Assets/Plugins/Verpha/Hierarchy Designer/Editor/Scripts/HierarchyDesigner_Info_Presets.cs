#if UNITY_EDITOR
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    [System.Serializable]
    public class HierarchyDesigner_Info_Presets
    {
        #region Properties
        public string presetName;
        public Color folderColor;
        public HierarchyDesigner_Info_Folder.FolderImageType folderImageType;
        public Color separatorTextColor;
        public Color separatorBackgroundColor;
        public FontStyle separatorFontStyle;
        public int separatorFontSize;
        public TextAnchor separatorTextAlignment;
        public HierarchyDesigner_Info_Separator.BackgroundImageType separatorBackgroundImageType;
        public Color tagTextColor;
        public FontStyle tagFontStyle;
        public int tagFontSize;
        public Color layerTextColor;
        public FontStyle layerFontStyle;
        public int layerFontSize;
        public Color treeColor;
        #endregion

        private HierarchyDesigner_Info_Presets(string name, Color folderColor, HierarchyDesigner_Info_Folder.FolderImageType folderImageType, Color separatorTextColor, Color separatorBackgroundColor, FontStyle separatorFontStyle, int separatorFontSize, TextAnchor separatorTextAlignment, HierarchyDesigner_Info_Separator.BackgroundImageType separatorBackgroundImageType, Color tagTextColor, FontStyle tagFontStyle, int tagFontSize, Color layerTextColor, FontStyle layerFontStyle, int layerFontSize, Color treeColor)
        {
            this.presetName = name;
            this.folderColor = folderColor;
            this.folderImageType = folderImageType;
            this.separatorTextColor = separatorTextColor;
            this.separatorBackgroundColor = separatorBackgroundColor;
            this.separatorFontStyle = separatorFontStyle;
            this.separatorFontSize = separatorFontSize;
            this.separatorTextAlignment = separatorTextAlignment;
            this.separatorBackgroundImageType = separatorBackgroundImageType;
            this.tagTextColor = tagTextColor;
            this.tagFontStyle = tagFontStyle;
            this.tagFontSize = tagFontSize;
            this.layerTextColor = layerTextColor;
            this.layerFontStyle = layerFontStyle;
            this.layerFontSize = layerFontSize;
            this.treeColor = treeColor;
        }

        #region Presets
        public static HierarchyDesigner_Info_Presets AzureDreamscapePreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Azure Dreamscape",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#318DCB"),
                HierarchyDesigner_Info_Folder.FolderImageType.ClassicOutline,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#7EBCEF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#3C5A81"),
                FontStyle.BoldAndItalic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedLeftAndRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#8E9FD5"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#8E9FD5"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#5A5485")
            );
        }

        public static HierarchyDesigner_Info_Presets BlackAndGoldPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Black and Gold",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFD102"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                FontStyle.BoldAndItalic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFC402")
            );
        }

        public static HierarchyDesigner_Info_Presets BlackAndWhitePreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Black and White",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ffffff"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                FontStyle.Bold,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ffffff80"),
                FontStyle.Italic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ffffff80"),
                FontStyle.Italic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF")
            );
        }

        public static HierarchyDesigner_Info_Presets BloodyMaryPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Bloody Mary",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C50515E6"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFE1"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#CF1625F0"),
                FontStyle.Bold,
                12,
                TextAnchor.UpperCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedBottom,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFEEAA9C"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFEEAA9C"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8")
            );
        }

        public static HierarchyDesigner_Info_Presets BlueHarmonyPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Blue Harmony",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#6AB1F8"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#277DEC"),
                FontStyle.Bold,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#6AB1F8F0"),
                FontStyle.Bold,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF"),
                FontStyle.Bold,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF")
            );
        }

        public static HierarchyDesigner_Info_Presets DeepOceanPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Deep Ocean",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1E4E8A"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#041F54C8"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#041F54"),
                FontStyle.Bold,
                12,
                TextAnchor.LowerRight,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#0E244E"),
                FontStyle.Bold,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#0E244E"),
                FontStyle.Bold,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1E4E8A")
            );
        }

        public static HierarchyDesigner_Info_Presets DunesPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Dunes",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E4C6AB"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AB673F"),
                FontStyle.Italic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1")
            );
        }

        public static HierarchyDesigner_Info_Presets MinimalBlackPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Minimal Black",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                HierarchyDesigner_Info_Folder.FolderImageType.ClassicOutline,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#3F3F3F"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                FontStyle.Bold,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Info_Separator.BackgroundImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000C8"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000C8"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000F0")
            );
        }

        public static HierarchyDesigner_Info_Presets MinimalWhitePreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Minimal White",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                HierarchyDesigner_Info_Folder.FolderImageType.ClassicOutline,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#BEBEBE"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                FontStyle.Bold,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Info_Separator.BackgroundImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8"),
                FontStyle.Italic,
                8,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFF0")
            );
        }

        public static HierarchyDesigner_Info_Presets NaturePreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Nature",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DFEAF0"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DFF6CA"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#70B879"),
                FontStyle.Normal,
                13,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD9A5C8"),
                FontStyle.Normal,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD9A5C8"),
                FontStyle.Normal,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#BCD8E3")
            );
        }

        public static HierarchyDesigner_Info_Presets NavyBlueLightPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Navy Blue Light",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#113065"),
                FontStyle.Bold,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6ECC8"),
                FontStyle.Bold,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6ECC8"),
                FontStyle.Bold,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC")
            );
        }

        public static HierarchyDesigner_Info_Presets OldSchoolPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Old School",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#686868"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#00FF34"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#010101"),
                FontStyle.Normal,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1FC742F0"),
                FontStyle.Normal,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1FC742F0"),
                FontStyle.Normal,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#686868")
            );
        }

        public static HierarchyDesigner_Info_Presets PrettyPinkPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Pretty Pink",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB6B90"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#EFEBE0"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570"),
                FontStyle.Bold,
                12,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570FA"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570FA"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570")
            );
        }

        public static HierarchyDesigner_Info_Presets RedDawnPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Red Dawn",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148"),
                HierarchyDesigner_Info_Folder.FolderImageType.Classic,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FF5F2A"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C00531"),
                FontStyle.BoldAndItalic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ClassicFadedLeftAndRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148F0"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148F0"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148")
            );
        }

        public static HierarchyDesigner_Info_Presets SunflowerPreset()
        {
            return new HierarchyDesigner_Info_Presets(
                "Sunflower",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                HierarchyDesigner_Info_Folder.FolderImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFC80A"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#2A8FF3"),
                FontStyle.Bold,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Info_Separator.BackgroundImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                FontStyle.BoldAndItalic,
                9,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701")
            );
        }
        #endregion
    }
}
#endif