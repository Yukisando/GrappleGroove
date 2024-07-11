#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Utility_Presets
    {
        #region Presets
        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset AgeOfEnlightenmentPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Age of Enlightenment",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E2DAC1"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#464646"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFF9F4"),
                null,
                FontStyle.Normal,
                11,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ClassicI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#6C6C6C"),
                FontStyle.Italic,
                10,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAF1EA"),
                FontStyle.Italic,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#6C6C6C"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAF1EA"),
                10,
                FontStyle.Normal,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset AzureDreamscapePreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Azure Dreamscape",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#318DCB"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#7EBCEF"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#3C5A81"),
                null,
                FontStyle.BoldAndItalic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedSideways,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#8E9FD5"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#8E9FD5"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#5A5485"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#8E9FD5"),
                11,
                FontStyle.BoldAndItalic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset BlackAndGoldPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Black and Gold",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFD102"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                null,
                FontStyle.BoldAndItalic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1C1C1C"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFC402"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFC402"),
                11,
                FontStyle.BoldAndItalic,
                TextAnchor.MiddleRight
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset BlackAndWhitePreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Black and White",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ffffff"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                null,
                FontStyle.Bold,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ffffff80"),
                FontStyle.Italic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ffffff80"),
                FontStyle.Italic,
                9,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                11,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset BloodyMaryPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Bloody Mary",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C50515E6"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFE1"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#CF1625F0"),
                null,
                FontStyle.Bold,
                12,
                TextAnchor.UpperCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedBottom,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFEEAA9C"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFEEAA9C"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8"),
                11,
                FontStyle.Normal,
                TextAnchor.UpperCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset BlueHarmonyPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Blue Harmony",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#6AB1F8"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#277DEC"),
                null,
                FontStyle.Bold,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#6AB1F8F0"),
                FontStyle.Bold,
                8,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF"),
                FontStyle.Bold,
                9,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#A5D2FF"),
                11,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset DeepOceanPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Deep Ocean",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1E4E8A"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#041F54C8"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#041F54"),
                null,
                FontStyle.Bold,
                12,
                TextAnchor.LowerRight,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#213864"),
                FontStyle.Bold,
                8,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#213864"),
                FontStyle.Bold,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#213864"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#213864"),
                10,
                FontStyle.BoldAndItalic,
                TextAnchor.MiddleRight
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset DunesPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Dunes",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E4C6AB"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AB673F"),
                null,
                FontStyle.Italic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DDC0A4E1"),
                11,
                FontStyle.Italic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset FrostyFogPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Frosty Fog",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C4E7F3DC"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline2X,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E2F0F5"),
                true,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C7E6F1"),
                HierarchyDesigner_Shared_ColorUtility.CreateGradient(GradientMode.Blend, ("#A9DDEF", 255, 20f), ("#BDE7F5", 200, 50f), ("#DCF6FF", 120, 90f), ("DBEFF5", 100, 100f)),
                FontStyle.Italic,
                13,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#ACDEEF"),
                FontStyle.BoldAndItalic,
                10,
                TextAnchor.MiddleRight  ,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#9FA8AB"),
                FontStyle.Italic,
                11,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#CADCE2"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C4E5F1"),
                11,
                FontStyle.BoldAndItalic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset LittleRedPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Little Red",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E02D3C"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E02D3CF0"),
                null,
                FontStyle.Bold,
                11,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                FontStyle.Bold,
                10,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#D62E3C"),
                FontStyle.Bold,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                11,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset MinimalBlackPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Minimal Black",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#646464"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000"),
                null,
                FontStyle.Bold,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000C8"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000C8"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000F0"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#000000F0"),
                10,
                FontStyle.Normal,
                TextAnchor.UpperCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset MinimalWhitePreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Minimal White",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#9B9B9B"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                null,
                FontStyle.Bold,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFC8"),
                FontStyle.Italic,
                8,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFF0"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFFF0"),
                10,
                FontStyle.Normal,
                TextAnchor.UpperCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset NaturePreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Nature",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DFEAF0"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DFF6CA"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#70B879"),
                null,
                FontStyle.Normal,
                13,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD9A5C8"),
                FontStyle.Normal,
                9,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD9A5C8"),
                FontStyle.Normal,
                9,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#BCD8E3"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#BFDFB1"),
                11,
                FontStyle.Italic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset NavyBlueLightPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Navy Blue Light",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#113065"),
                null,
                FontStyle.Bold,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6ECC8"),
                FontStyle.Bold,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6ECC8"),
                FontStyle.Bold,
                9,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#AAD6EC"),
                11,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset OldSchoolPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Old School",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#686868"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#00FF34"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#010101"),
                null,
                FontStyle.Normal,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1FC742F0"),
                FontStyle.Normal,
                9,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1FC742F0"),
                FontStyle.Normal,
                9,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#686868"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#7D7D7D"),
                11,
                FontStyle.Normal,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset PrettyPinkPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Pretty Pink",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB6B90"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#EFEBE0"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570"),
                null,
                FontStyle.Bold,
                12,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernII,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570FA"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570FA"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FB4570"),
                11,
                FontStyle.BoldAndItalic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset PrismaticPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Prismatic",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#CCE5E5"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                true,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                #if UNITY_2022_3_OR_NEWER
                HierarchyDesigner_Shared_ColorUtility.CreateGradient(GradientMode.PerceptualBlend,("#2F7FFF", 155, 0f),("#72BFAF", 158, 35f),("E8CEE8", 162, 70f),("#FFFFFF", 165, 100f)),
                #else
                HierarchyDesigner_Shared_ColorUtility.CreateGradient(GradientMode.Blend, ("#2F7FFF", 155, 0f), ("#72BFAF", 158, 35f), ("E8CEE8", 162, 70f), ("#FFFFFF", 165, 100f)),
                #endif
                FontStyle.BoldAndItalic,
                12,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.NeoI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#9FD3E0"),
                FontStyle.BoldAndItalic,
                10,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#E09FAD"),
                FontStyle.BoldAndItalic,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                11,
                FontStyle.BoldAndItalic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset RedDawnPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Red Dawn",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FF5F2A"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#C00531"),
                null,
                FontStyle.BoldAndItalic,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedSideways,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148F0"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148F0"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#DF4148"),
                11,
                FontStyle.Italic,
                TextAnchor.MiddleRight
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset StrawberrySalmonPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Strawberry Salmon",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAD8D8"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.DefaultOutline2X,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAD8D8"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F87474"),
                null,
                FontStyle.Bold,
                11,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.NeoI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#D85E74"),
                FontStyle.Italic,
                10,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAD8D8"),
                FontStyle.Italic,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAD8D8"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FAD8D8"),
                11,
                FontStyle.Italic,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset SunflowerPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Sunflower",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFC80A"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#2A8FF3"),
                null,
                FontStyle.Bold,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.ModernI,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                FontStyle.BoldAndItalic,
                9,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                10,
                FontStyle.Bold,
                TextAnchor.MiddleCenter
            );
        }

        public static HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset WildcatsPreset()
        {
            return new HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset(
                "Wildcats",
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFCF28"),
                HierarchyDesigner_Configurable_Folder.FolderImageType.Default,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFCF28"),
                false,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#1D5098"),
                null,
                FontStyle.Bold,
                13,
                TextAnchor.MiddleCenter,
                HierarchyDesigner_Configurable_Separator.SeparatorImageType.DefaultFadedSideways,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                FontStyle.Bold,
                9,
                TextAnchor.MiddleRight,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFCF28"),
                FontStyle.BoldAndItalic,
                10,
                TextAnchor.MiddleLeft,
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#FFFFFF"),
                HierarchyDesigner_Shared_ColorUtility.HexToColor("#F8B701"),
                11,
                FontStyle.BoldAndItalic,
                TextAnchor.UpperCenter
            );
        }
        #endregion

        #region Methods
        public static void ApplyPresetToFolders(HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset preset)
        {
            Dictionary<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> foldersData = HierarchyDesigner_Configurable_Folder.GetAllFoldersData(false);
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Folder.HierarchyDesigner_FolderData> folder in foldersData)
            {
                HierarchyDesigner_Configurable_Folder.SetFolderData(folder.Key, preset.folderColor, preset.folderImageType);
            }
        }

        public static void ApplyPresetToSeparators(HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset preset)
        {
            Dictionary<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separatorsData = HierarchyDesigner_Configurable_Separator.GetAllSeparatorsData(false);
            foreach (KeyValuePair<string, HierarchyDesigner_Configurable_Separator.HierarchyDesigner_SeparatorData> separator in separatorsData)
            {
                HierarchyDesigner_Configurable_Separator.SetSeparatorData(separator.Key,
                    preset.separatorTextColor, 
                    preset.separatorIsGradientBackground,
                    preset.separatorBackgroundColor, 
                    preset.separatorBackgroundGradient,
                    preset.separatorFontSize, 
                    preset.separatorFontStyle, 
                    preset.separatorTextAlignment, 
                    preset.separatorBackgroundImageType);
            }
        }

        public static void ApplyPresetToTag(HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset preset)
        {
            HierarchyDesigner_Configurable_DesignSettings.TagColor = preset.tagTextColor;
            HierarchyDesigner_Configurable_DesignSettings.TagFontStyle = preset.tagFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.TagFontSize = preset.tagFontSize;
            HierarchyDesigner_Configurable_DesignSettings.TagTextAnchor = preset.tagTextAnchor;
        }

        public static void ApplyPresetToLayer(HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset preset)
        {
            HierarchyDesigner_Configurable_DesignSettings.LayerColor = preset.layerTextColor;
            HierarchyDesigner_Configurable_DesignSettings.LayerFontStyle = preset.layerFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.LayerFontSize = preset.layerFontSize;
            HierarchyDesigner_Configurable_DesignSettings.LayerTextAnchor = preset.layerTextAnchor;
        }

        public static void ApplyPresetToTree(HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset preset)
        {
            HierarchyDesigner_Configurable_DesignSettings.HierarchyTreeColor = preset.treeColor;
        }

        public static void ApplyPresetToLockLabel(HierarchyDesigner_Configurable_Presets.HierarchyDesigner_Preset preset)
        {
            HierarchyDesigner_Configurable_DesignSettings.LockColor = preset.lockColor;
            HierarchyDesigner_Configurable_DesignSettings.LockFontSize = preset.lockFontSize;
            HierarchyDesigner_Configurable_DesignSettings.LockFontStyle = preset.lockFontStyle;
            HierarchyDesigner_Configurable_DesignSettings.LockTextAnchor = preset.lockTextAnchor;
        }
        #endregion
    }
}
#endif