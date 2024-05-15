#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Utility_Tools
    {
        #region MenuItem Properties
        private const string MenuPath = "Hierarchy Designer/Hierarchy Tools/";
        private const int priorityBase = 50;
        private const int priorityDivisor = 11;
        private const int priorityDivisorTwo = 22;
        private const int priorityDivisorThree = 35;
        #endregion

        #region Counting
        #region 2D Object
        [MenuItem(MenuPath + "Counting/2D Object/Count All Sprites", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSprites() => CountAllOfComponentType<SpriteRenderer>("Sprites");

        [MenuItem(MenuPath + "Counting/2D Object/Count All 9-Sliced", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAll9SlicedSprites() => CountAll2DSpritesByType("9-Sliced");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Capsules", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCapsuleSprites() => CountAll2DSpritesByType("Capsule");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Circles", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCircleSprites() => CountAll2DSpritesByType("Circle");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Hexagon Flat-Tops", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllHexagonFlatTopSprites() => CountAll2DSpritesByType("Hexagon Flat-Top");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Hexagon Pointed-Tops", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllHexagonPointedTopSprites() => CountAll2DSpritesByType("Hexagon Pointed-Top");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Isometric Diamonds", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllIsometricDiamondSprites() => CountAll2DSpritesByType("Isometric Diamond");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Squares", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSquareSprites() => CountAll2DSpritesByType("Square");

        [MenuItem(MenuPath + "Counting/2D Object/Count All Triangles", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllTriangleSprites() => CountAll2DSpritesByType("Triangle");
        #endregion

        #region 3D Object
        [MenuItem(MenuPath + "Counting/3D Object/Count All Mesh Renderers", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllMeshRenderers() => CountAllOfComponentType<MeshRenderer>("Mesh Renderers");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Cubes", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCubes() => CountAll3DObjects("Cube");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Spheres", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSpheres() => CountAll3DObjects("Sphere");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Capsules", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCapsules() => CountAll3DObjects("Capsule");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Cylinders", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCylinders() => CountAll3DObjects("Cylinder");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Planes", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllPlanes() => CountAll3DObjects("Plane");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Quads", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllQuads() => CountAll3DObjects("Quad");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Terrains", false, priorityBase + priorityDivisorTwo + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllTerrains() => CountAllOfComponentType<Terrain>("Terrains");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Wind Zones", false, priorityBase + priorityDivisorTwo + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllWindZones() => CountAllOfComponentType<WindZone>("Wind Zones");

        [MenuItem(MenuPath + "Counting/3D Object/Count All Skinned Mesh Renderers", false, priorityBase + priorityDivisorTwo + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSkinnedMeshRenderers() => CountAllOfComponentType<SkinnedMeshRenderer>("Skinned Mesh Renderers");
        #endregion

        #region Audio
        [MenuItem(MenuPath + "Counting/Audio/Count All Audio Sources", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllAudioSources() => CountAllOfComponentType<AudioSource>("Audio Sources");

        [MenuItem(MenuPath + "Counting/Audio/Count All Audio Reverb Zones", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllAudioReverbZones() => CountAllOfComponentType<AudioReverbZone>("Audio Reverb Zones");
        #endregion

        #region Effects
        [MenuItem(MenuPath + "Counting/Effects/Count All Particle Systems", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllParticleSystems() => CountAllOfComponentType<ParticleSystem>("Particle Systems");

        [MenuItem(MenuPath + "Counting/Effects/Count All Particle System Force Fields", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllParticleSystemForceFields() => CountAllOfComponentType<ParticleSystemForceField>("Particle System Force Fields");

        [MenuItem(MenuPath + "Counting/Effects/Count All Trails", false, priorityBase + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllTrails() => CountAllOfComponentType<TrailRenderer>("Trails");

        [MenuItem(MenuPath + "Counting/Effects/Count All Lines", false, priorityBase + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllLines() => CountAllOfComponentType<LineRenderer>("Lines");
        #endregion

        #region Light
        [MenuItem(MenuPath + "Counting/Light/Count All Lights", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllLights() => CountAllOfComponentType<Light>("Lights");

        [MenuItem(MenuPath + "Counting/Light/Count All Directional Lights", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllDirectionalLights() => CountAllOfComponentType<Light>("Directional Lights", light => light.type == LightType.Directional);

        [MenuItem(MenuPath + "Counting/Light/Count All Point Lights", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllPointLights() => CountAllOfComponentType<Light>("Point Lights", light => light.type == LightType.Point);

        [MenuItem(MenuPath + "Counting/Light/Count All Spot Lights", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSpotLights() => CountAllOfComponentType<Light>("Spot Lights", light => light.type == LightType.Spot);

        [MenuItem(MenuPath + "Counting/Light/Count All Area Lights", false, priorityBase + priorityDivisor + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllAreaLights() => CountAllOfComponentType<Light>("Area Lights", light => light.type == LightType.Rectangle);

        [MenuItem(MenuPath + "Counting/Light/Count All Reflection Probes", false, priorityBase + priorityDivisorTwo + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllReflectionProbes() => CountAllOfComponentType<ReflectionProbe>("Reflection Probes");

        [MenuItem(MenuPath + "Counting/Light/Count All Light Probe Groups", false, priorityBase + priorityDivisorTwo + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllLightProbeGroups() => CountAllOfComponentType<LightProbeGroup>("Light Probe Groups");
        #endregion

        #region Video
        [MenuItem(MenuPath + "Counting/Video/Count All Video Players", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllVideoPlayers() => CountAllOfComponentType<UnityEngine.Video.VideoPlayer>("Video Players");
        #endregion

        #region UI
        [MenuItem(MenuPath + "Counting/UI/Count All Images", false, priorityBase + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllImages() => CountAllOfComponentType<Image>("Images");

        [MenuItem(MenuPath + "Counting/UI/Count All Raw Images", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllRawImages() => CountAllOfComponentType<RawImage>("Raw Images");

        [MenuItem(MenuPath + "Counting/UI/Count All Buttons", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllButtons() => CountAllOfComponentType<Button>("Buttons");

        [MenuItem(MenuPath + "Counting/UI/Count All Toggles", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllToggles() => CountAllOfComponentType<Toggle>("Toggles");

        [MenuItem(MenuPath + "Counting/UI/Count All Sliders", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSliders() => CountAllOfComponentType<Slider>("Sliders");

        [MenuItem(MenuPath + "Counting/UI/Count All Scrollbars", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllScrollbars() => CountAllOfComponentType<Scrollbar>("Scrollbars");

        [MenuItem(MenuPath + "Counting/UI/Count All Scroll Views", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllScrollViews() => CountAllOfComponentType<ScrollRect>("Scroll Rects");

        #region TMP
        private static void CountTMPComponentIfAvailable<T>() where T : Component
        {
            if (IsTMPAvailable())
            {
                CountAllOfComponentType<T>("TMP Component");
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }

        [MenuItem(MenuPath + "Counting/UI/Count All Texts - TextMeshPro", false, priorityBase + priorityDivisorTwo + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllTextMeshProTexts() => CountTMPComponentIfAvailable<TMPro.TMP_Text>();

        [MenuItem(MenuPath + "Counting/UI/Count All Dropdowns - TextMeshPro", false, priorityBase + priorityDivisorTwo + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllTextMeshProDropdowns() => CountTMPComponentIfAvailable<TMPro.TMP_Dropdown>();

        [MenuItem(MenuPath + "Counting/UI/Count All Input Fields - TextMeshPro", false, priorityBase + priorityDivisorTwo + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllTextMeshProInputFields() => CountTMPComponentIfAvailable<TMPro.TMP_InputField>();
        #endregion

        [MenuItem(MenuPath + "Counting/UI/Count All Canvases", false, priorityBase + priorityDivisorThree + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCanvases() => CountAllOfComponentType<Canvas>("Canvases");

        [MenuItem(MenuPath + "Counting/UI/Count All Event Systems", false, priorityBase + priorityDivisorThree + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllEventSystems() => CountAllOfComponentType<UnityEngine.EventSystems.EventSystem>("Event Systems");
        #endregion

        #region Camera
        [MenuItem(MenuPath + "Counting/Camera/Count All Cameras", false, priorityBase + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllCameras() => CountAllOfComponentType<Camera>("Cameras");
        #endregion

        #region Others
        [MenuItem(MenuPath + "Counting/Others/Count All MonoBehaviours", false, priorityBase + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllMonoBehaviours() => CountAllOfComponentsType<MonoBehaviour>("MonoBehaviour (Scripts)");
        #endregion

        #region Integrations
        [MenuItem(MenuPath + "Counting/Integrations/Count All Folders", false, priorityBase + priorityDivisor + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllFolders() => CountAllOfComponentType<HierarchyDesignerFolder>("Hierarchy Designer Folders");

        [MenuItem(MenuPath + "Counting/Integrations/Count All Separators", false, priorityBase + priorityDivisor + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Counting)]
        public static void CountAllSeparators() => CountAllSeparatorsInScene();
        #endregion
        #endregion

        #region Locking
        [MenuItem(MenuPath + "Locking/Lock All GameObject Parents", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void LockAllGameObjectParents() => SetLockStateForAllGameObjectParents(true);

        [MenuItem(MenuPath + "Locking/Lock All GameObjects", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void LockAllGameObjects() => SetLockStateForAllGameObjects(true);

        [MenuItem(MenuPath + "Locking/Lock GameObject", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void LockGameObject() => ToggleGameObjectLock(true);

        [MenuItem(MenuPath + "Locking/Unlock All GameObject Parents", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void UnlockAllGameObjectParents() => SetLockStateForAllGameObjectParents(false);

        [MenuItem(MenuPath + "Locking/Unlock All GameObjects", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void UnlockAllGameObjects() => SetLockStateForAllGameObjects(false);

        [MenuItem(MenuPath + "Locking/Unlock GameObject", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void UnlockGameObject() => ToggleGameObjectLock(false);

        [MenuItem(MenuPath + "Locking/Integrations/Lock All Folders", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void LockAllFolders() => SetLockStateForAllFolders(true);

        [MenuItem(MenuPath + "Locking/Integrations/Unlock All Folders", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Locking)]
        public static void UnlockAllFolders() => SetLockStateForAllFolders(false);
        #endregion

        #region Renaming
        [MenuItem(MenuPath + "Renaming/Rename Selected GameObjects With Auto Index", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Renaming)]
        public static void RenameGameObjectsWithIndex() => RenameSelectedGameObjects("rename with automatic incrementation", true);

        [MenuItem(MenuPath + "Renaming/Rename Selected GameObjects With Same Name", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Renaming)]
        public static void RenameGameObjectsWithSameName() => RenameSelectedGameObjects("rename with the same name", false);
        #endregion

        #region Selecting
        #region 2D Object
        [MenuItem(MenuPath + "Selecting/2D Object/Select All Sprites", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSprites() => SelectAllOfComponentType<SpriteRenderer>();

        [MenuItem(MenuPath + "Selecting/2D Object/Select All 9-Sliced", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAll9Sliced() => SelectAll2DSpritesByType("9-Sliced");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Capsules", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCapsuleSprites() => SelectAll2DSpritesByType("Capsule");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Circles", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCircleSprites() => SelectAll2DSpritesByType("Circle");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Hexagon Flat-Tops", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllHexagonFlatTopSprites() => SelectAll2DSpritesByType("Hexagon Flat-Top");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Hexagon Pointed-Tops", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllHexagonPointedTopSprites() => SelectAll2DSpritesByType("Hexagon Pointed-Top");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Isometric Diamonds", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllIsometricDiamondSprites() => SelectAll2DSpritesByType("Isometric Diamond");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Squares", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSquareSprites() => SelectAll2DSpritesByType("Square");

        [MenuItem(MenuPath + "Selecting/2D Object/Select All Triangles", false, priorityBase + priorityDivisor)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTriangleSprites() => SelectAll2DSpritesByType("Triangle");
        #endregion

        #region 3D Object
        [MenuItem(MenuPath + "Selecting/3D Object/Select All Mesh Renderers", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllMeshRenderers() => SelectAllOfComponentType<MeshRenderer>();

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Cubes", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCubes() => SelectAll3DObjects("Cube");

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Spheres", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSpheres() => SelectAll3DObjects("Sphere");

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Capsules", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCapsules() => SelectAll3DObjects("Capsule");

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Cylinders", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCylinders() => SelectAll3DObjects("Cylinder");

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Planes", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllPlanes() => SelectAll3DObjects("Plane");

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Quads", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllQuads() => SelectAll3DObjects("Quad");

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Terrains", false, priorityBase + priorityDivisorTwo + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTerrains() => SelectAllOfComponentType<Terrain>();

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Wind Zones", false, priorityBase + priorityDivisorTwo + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTerrainsWindZones() => SelectAllOfComponentType<WindZone>();

        [MenuItem(MenuPath + "Selecting/3D Object/Select All Skinned Mesh Renderers", false, priorityBase + priorityDivisorTwo + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSkinnedMeshRenderers() => SelectAllOfComponentType<SkinnedMeshRenderer>();
        #endregion

        #region Audio
        [MenuItem(MenuPath + "Selecting/Audio/Select All Audio Sources", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllAudioSources() => SelectAllOfComponentType<AudioSource>();

        [MenuItem(MenuPath + "Selecting/Audio/Select All Audio Reverb Zones", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllAudioReverbZones() => SelectAllOfComponentType<AudioReverbZone>();
        #endregion

        #region Effects
        [MenuItem(MenuPath + "Selecting/Effects/Select All Particle Systems", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllParticleSystems() => SelectAllOfComponentType<ParticleSystem>();

        [MenuItem(MenuPath + "Selecting/Effects/Select All Particle System Force Fields", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllParticleSystemForceFields() => SelectAllOfComponentType<ParticleSystemForceField>();

        [MenuItem(MenuPath + "Selecting/Effects/Select All Lines", false, priorityBase + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllLines() => SelectAllOfComponentType<LineRenderer>();

        [MenuItem(MenuPath + "Selecting/Effects/Select All Trails", false, priorityBase + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTrails() => SelectAllOfComponentType<TrailRenderer>();
        #endregion
        
        #region Light
        [MenuItem(MenuPath + "Selecting/Light/Select All Lights", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllLights() => SelectAllOfComponentType<Light>();

        [MenuItem(MenuPath + "Selecting/Light/Select All Directional Lights", false, priorityBase + priorityDivisor + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllDirectionalLights() => SelectAllOfComponentType<Light>(light => light.type == LightType.Directional);

        [MenuItem(MenuPath + "Selecting/Light/Select All Point Lights", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllPointLights() => SelectAllOfComponentType<Light>(light => light.type == LightType.Point);

        [MenuItem(MenuPath + "Selecting/Light/Select All Spot Lights", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSpotLights() => SelectAllOfComponentType<Light>(light => light.type == LightType.Spot);

        [MenuItem(MenuPath + "Selecting/Light/Select All Area Lights", false, priorityBase + priorityDivisor + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllAreaLights() => SelectAllOfComponentType<Light>(light => light.type == LightType.Rectangle);

        [MenuItem(MenuPath + "Selecting/Light/Select All Reflection Probes", false, priorityBase + priorityDivisorTwo + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllReflectionProbes() => SelectAllOfComponentType<ReflectionProbe>();

        [MenuItem(MenuPath + "Selecting/Light/Select All Light Probe Groups", false, priorityBase + priorityDivisorTwo + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllLightProbeGroups() => SelectAllOfComponentType<LightProbeGroup>();
        #endregion

        #region Video
        [MenuItem(MenuPath + "Selecting/Video/Select All Video Players", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllVideoPlayers() => SelectAllOfComponentType<UnityEngine.Video.VideoPlayer>();
        #endregion
        
        #region UI
        [MenuItem(MenuPath + "Selecting/UI/Select All Images", false, priorityBase + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllImages() => SelectAllOfComponentType<Image>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Raw Images", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllRawImages() => SelectAllOfComponentType<RawImage>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Toggles", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllToggles() => SelectAllOfComponentType<Toggle>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Sliders", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSliders() => SelectAllOfComponentType<Slider>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Scrollbars", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllScrollbars() => SelectAllOfComponentType<Scrollbar>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Scroll Views", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllScrollViews() => SelectAllOfComponentType<ScrollRect>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Buttons", false, priorityBase + priorityDivisor + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllButtons() => SelectAllOfComponentType<Button>();

        #region TMP
        private static bool? _isTMPAvailable;
        private static bool IsTMPAvailable()
        {
            if (!_isTMPAvailable.HasValue)
            {
                _isTMPAvailable = AssetDatabase.FindAssets("t:TMP_Settings").Length > 0;
            }
            return _isTMPAvailable.Value;
        }
        private static void SelectTMPComponentIfAvailable<T>() where T : Component
        {
            if (IsTMPAvailable())
            {
                SelectAllOfComponentType<T>();
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }

        [MenuItem(MenuPath + "Selecting/UI/Select All Texts - TextMeshPro", false, priorityBase + priorityDivisorTwo + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTextMeshProTexts() => SelectTMPComponentIfAvailable<TMPro.TMP_Text>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Dropdowns - TextMeshPro", false, priorityBase + priorityDivisorTwo + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTextMeshProDropdowns() => SelectTMPComponentIfAvailable<TMPro.TMP_Dropdown>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Input Fields - TextMeshPro", false, priorityBase + priorityDivisorTwo + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllTextMeshProInputFields() => SelectTMPComponentIfAvailable<TMPro.TMP_InputField>();
        #endregion

        [MenuItem(MenuPath + "Selecting/UI/Select All Canvases", false, priorityBase + priorityDivisorThree + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCanvases() => SelectAllOfComponentType<Canvas>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Event Systems", false, priorityBase + priorityDivisorThree + 2)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllEventSystems() => SelectAllOfComponentType<UnityEngine.EventSystems.EventSystem>();
        #endregion

        #region Camera
        [MenuItem(MenuPath + "Selecting/Camera/Select All Cameras", false, priorityBase + 3)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllCameras() => SelectAllOfComponentType<Camera>();
        #endregion

        #region Others
        [MenuItem(MenuPath + "Selecting/Others/Select All MonoBehaviours", false, priorityBase + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllMonoBehaviours() => SelectAllOfComponentType<MonoBehaviour>();
        #endregion

        #region Integrations
        [MenuItem(MenuPath + "Selecting/Integrations/Select All Folders", false, priorityBase + priorityDivisor + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllFolders() => SelectAllOfComponentType<HierarchyDesignerFolder>();

        [MenuItem(MenuPath + "Selecting/Integrations/Select All Separators", false, priorityBase + priorityDivisor + 4)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Selecting)]
        public static void SelectAllSeparators() => SelectAllSeparatorsInScene();


        #endregion
        #endregion

        #region Sorting
        [MenuItem(MenuPath + "Sorting/Sort Alphabetically Ascending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortAlphabeticallyAscending() => SortSelectedGameObjectChildren(AlphanumericComparison, "sort its children alphabetically ascending");

        [MenuItem(MenuPath + "Sorting/Sort Alphabetically Descending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortAlphabeticallyDescending() => SortSelectedGameObjectChildren((a, b) => -AlphanumericComparison(a, b), "sort its children alphabetically descending");

        [MenuItem(MenuPath + "Sorting/Sort By Components Amount Ascending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByComponentsAmountAscending() => SortSelectedGameObjectChildren((a, b) => a.GetComponents<Component>().Length.CompareTo(b.GetComponents<Component>().Length), "sort its children by components amount ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Components Amount Descending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByComponentsAmountDescending() => SortSelectedGameObjectChildren((a, b) => b.GetComponents<Component>().Length.CompareTo(a.GetComponents<Component>().Length), "sort its children by components amount descending");

        [MenuItem(MenuPath + "Sorting/Sort By Layer Alphabetically Ascending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByLayerAlphabeticallyAscending() => SortSelectedGameObjectChildrenByLayer(true, "sort its children by layer alphabetically ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Layer Alphabetically Descending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByLayerAlphabeticallyDescending() => SortSelectedGameObjectChildrenByLayer(false, "sort its children by layer alphabetically descending");

        [MenuItem(MenuPath + "Sorting/Sort By Layer List Order Ascending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByLayerListOrderAscending() => SortSelectedGameObjectChildrenByLayerListOrder(true, "sort its children by layer list order ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Layer List Order Descending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByLayerListOrderDescending() => SortSelectedGameObjectChildrenByLayerListOrder(false, "sort its children by layer list order descending");

        [MenuItem(MenuPath + "Sorting/Sort By Length Ascending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByLengthAscending() => SortSelectedGameObjectChildren((a, b) => a.name.Length.CompareTo(b.name.Length), "sort its children by length ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Length Descending", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByLengthDescending() => SortSelectedGameObjectChildren((a, b) => b.name.Length.CompareTo(a.name.Length), "sort its children by length descending");

        [MenuItem(MenuPath + "Sorting/Sort Randomly", false, priorityBase)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortRandomly() => SortSelectedGameObjectChildrenRandomly("sort its children randomly");

        [MenuItem(MenuPath + "Sorting/Sort By Tag Alphabetically Ascending", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByTagAscending() => SortSelectedGameObjectChildrenByTag(true, "sort its children by tag ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Tag Alphabetically Descending", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByTagDescending() => SortSelectedGameObjectChildrenByTag(false, "sort its children by tag descending");

        [MenuItem(MenuPath + "Sorting/Sort By Tag List Order Ascending", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByTagListOrderAscending() => SortSelectedGameObjectChildrenByTagListOrder(true, "sort its children by tag list order ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Tag List Order Descending", false, priorityBase + 1)]
        [HierarchyDesigner_Tool(HierarchyDesigner_ToolCategory.Sorting)]
        public static void SortByTagListOrderDescending() => SortSelectedGameObjectChildrenByTagListOrder(false, "sort its children by tag list order descending");
        #endregion

        private static void CountAllOfComponentType<T>(string componentName) where T : Component
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    count++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total {componentName} in the scene: {count}.\n{componentName} Found: {namesString}.\n");
        }

        private static void CountAllOfComponentType<T>(string componentName, Func<T, bool> predicate = null) where T : Component
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                T component = gameObject.GetComponent<T>();
                if (component != null && (predicate == null || predicate(component)))
                {
                    count++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total {componentName} in the scene: {count}.\n{componentName} Found: {namesString}.\n");
        }

        private static void CountAllOfComponentsType<T>(string componentName) where T : Component
        {
            var allGameObjects = GetAllGameObjectsInScene();
            int totalComponentCount = 0;
            List<string> detailedNames = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                T[] components = gameObject.GetComponents<T>();
                if (components.Length > 0)
                {
                    List<string> componentNames = new List<string>();
                    foreach (T component in components)
                    {
                        if (component != null)
                        {
                            componentNames.Add(component.GetType().Name);
                        }
                        else
                        {
                            componentNames.Add("Invalid MonoBehaviour");
                        }
                    }
                    if (componentNames.Count > 0)
                    {
                        totalComponentCount += components.Length;
                        string componentDetails = $"{gameObject.name} ({string.Join(", ", componentNames)})";
                        detailedNames.Add(componentDetails);
                    }
                }
            }

            string namesString = detailedNames.Count > 0 ? string.Join(", ", detailedNames) : "none";
            Debug.Log($"Total {componentName} in the scene: {totalComponentCount}.\n{componentName} Found: {namesString}.\n");
        }

        private static IEnumerable<GameObject> GetAllGameObjectsInScene()
        {
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            List<GameObject> allGameObjects = new List<GameObject>();
            Stack<GameObject> stack = new Stack<GameObject>(rootObjects);

            while (stack.Count > 0)
            {
                GameObject current = stack.Pop();
                allGameObjects.Add(current);
                foreach (Transform child in current.transform)
                {
                    stack.Push(child.gameObject);
                }
            }
            return allGameObjects;
        }

        private static void CountAll2DSpritesByType(string spriteType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType))
                {
                    count++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total 2D {spriteType} sprites in the scene: {count}.\n{spriteType} Sprites Found: {namesString}.\n");
        }

        private static void CountAll3DObjects(string primitiveType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains(primitiveType))
                {
                    count++;
                    names.Add(gameObject.name);
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total 3D {primitiveType} objects in the scene: {count}.\n{primitiveType} Objects Found: {namesString}.\n");
        }

        private static void CountAllSeparatorsInScene()
        {
            var allGameObjects = GetAllSceneGameObjects();
            int separatorCount = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (IsSeparator(gameObject))
                {
                    separatorCount++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total separators in the scene: {separatorCount}.\nSeparators Found: {namesString}.\n");
        }

        private static void SetLockStateForAllFolders(bool lockState)
        {
            foreach (GameObject gameObject in GetAllSceneGameObjects())
            {
                if (HierarchyDesigner_Visual_Folder.IsFolder(gameObject))
                {
                    SetGameObjectLockState(gameObject, lockState);
                    SetEditableState(gameObject, !lockState);
                }
            }
            HierarchyDesigner_Visual_Tools.Cleanup();
        }

        public static void SetGameObjectLockState(GameObject gameObject, bool isLocked)
        {
            if (HierarchyDesigner_Utility_Separator.IsSeparator(gameObject)) return;
            SetEditableState(gameObject, !isLocked);
        }

        public static void SetEditableState(GameObject gameObject, bool editable)
        {
            Undo.RegisterCompleteObjectUndo(gameObject, $"{(editable ? "Unlock" : "Lock")} GameObject");

            HideFlags newFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            if (gameObject.hideFlags != newFlags)
            {
                gameObject.hideFlags = newFlags;
                EditorUtility.SetDirty(gameObject);
            }

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component && component.hideFlags != newFlags)
                {
                    component.hideFlags = newFlags;
                    EditorUtility.SetDirty(component);
                }
            }

            if (editable)
            {
                SceneVisibilityManager.instance.EnablePicking(gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.DisablePicking(gameObject, true);
            }

            EditorWindow[] allEditorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (EditorWindow inspector in allEditorWindows)
            {
                if (inspector.GetType().Name == "InspectorWindow")
                {
                    inspector.Repaint();
                }
            }
        }

        private static void SetLockStateForAllGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllSceneGameObjects())
            {
                if (!HierarchyDesigner_Utility_Separator.IsSeparator(gameObject))
                {
                    SetGameObjectLockState(gameObject, lockState);
                    SetEditableState(gameObject, !lockState);
                }
            }
            HierarchyDesigner_Visual_Tools.Cleanup();
        }

        private static void SetLockStateForAllGameObjectParents(bool lockState)
        {
            foreach (GameObject gameObject in GetAllSceneGameObjects())
            {
                if (IsGameObjectParent(gameObject))
                {
                    SetGameObjectLockState(gameObject, lockState);
                    SetEditableState(gameObject, !lockState);
                }
            }
            HierarchyDesigner_Visual_Tools.Cleanup();
        }

        private static void ToggleGameObjectLock(bool lockState)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null){ Debug.Log($"No GameObject selected. Please select a GameObject to {(lockState ? "lock" : "unlock")}."); return; }
            if (HierarchyDesigner_Utility_Separator.IsSeparator(selectedGameObject)){ Debug.Log("Selected GameObject is a separator and cannot be locked/unlocked."); return; }

            if (IsGameObjectLocked(selectedGameObject) != lockState)
            {
                SetGameObjectLockState(selectedGameObject, lockState);
                HierarchyDesigner_Visual_Tools.Cleanup();
            }
            else
            {
                Debug.Log(selectedGameObject + (lockState ? " is already locked" : " is already unlocked"));
            }
        }

        public static bool IsGameObjectLocked(GameObject gameObject)
        {
            if (gameObject == null) { return false; }
            return (gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
        }

        private static IEnumerable<GameObject> GetAllSceneGameObjects()
        {
            GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            List<GameObject> allGameObjects = new List<GameObject>();

            Stack<GameObject> stack = new Stack<GameObject>(rootObjects);
            while (stack.Count > 0)
            {
                GameObject current = stack.Pop();
                allGameObjects.Add(current);

                foreach (Transform child in current.transform)
                {
                    stack.Push(child.gameObject);
                }
            }
            return allGameObjects;
        }

        private static bool IsGameObjectParent(GameObject gameObject)
        {
            return gameObject.transform.childCount > 0;
        }

        public static void RenameSelectedGameObjects(string sortingActionDescription, bool autoIndex)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length == 0){ Debug.Log($"No GameObject selected. Please select one or more GameObjects to {sortingActionDescription}."); return; }

            HierarchyDesigner_Window_Renaming.OpenWindow(selectedGameObjects, new Vector2(0, 0), autoIndex);
        }

        private static void SelectAllOfComponentType<T>() where T : Component
        {
            var allGameObjects = GetAllSceneGameObjects();
            List<GameObject> gameObjectsWithComponent = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    gameObjectsWithComponent.Add(gameObject);
                }
            }

            if (gameObjectsWithComponent.Count > 0)
            {
                FocusHierarchyWindow();
                Selection.objects = gameObjectsWithComponent.ToArray();
            }
            else
            {
                Debug.Log($"No GameObjects with {typeof(T).Name} found in the current scene.");
            }
        }

        private static void SelectAllOfComponentType<T>(Func<T, bool> predicate = null) where T : Component
        {
            var allGameObjects = GetAllSceneGameObjects();
            List<GameObject> gameObjectsWithComponent = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                T component = gameObject.GetComponent<T>();
                if (component != null && (predicate == null || predicate(component)))
                {
                    gameObjectsWithComponent.Add(gameObject);
                }
            }

            if (gameObjectsWithComponent.Count > 0)
            {
                FocusHierarchyWindow();
                Selection.objects = gameObjectsWithComponent.ToArray();
            }
            else
            {
                Debug.Log($"No GameObjects with {typeof(T).Name} found in the current scene.");
            }
        }

        private static void SelectAll2DSpritesByType(string spriteType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            List<GameObject> selectedSprites = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType))
                {
                    selectedSprites.Add(gameObject);
                }
            }

            SelectOrDisplayMessage(selectedSprites, spriteType, "2D");
        }

        private static void SelectAll3DObjects(string primitiveType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            List<GameObject> gameObjectsOfPrimitiveType = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter != null && meshFilter.sharedMesh != null && meshFilter.sharedMesh.name.Contains(primitiveType))
                {
                    gameObjectsOfPrimitiveType.Add(gameObject);
                }
            }

            SelectOrDisplayMessage(gameObjectsOfPrimitiveType, primitiveType, "3D");
        }

        private static void SelectOrDisplayMessage(List<GameObject> gameObjects, string type, string dimension)
        {
            if (gameObjects.Count > 0)
            {
                FocusHierarchyWindow();
                Selection.objects = gameObjects.ToArray();
            }
            else
            {
                Debug.Log($"No {dimension} {type} objects found in the current scene.");
            }
        }

        private static void FocusHierarchyWindow()
        {
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
        }

        private static void SelectAllSeparatorsInScene()
        {
            var allGameObjects = GetAllSceneGameObjects();
            List<GameObject> separatorObjects = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (IsSeparator(gameObject))
                {
                    separatorObjects.Add(gameObject);
                }
            }

            if (separatorObjects.Count > 0)
            {
                FocusHierarchyWindow();
                Selection.objects = separatorObjects.ToArray();
            }
            else
            {
                Debug.Log("No Separators found in the current scene.");
            }
        }

        private static bool IsSeparator(GameObject gameObject)
        {
            return gameObject != null && gameObject.tag == "EditorOnly" && gameObject.name.StartsWith("//");
        }

        private static void SortSelectedGameObjectChildren(Comparison<GameObject> comparison, string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null) { Debug.Log($"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}."); return; }

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in selectedGameObject.transform)
            {
                children.Add(child.gameObject);
            }

            children.Sort(comparison);
            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }

        private static int AlphanumericComparison(GameObject x, GameObject y)
        {
            string xName = x.name;
            string yName = y.name;

            string[] xParts = System.Text.RegularExpressions.Regex.Split(xName.Replace(" ", ""), "([0-9]+)");
            string[] yParts = System.Text.RegularExpressions.Regex.Split(yName.Replace(" ", ""), "([0-9]+)");

            for (int i = 0; i < Math.Min(xParts.Length, yParts.Length); i++)
            {
                if (int.TryParse(xParts[i], out int xPartNum) && int.TryParse(yParts[i], out int yPartNum))
                {
                    if (xPartNum != yPartNum) return xPartNum.CompareTo(yPartNum);
                }
                else
                {
                    int compareResult = xParts[i].CompareTo(yParts[i]);
                    if (compareResult != 0) return compareResult;
                }
            }
            return xParts.Length.CompareTo(yParts.Length);
        }

        private static void SortSelectedGameObjectChildrenByLayer(bool ascending, string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null) { Debug.Log($"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}."); return; }

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in selectedGameObject.transform)
            {
                children.Add(child.gameObject);
            }

            children.Sort((x, y) =>
            {
                string layerX = LayerMask.LayerToName(x.layer);
                string layerY = LayerMask.LayerToName(y.layer);
                return ascending ? string.Compare(layerX, layerY, StringComparison.Ordinal) : string.Compare(layerY, layerX, StringComparison.Ordinal);
            });

            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }

        private static void SortSelectedGameObjectChildrenByLayerListOrder(bool ascending, string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null) { Debug.Log($"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}."); return; }

            List<string> allLayers = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName))
                {
                    allLayers.Add(layerName);
                }
            }

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in selectedGameObject.transform)
            {
                children.Add(child.gameObject);
            }

            children.Sort((x, y) =>
            {
                int indexX = allLayers.IndexOf(LayerMask.LayerToName(x.layer));
                int indexY = allLayers.IndexOf(LayerMask.LayerToName(y.layer));
                return ascending ? indexX.CompareTo(indexY) : indexY.CompareTo(indexX);
            });

            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }

        private static void SortSelectedGameObjectChildrenRandomly(string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null) { Debug.Log($"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}."); return; }

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in selectedGameObject.transform)
            {
                children.Add(child.gameObject);
            }

            System.Random rng = new System.Random();
            int n = children.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                GameObject value = children[k];
                children[k] = children[n];
                children[n] = value;
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }

        private static void SortSelectedGameObjectChildrenByTag(bool ascending, string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null) { Debug.Log($"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}."); return; }

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in selectedGameObject.transform)
            {
                children.Add(child.gameObject);
            }

            if (ascending)
            {
                children.Sort((x, y) => string.Compare(x.tag, y.tag, StringComparison.Ordinal));
            }
            else
            {
                children.Sort((x, y) => string.Compare(y.tag, x.tag, StringComparison.Ordinal));
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }

        private static void SortSelectedGameObjectChildrenByTagListOrder(bool ascending, string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject == null) { Debug.Log($"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}."); return; }

            List<string> allTags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
            string[] predefinedOrder = new string[] { "Untagged", "Respawn", "Finish", "EditorOnly", "MainCamera", "Player", "GameController" };
            allTags.Sort((x, y) =>
            {
                int indexX = Array.IndexOf(predefinedOrder, x);
                int indexY = Array.IndexOf(predefinedOrder, y);
                indexX = indexX == -1 ? int.MaxValue : indexX;
                indexY = indexY == -1 ? int.MaxValue : indexY;
                return indexX.CompareTo(indexY);
            });

            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in selectedGameObject.transform)
            {
                children.Add(child.gameObject);
            }

            children.Sort((x, y) =>
            {
                int indexX = allTags.IndexOf(x.tag);
                int indexY = allTags.IndexOf(y.tag);
                return ascending ? indexX.CompareTo(indexY) : indexY.CompareTo(indexX);
            });

            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }
    }
}
#endif