#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public static class HierarchyDesigner_Visual_GameObject
    {
        #region Hierarchy Tree Icons
        private static readonly Texture2D branchIcon_I = HierarchyDesigner_Manager_Tree.BranchIcon_I;
        private static readonly Texture2D branchIcon_L = HierarchyDesigner_Manager_Tree.BranchIcon_L;
        private static readonly Texture2D branchIcon_T = HierarchyDesigner_Manager_Tree.BranchIcon_T;
        private static readonly Texture2D branchIcon_End = HierarchyDesigner_Manager_Tree.BranchIcon_End;
        #endregion
        #region TMP Availability Check
        private static bool? _isTMPAvailable;
        private static Type _tmpTextType;
        private static Type _tmpDropdownType;
        private static Type _tmpInputFieldType;
        private static bool CheckTMPAvailability()
        {
            if (!_isTMPAvailable.HasValue)
            {
                _isTMPAvailable = AssetDatabase.FindAssets("t:TMP_Settings").Length > 0;
                if (_isTMPAvailable.Value)
                {
                    _tmpTextType = Type.GetType("TMPro.TMP_Text, Unity.TextMeshPro");
                    _tmpDropdownType = Type.GetType("TMPro.TMP_Dropdown, Unity.TextMeshPro");
                    _tmpInputFieldType = Type.GetType("TMPro.TMP_InputField, Unity.TextMeshPro");
                }
            }
            return _isTMPAvailable.Value;
        }
        private static Type TMPTextType => _tmpTextType;
        private static Type TMPDropdownType => _tmpDropdownType;
        private static Type TMPInputFieldType => _tmpInputFieldType;
        #endregion
        #region Other Properties
        private const float componentIconsOffset = 20f;
        private const float tagLayerOffset = 3f;
        private const float buttonsOffset = 15f;
        private const float buttonsLockedOffset = 53f;
        #endregion
        #region Cached Component Types
        private static readonly Type ButtonType = typeof(Button);
        private static readonly Type ScrollbarType = typeof(Scrollbar);
        private static readonly Type TextType = typeof(Text);
        private static readonly Type RawImageType = typeof(RawImage);
        private static readonly Type ImageType = typeof(Image);
        private static readonly Type ScrollRect = typeof(ScrollRect);
        private static readonly Type Dropdown = typeof(Dropdown);
        private static readonly Type InputField = typeof(InputField);
        private static readonly Type MonoBehaviourType = typeof(MonoBehaviour);
        #endregion
        #region Cached Properties
        private static readonly string ScriptPropertyName = "m_Script";
        private static readonly string WarningIconTexture = "console.warnicon";
        private static readonly Texture2D backgroundImage = HierarchyDesigner_Manager_Background.BackgroundImage;
        #region Tag and Layer
        private static Rect tagLabelRect, layerLabelRect;
        private static GUIStyle _tagStyle;
        private static GUIStyle tagStyle
        {
            get
            {
                if (_tagStyle == null)
                {
                    _tagStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        fontStyle = HierarchyDesigner_Manager_Settings.TagFontStyle,
                        fontSize = HierarchyDesigner_Manager_Settings.TagFontSize,
                        normal = { textColor = HierarchyDesigner_Manager_Settings.TagTextColor }
                    };
                }
                else
                {
                    _tagStyle.fontStyle = HierarchyDesigner_Manager_Settings.TagFontStyle;
                    _tagStyle.fontSize = HierarchyDesigner_Manager_Settings.TagFontSize;
                    _tagStyle.normal.textColor = HierarchyDesigner_Manager_Settings.TagTextColor;
                }
                return _tagStyle;
            }
        }

        private static GUIStyle _layerStyle;
        private static GUIStyle layerStyle
        {
            get
            {
                if (_layerStyle == null)
                {
                    _layerStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        fontStyle = HierarchyDesigner_Manager_Settings.LayerFontStyle,
                        fontSize = HierarchyDesigner_Manager_Settings.LayerFontSize,
                        normal = { textColor = HierarchyDesigner_Manager_Settings.LayerTextColor }
                    };
                }
                else
                {
                    _layerStyle.fontStyle = HierarchyDesigner_Manager_Settings.LayerFontStyle;
                    _layerStyle.fontSize = HierarchyDesigner_Manager_Settings.LayerFontSize;
                    _layerStyle.normal.textColor = HierarchyDesigner_Manager_Settings.LayerTextColor;
                }
                return _layerStyle;
            }
        }
        #endregion
        private static Color treeColorCache;
        private static bool isTreeColorCacheInitialized = false;
        private static Color TreeColor
        {
            get
            {
                if (!isTreeColorCacheInitialized)
                {
                    treeColorCache = HierarchyDesigner_Manager_Settings.TreeColor;
                    isTreeColorCacheInitialized = true;
                }
                return treeColorCache;
            }
        }
        private static Dictionary<int, Texture2D> mainIconCache = new Dictionary<int, Texture2D>();
        private static Dictionary<WeakReference, Component[]> componentCache = new Dictionary<WeakReference, Component[]>();
        private static List<Component[]> componentArrayPool = new List<Component[]>();
        private static readonly Dictionary<int, (Texture2D icon, Color color)> branchIconCache = new Dictionary<int, (Texture2D icon, Color color)>();
        private static readonly Dictionary<(string, GUIStyle, int), Vector2> guiContentSizeCache = new Dictionary<(string, GUIStyle, int), Vector2>();
        private static Dictionary<string, bool> tagExclusionCache = new Dictionary<string, bool>();
        private static Dictionary<string, bool> layerExclusionCache = new Dictionary<string, bool>();
        private static HashSet<int> visibleGameObjects = new HashSet<int>();
        #endregion

        static HierarchyDesigner_Visual_GameObject()
        {
            CheckTMPAvailability();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && HierarchyDesigner_Shared_EditorState.IsPlaying) return;

            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null || HierarchyDesigner_Utility_Separator.IsSeparator(gameObject)) return;
            visibleGameObjects.Add(instanceID);

            bool isGameObjectLocked = HierarchyDesigner_Utility_Tools.IsGameObjectLocked(gameObject);
            if (!isGameObjectLocked) { DrawTagAndLayerInfo(gameObject, selectionRect); }
            if (Event.current.type != EventType.Repaint)
            {
                HandleShortcuts(gameObject, selectionRect);
                if (HierarchyDesigner_Manager_Settings.ShowHierarchyButtons) { ProcessButtonInteractions(gameObject, selectionRect); }
                return;
            }

            DrawMainGameObjectIcon(gameObject, selectionRect, instanceID);
            if (HierarchyDesigner_Manager_Settings.ShowHierarchyTree && gameObject.transform.parent != null) { DrawHierarchyTree(gameObject, selectionRect); }
            if (!isGameObjectLocked) { DrawComponentIcons(gameObject, selectionRect); }
            if (HierarchyDesigner_Manager_Settings.ShowHierarchyButtons) { DrawInteractableIcons(gameObject, selectionRect); }
        }

        #region Buttons
        private static void ProcessButtonInteractions(GameObject gameObject, Rect selectionRect)
        {
            Rect lockIconRect, activeToggleRect;
            GetButtonRects(gameObject, selectionRect, out lockIconRect, out activeToggleRect);

            bool isMouseDown = Event.current.type == EventType.MouseDown;
            if (isMouseDown && lockIconRect.Contains(Event.current.mousePosition))
            {
                ToggleLockState(gameObject, !HierarchyDesigner_Utility_Tools.IsGameObjectLocked(gameObject));
                Event.current.Use();
            }
            else if (isMouseDown && activeToggleRect.Contains(Event.current.mousePosition))
            {
                ToggleActiveState(gameObject, !gameObject.activeSelf);
                Event.current.Use();
            }
        }

        private static void GetButtonRects(GameObject gameObject, Rect selectionRect, out Rect lockIconRect, out Rect activeToggleRect)
        {
            HierarchyDesigner_Info_Buttons.ButtonsPositionType buttonsPosition = HierarchyDesigner_Manager_Settings.ButtonsPosition;

            float iconStartX = selectionRect.xMax;
            if (buttonsPosition == HierarchyDesigner_Info_Buttons.ButtonsPositionType.Docked) { iconStartX -= 35f; }
            else { iconStartX = CalculateEndOfPropertiesX(gameObject, selectionRect) + selectionRect.x + buttonsOffset; }

            lockIconRect = new Rect(iconStartX, selectionRect.y, 20, selectionRect.height);
            activeToggleRect = new Rect(iconStartX + 20, selectionRect.y, 30, selectionRect.height);
        }

        private static void DrawInteractableIcons(GameObject gameObject, Rect selectionRect)
        {
            HierarchyDesigner_Info_Buttons.ButtonsPositionType buttonsPosition = HierarchyDesigner_Manager_Settings.ButtonsPosition;

            float iconStartX = selectionRect.xMax;
            if (buttonsPosition == HierarchyDesigner_Info_Buttons.ButtonsPositionType.Docked) { iconStartX -= 35f; }
            else { iconStartX = CalculateEndOfPropertiesX(gameObject, selectionRect) + selectionRect.x + buttonsOffset; }

            Rect lockIconRect = new Rect(iconStartX, selectionRect.y, 20, selectionRect.height);
            Rect activeToggleRect = new Rect(iconStartX + 20, selectionRect.y, 30, selectionRect.height);

            DrawLockButton(gameObject, lockIconRect);
            DrawActiveToggleButton(gameObject, activeToggleRect);
        }

        private static float CalculateEndOfPropertiesX(GameObject gameObject, Rect selectionRect)
        {
            if ((gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable) 
            { 
                return GetInitialIconOffset(gameObject) + buttonsLockedOffset; 
            }
            float offsetX = GetInitialIconOffset(gameObject);

            if (HierarchyDesigner_Manager_Settings.ShowComponentIcons)
            {
                offsetX += CalculateComponentIconsWidth(gameObject);
            }
            if (HierarchyDesigner_Manager_Settings.ShowTag && !IsTagExcluded(gameObject.tag))
            {
                offsetX += GetCachedGUIContentSize(new GUIContent(gameObject.tag), tagStyle).x + 5;
            }
            if (HierarchyDesigner_Manager_Settings.ShowLayer && !IsLayerExcluded(LayerMask.LayerToName(gameObject.layer)))
            {
                offsetX += GetCachedGUIContentSize(new GUIContent(LayerMask.LayerToName(gameObject.layer)), layerStyle).x + 5;
            }
            return offsetX;
        }

        private static float GetInitialIconOffset(GameObject gameObject)
        {
            GUIStyle style = GUI.skin.label;
            Vector2 contentSize = style.CalcSize(new GUIContent(gameObject.name));
            return contentSize.x + 5;
        }

        private static float CalculateComponentIconsWidth(GameObject gameObject)
        {
            Component[] components = GetComponentsFromCacheOrObject(gameObject);
            float totalWidth = 0f;
            float componentOffset = 16f;

            foreach (Component component in components)
            {
                if (ShouldComponentBeVisible(component, gameObject))
                {
                    totalWidth += componentOffset;
                }
            }
            return totalWidth;
        }

        private static bool ShouldComponentBeVisible(Component component, GameObject gameObject)
        {
            Type componentType = component.GetType();
            if ((componentType == typeof(Transform) || componentType == typeof(RectTransform)) && !HierarchyDesigner_Manager_Settings.ShowTransformComponent) return false;
            if (HierarchyDesigner_Visual_Folder.IsFolder(gameObject) && !HierarchyDesigner_Manager_Settings.ShowComponentIconsForFolders) return false;
            return true;
        }

        private static void DrawLockButton(GameObject gameObject, Rect rect)
        {
            bool isLocked = HierarchyDesigner_Utility_Tools.IsGameObjectLocked(gameObject);
            Texture2D lockIcon = HierarchyDesigner_Manager_Buttons.LockIcon;
            if (GUI.Button(rect, lockIcon))
            {
                ToggleLockState(gameObject, !isLocked);
            }
        }

        private static void DrawActiveToggleButton(GameObject gameObject, Rect rect)
        {
            bool isActive = gameObject.activeSelf;
            if (GUI.Button(rect, new GUIContent(isActive ? "On" : "Off")))
            {
                ToggleActiveState(gameObject, !isActive);
            }
        }

        private static void ToggleLockState(GameObject gameObject, bool newState)
        {
            HierarchyDesigner_Utility_Tools.SetGameObjectLockState(gameObject, newState);
        }

        private static void ToggleActiveState(GameObject gameObject, bool newState)
        {
            Undo.RecordObject(gameObject, "Toggle Active State");
            gameObject.SetActive(newState);
            EditorUtility.SetDirty(gameObject);
        }
        #endregion

        private static void HandleShortcuts(GameObject gameObject, Rect selectionRect)
        {
            if (gameObject == null) return;
            HandleTagAndLayerShortcut(gameObject, selectionRect);
            HandleRenameShortcut();
        }

        private static void OnHierarchyChanged()
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && HierarchyDesigner_Shared_EditorState.IsPlaying) return;

            ValidateComponentCache();
            List<WeakReference> deadComponentReferences = new List<WeakReference>();
            foreach (WeakReference key in componentCache.Keys)
            {
                if (!key.IsAlive)
                {
                    deadComponentReferences.Add(key);
                }
            }
            foreach (WeakReference deadRef in deadComponentReferences)
            {
                if (componentCache.TryGetValue(deadRef, out Component[] components))
                {
                    ReturnComponentsToPool(components);
                }
                componentCache.Remove(deadRef);
            }

            List<int> keysToRemoveFromMainIconCache = new List<int>();
            foreach (var kvp in mainIconCache)
            {
                if (EditorUtility.InstanceIDToObject(kvp.Key) == null)
                {
                    keysToRemoveFromMainIconCache.Add(kvp.Key);
                }
            }
            foreach (int key in keysToRemoveFromMainIconCache)
            {
                mainIconCache.Remove(key);
            }

            branchIconCache.Clear();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ValidateComponentCache();
            visibleGameObjects.Clear();
            guiContentSizeCache.Clear();
            CleanupCache();
            CleanupComponentArrayPool();
        }

        private static void ValidateComponentCache()
        {
            List<WeakReference> deadReferences = new List<WeakReference>();
            foreach (WeakReference key in componentCache.Keys)
            {
                if (!key.IsAlive)
                {
                    deadReferences.Add(key);
                }
            }
            foreach (WeakReference deadRef in deadReferences)
            {
                componentCache.Remove(deadRef);
            }
        }

        private static void CleanupCache()
        {
            List<WeakReference> deadComponentReferences = new List<WeakReference>();
            foreach (WeakReference key in componentCache.Keys)
            {
                if (!key.IsAlive)
                {
                    deadComponentReferences.Add(key);
                }
            }
            foreach (WeakReference deadRef in deadComponentReferences)
            {
                componentCache.Remove(deadRef);
            }

            List<int> keysToRemove = new List<int>();
            foreach (int key in branchIconCache.Keys)
            {
                if (EditorUtility.InstanceIDToObject(key) == null)
                {
                    keysToRemove.Add(key);
                }
            }
            foreach (int key in keysToRemove)
            {
                branchIconCache.Remove(key);
            }
        }

        private static void DrawMainGameObjectIcon(GameObject gameObject, Rect selectionRect, int instanceID)
        {
            if (!HierarchyDesigner_Manager_Settings.ShowMainIconOfGameObject || ShouldSkipDrawing(gameObject) || HierarchyDesigner_Visual_Folder.IsFolder(gameObject)) return;

            bool isHovering = Event.current.type == EventType.Repaint && selectionRect.Contains(Event.current.mousePosition);
            GUI.color = HierarchyDesigner_Shared_ColorUtility.GetBackgroundColor(isHovering, instanceID);
            if (backgroundImage != null)
            {
                DrawBackgroundImage(selectionRect);
            }

            GUI.color = Color.white;
            Texture2D mainIcon = GetMainIconForGameObject(gameObject);
            if (mainIcon != null)
            {
                DrawMainIcon(mainIcon, selectionRect);
            }
        }

        private static void DrawBackgroundImage(Rect selectionRect)
        {
            float iconSize = selectionRect.height * 1.0f;
            float iconYPosition = selectionRect.y + (selectionRect.height - iconSize) / 2;
            Rect backgroundRect = new Rect(selectionRect.x, iconYPosition, iconSize, iconSize);
            GUI.DrawTexture(backgroundRect, backgroundImage);
        }

        private static Texture2D GetMainIconForGameObject(GameObject gameObject)
        {
            int instanceID = gameObject.GetInstanceID();
            if (!mainIconCache.TryGetValue(instanceID, out Texture2D mainIcon))
            {
                Component[] components = GetComponentsFromCacheOrObject(gameObject);
                UpdateMainIconCacheIfNeeded(gameObject, instanceID, components);
                mainIconCache.TryGetValue(instanceID, out mainIcon);
            }
            return mainIcon;
        }

        private static void DrawMainIcon(Texture2D icon, Rect selectionRect)
        {
            float iconSize = selectionRect.height * 1.0f;
            float iconYPosition = selectionRect.y + (selectionRect.height - iconSize) / 2;
            Rect iconRect = new Rect(selectionRect.x, iconYPosition, iconSize, iconSize);
            GUI.DrawTexture(iconRect, icon);
        }

        private static Component FindMainUIComponent(Component[] components)
        {
            if (components == null || components.Length == 0) return null;

            if (TMPTextType != null)
            {
                for (int i = 0; i < components.Length; i++)
                {
                    Type componentType = components[i]?.GetType();
                    if (componentType != null &&
                        (TMPTextType.IsAssignableFrom(componentType) ||
                         TMPDropdownType != null && TMPDropdownType.IsAssignableFrom(componentType) ||
                         TMPInputFieldType != null && TMPInputFieldType.IsAssignableFrom(componentType)))
                    {
                        return components[i];
                    }
                }
            }

            Component imageComponent = null;
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null) continue;
                Type componentType = components[i].GetType();
                if (componentType == ButtonType || componentType == ScrollbarType || componentType == TextType || componentType == RawImageType || componentType == ScrollRect || componentType == Dropdown || componentType == InputField)
                {
                    return components[i];
                }
                else if (componentType == ImageType)
                {
                    imageComponent = imageComponent ?? components[i];
                }
            }
            return imageComponent;
        }

        private static void DrawComponentIcons(GameObject gameObject, Rect selectionRect)
        {
            if (!visibleGameObjects.Contains(gameObject.GetInstanceID()) || !HierarchyDesigner_Manager_Settings.ShowComponentIcons || ShouldSkipDrawing(gameObject)) { return; }

            Component[] components = GetComponentsFromCacheOrObject(gameObject);
            if (components.Length == 0) return;

            List<Component> filteredComponents = FilterComponents(components, HierarchyDesigner_Manager_Settings.ShowTransformComponent);

            float iconSize = selectionRect.height * 0.8f;
            float iconOffset = GetInitialIconOffset(filteredComponents.ToArray(), selectionRect);

            foreach (Component component in filteredComponents)
            {
                if (component == null)
                {
                    Texture2D warningIcon = EditorGUIUtility.FindTexture(WarningIconTexture);
                    DrawIcon(selectionRect, iconSize, ref iconOffset, warningIcon);
                    continue;
                }

                Texture2D componentIcon = GetComponentIcon(component);
                if (componentIcon != null)
                {
                    string componentName = component.GetType().Name;
                    GUIContent iconContent = new GUIContent(componentIcon, componentName);
                    DrawIconWithTooltip(selectionRect, iconSize, ref iconOffset, iconContent);
                }
            }
        }

        private static void DrawIconWithTooltip(Rect selectionRect, float iconSize, ref float iconOffset, GUIContent content)
        {
            float iconYPosition = selectionRect.y + (selectionRect.height - iconSize) / 2;
            Rect iconRect = new Rect(selectionRect.x + iconOffset, iconYPosition, iconSize, iconSize);
            GUI.Box(iconRect, content, GUIStyle.none);
            iconOffset += iconSize + 2;
        }

        private static List<Component> FilterComponents(Component[] components, bool showTransformComponent)
        {
            List<Component> filteredComponents = new List<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is Transform && !showTransformComponent) continue;
                filteredComponents.Add(components[i]);
            }
            return filteredComponents;
        }

        private static Texture2D GetComponentIcon(Component component)
        {
            if (component == null) { return null; }
            if (IsScriptMissing(component)) { return EditorGUIUtility.FindTexture(WarningIconTexture); }
            return EditorGUIUtility.ObjectContent(component, component.GetType()).image as Texture2D;
        }

        private static void DrawIcon(Rect selectionRect, float iconSize, ref float iconOffset, Texture2D componentIcon)
        {
            float iconYPosition = selectionRect.y + (selectionRect.height - iconSize) / 2;
            Rect iconRect = new Rect(selectionRect.x + iconOffset, iconYPosition, iconSize, iconSize);
            GUI.DrawTexture(iconRect, componentIcon);
            iconOffset += iconSize + 2;
        }

        private static bool IsScriptMissing(Component component)
        {
            if (component.GetType() == MonoBehaviourType)
            {
                SerializedObject serializedObject = new SerializedObject(component);
                SerializedProperty scriptProperty = serializedObject.FindProperty(ScriptPropertyName);
                return scriptProperty == null || scriptProperty.objectReferenceValue == null;
            }
            return false;
        }

        private static Component[] GetComponentsFromCacheOrObject(GameObject gameObject)
        {
            if (gameObject == null) { return new Component[0]; }

            int instanceID = gameObject.GetInstanceID();
            WeakReference weakRef = new WeakReference(gameObject);

            if (!componentCache.TryGetValue(weakRef, out Component[] components) || !weakRef.IsAlive)
            {
                components = GetPooledComponents(gameObject);
                componentCache[weakRef] = components;
            }

            UpdateMainIconCacheIfNeeded(gameObject, instanceID, components);
            return components;
        }

        private static Component[] GetPooledComponents(GameObject gameObject)
        {
            Component[] allComponents = gameObject.GetComponents<Component>();
            int componentCount = allComponents.Length;

            Component[] componentsFromPool = null;
            for (int i = 0; i < componentArrayPool.Count; i++)
            {
                if (componentArrayPool[i].Length >= componentCount && ValidateComponentsArray(componentArrayPool[i], componentCount))
                {
                    componentsFromPool = componentArrayPool[i];
                    componentArrayPool.RemoveAt(i);
                    break;
                }
            }

            if (componentsFromPool == null)
            {
                componentsFromPool = new Component[componentCount];
            }
            else
            {
                for (int i = componentCount; i < componentsFromPool.Length; i++)
                {
                    componentsFromPool[i] = null;
                }
            }

            Array.Copy(allComponents, componentsFromPool, componentCount);
            return componentsFromPool;
        }

        private static bool ValidateComponentsArray(Component[] components, int expectedComponentCount)
        {
            if (components.Length < expectedComponentCount) { return false; }
            return Array.Exists(components, component => component != null);
        }

        private static void ReturnComponentsToPool(Component[] components)
        {
            if (Array.Exists(components, component => component != null)) { componentArrayPool.Add(components); }
        }

        private static void CleanupComponentArrayPool()
        {
            componentArrayPool.RemoveAll(componentsArray => !Array.Exists(componentsArray, component => component != null));
        }

        private static float GetInitialIconOffset(Component[] components, Rect selectionRect)
        {
            if (components == null || components.Length == 0 || components[0] == null || components[0].gameObject == null) { return 0f; }
            GUIStyle style = GUI.skin.label;
            GUIContent content = new GUIContent(components[0].gameObject.name);
            float textWidth = style.CalcSize(content).x;
            return textWidth + componentIconsOffset;
        }

        private static void UpdateMainIconCacheIfNeeded(GameObject gameObject, int instanceID, Component[] components)
        {
            Texture2D newMainIcon = GetIconForMainComponent(gameObject, components);
            if (!mainIconCache.TryGetValue(instanceID, out Texture2D currentMainIcon) || currentMainIcon != newMainIcon) { mainIconCache[instanceID] = newMainIcon; }
        }

        private static Texture2D GetIconForMainComponent(GameObject gameObject, Component[] components)
        {
            Component mainComponent = FindMainUIComponent(components);
            if (mainComponent == null) { mainComponent = (components != null && components.Length > 1) ? components[1] : gameObject.transform; }
            if (mainComponent != null) { return EditorGUIUtility.ObjectContent(mainComponent, mainComponent.GetType()).image as Texture2D; }
            return null;
        }

        #region Tag and Layer
        private static void DrawTagAndLayerInfo(GameObject gameObject, Rect selectionRect)
        {
            if (ShouldSkipDrawing(gameObject) || (!HierarchyDesigner_Manager_Settings.ShowTag && !HierarchyDesigner_Manager_Settings.ShowLayer)) return;

            float iconOffset = GetAdjustedIconOffset(gameObject, selectionRect.height * 0.65f, selectionRect) + tagLayerOffset;
            const float spaceBetween = 3f;

            if (HierarchyDesigner_Manager_Settings.ShowTag)
            {
                tagLabelRect = new Rect();
                if (!IsTagExcluded(gameObject.tag))
                {
                    GUIStyle tagCurrentStyle = tagStyle;
                    GUIContent tagContent = new GUIContent(gameObject.tag);
                    Vector2 tagSize = GetCachedGUIContentSize(tagContent, tagCurrentStyle);
                    float tagLabelWidth = tagSize.x;

                    tagLabelRect = new Rect(selectionRect.x + iconOffset, selectionRect.y, tagLabelWidth, selectionRect.height);
                    GUI.Label(tagLabelRect, gameObject.tag, tagCurrentStyle);
                    iconOffset += tagLabelWidth + spaceBetween;
                }
            }
            if (HierarchyDesigner_Manager_Settings.ShowLayer)
            {
                string layerName = LayerMask.LayerToName(gameObject.layer);
                layerLabelRect = new Rect();
                if (!IsLayerExcluded(layerName))
                {
                    GUIStyle layerCurrentStyle = layerStyle;
                    GUIContent layerContent = new GUIContent(layerName);
                    Vector2 layerSize = GetCachedGUIContentSize(layerContent, layerCurrentStyle);
                    float layerLabelWidth = layerSize.x;

                    layerLabelRect = new Rect(selectionRect.x + iconOffset, selectionRect.y, layerLabelWidth, selectionRect.height);
                    GUI.Label(layerLabelRect, layerName, layerCurrentStyle);
                }
            }
        }

        private static bool IsTagExcluded(string tag)
        {
            if (!tagExclusionCache.TryGetValue(tag, out bool isExcluded))
            {
                isExcluded = HierarchyDesigner_Manager_Settings.ExcludedTags.Contains(tag);
                tagExclusionCache[tag] = isExcluded;
            }
            return isExcluded;
        }

        private static bool IsLayerExcluded(string layerName)
        {
            if (!layerExclusionCache.TryGetValue(layerName, out bool isExcluded))
            {
                isExcluded = HierarchyDesigner_Manager_Settings.ExcludedLayers.Contains(layerName);
                layerExclusionCache[layerName] = isExcluded;
            }
            return isExcluded;
        }

        public static void ClearExcludeTagLayerCache()
        {
            tagExclusionCache.Clear();
            layerExclusionCache.Clear();
        }

        public static void RecalculateTagAndLayerSizes()
        {
            guiContentSizeCache.Clear();
            _tagStyle = null;
            _layerStyle = null;
        }
        #endregion

        private static Vector2 GetCachedGUIContentSize(GUIContent content, GUIStyle style)
        {
            var key = (content.text, style, style.fontSize);
            if (!guiContentSizeCache.TryGetValue(key, out Vector2 size))
            {
                size = style.CalcSize(content);
                guiContentSizeCache[key] = size;
            }
            return size;
        }

        private static void HandleTagAndLayerShortcut(GameObject gameObject, Rect selectionRect)
        {
            if (HierarchyDesigner_Manager_Settings.ShowTag || HierarchyDesigner_Manager_Settings.ShowLayer)
            {
                if (HierarchyDesigner_Utility_Shortcut.IsShortcutPressed(HierarchyDesigner_Manager_Settings.ChangeTagAndLayerShortcut))
                {
                    Vector2 mousePosition = Event.current.mousePosition;
                    if (HierarchyDesigner_Manager_Settings.ShowTag && tagLabelRect.Contains(mousePosition))
                    {
                        HierarchyDesigner_Window_TagLayer.OpenWindow(gameObject, true, Event.current.mousePosition);
                        Event.current.Use();
                        return;
                    }
                    if (HierarchyDesigner_Manager_Settings.ShowLayer && layerLabelRect.Contains(mousePosition))
                    {
                        HierarchyDesigner_Window_TagLayer.OpenWindow(gameObject, false, Event.current.mousePosition);
                        Event.current.Use();
                    }
                }
            }
        }

        private static void HandleRenameShortcut()
        {
            if (HierarchyDesigner_Utility_Shortcut.IsShortcutPressed(HierarchyDesigner_Manager_Settings.RenameGameObjectsShortcut))
            {
                GameObject[] selectedGameObjects = Selection.gameObjects;
                if (selectedGameObjects.Length > 0)
                {
                    HierarchyDesigner_Window_Renaming.OpenWindow(selectedGameObjects, Event.current.mousePosition);
                }
            }
        }

        private static float GetAdjustedIconOffset(GameObject gameObject, float iconSize, Rect selectionRect)
        {
            float offset = 0f;
            float spaceBetweenElements = 5f;

            float mainIconWidth = 8f;
            offset += mainIconWidth + spaceBetweenElements;

            GUIContent gameObjectNameContent = new GUIContent(gameObject.name);
            GUIStyle labelStyle = GUI.skin.label;
            float gameObjectNameWidth = labelStyle.CalcSize(gameObjectNameContent).x;
            offset += gameObjectNameWidth + spaceBetweenElements;

            if (HierarchyDesigner_Manager_Settings.ShowComponentIcons)
            {
                Component[] components = GetComponentsFromCacheOrObject(gameObject);

                int visibleComponentsCount = 0;
                foreach (Component component in components)
                {
                    if (HierarchyDesigner_Manager_Settings.ShowTransformComponent || !(component is Transform))
                    {
                        visibleComponentsCount++;
                    }
                }

                offset += visibleComponentsCount * (iconSize + spaceBetweenElements);
            }
            return offset;
        }

        private static void DrawHierarchyTree(GameObject gameObject, Rect selectionRect)
        {
            const float iconOffset = 14f;
            const float initialOffset = 22f;
            Transform transform = gameObject.transform;
            float startX = selectionRect.x - initialOffset;
            Texture2D branchIcon = GetOrCreateBranchIcon(transform);
            GUI.DrawTexture(new Rect(startX, selectionRect.y, selectionRect.height, selectionRect.height), branchIcon, ScaleMode.ScaleToFit, true, 0, TreeColor, 0, 0);
            DrawParentTreeLines(transform.parent, startX, iconOffset, selectionRect.height, selectionRect.y);
        }

        private static Texture2D GetOrCreateBranchIcon(Transform transform)
        {
            int instanceID = transform.GetInstanceID();
            Color currentTreeColor = GetCachedTreeColor();

            if (!branchIconCache.TryGetValue(instanceID, out var cached) || cached.color != currentTreeColor)
            {
                bool isLastChild = transform.GetSiblingIndex() == transform.parent.childCount - 1;
                Texture2D branchIcon = isLastChild ? (transform.childCount == 0 ? branchIcon_End : branchIcon_L) : branchIcon_T;
                branchIconCache[instanceID] = (branchIcon, currentTreeColor);
                return branchIcon;
            }
            return cached.icon;
        }

        private static void DrawParentTreeLines(Transform parentTransform, float startX, float iconOffset, float rectHeight, float yPos)
        {
            Color currentTreeColor = GetCachedTreeColor();
            while (parentTransform != null)
            {
                if (parentTransform.parent == null) break;

                startX -= iconOffset;
                Rect rect = new Rect(startX, yPos, rectHeight, rectHeight);

                if (parentTransform.GetSiblingIndex() != parentTransform.parent.childCount - 1)
                {
                    GUI.DrawTexture(rect, branchIcon_I, ScaleMode.ScaleToFit, true, 0, currentTreeColor, 0, 0);
                }

                parentTransform = parentTransform.parent;
            }
        }

        private static Color GetCachedTreeColor()
        {
            if (!isTreeColorCacheInitialized)
            {
                treeColorCache = HierarchyDesigner_Manager_Settings.TreeColor;
                isTreeColorCacheInitialized = true;
            }
            return treeColorCache;
        }

        public static void UpdateTreeColorCache()
        {
            isTreeColorCacheInitialized = false;
        }

        private static bool ShouldSkipDrawing(GameObject gameObject)
        {
            return HierarchyDesigner_Utility_Separator.IsSeparator(gameObject) || (HierarchyDesigner_Visual_Folder.IsFolder(gameObject) && !HierarchyDesigner_Manager_Settings.ShowComponentIconsForFolders);
        }
    }
}
#endif