#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public static class HierarchyDesigner_Utility_Shortcut
    {
        private static bool actionApplied = false;

        static HierarchyDesigner_Utility_Shortcut()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            if (HierarchyDesigner_Manager_Settings.DisableHierarchyDesignerDuringPlayMode && HierarchyDesigner_Shared_EditorState.IsPlaying) return;
            if (actionApplied) return;

            Event currentEvent = Event.current;
            if (currentEvent.type != EventType.KeyDown && currentEvent.type != EventType.MouseDown) { return; }

            GameObject hoveredObj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (hoveredObj == null) return;

            bool isHovered = selectionRect.Contains(currentEvent.mousePosition);
            GameObject[] selectedObjects = Selection.gameObjects;

            bool isShortcutPressed = IsShortcutPressed(HierarchyDesigner_Manager_Settings.EnableDisableShortcut) || IsShortcutPressed(HierarchyDesigner_Manager_Settings.LockUnlockShortcut);
            if (!isShortcutPressed) return;

            if (selectedObjects.Length >= 1)
            {
                foreach (GameObject obj in selectedObjects)
                {
                    ApplyShortcutAction(obj, IsShortcutPressed(HierarchyDesigner_Manager_Settings.EnableDisableShortcut), IsShortcutPressed(HierarchyDesigner_Manager_Settings.LockUnlockShortcut));
                }
                actionApplied = true;
                EditorApplication.delayCall += ResetActionAppliedFlag;
            }
            else if (isHovered)
            {
                ApplyShortcutAction(hoveredObj, IsShortcutPressed(HierarchyDesigner_Manager_Settings.EnableDisableShortcut), IsShortcutPressed(HierarchyDesigner_Manager_Settings.LockUnlockShortcut));
            }
        }

        private static void ApplyShortcutAction(GameObject obj, bool isEnableDisableShortcutPressed, bool isLockUnlockShortcutPressed)
        {
            if (isEnableDisableShortcutPressed)
            {
                ToggleActiveState(obj);
            }
            if (isLockUnlockShortcutPressed)
            {
                ToggleLockState(obj);
            }
        }

        private static void ToggleActiveState(GameObject obj)
        {
            Undo.RecordObject(obj, "Toggle Active State");
            obj.SetActive(!obj.activeSelf);
        }

        private static void ToggleLockState(GameObject obj)
        {
            bool isCurrentlyLocked = HierarchyDesigner_Utility_Tools.IsGameObjectLocked(obj);
            HierarchyDesigner_Utility_Tools.SetGameObjectLockState(obj, !isCurrentlyLocked);
        }

        private static void ResetActionAppliedFlag()
        {
            actionApplied = false;
            EditorApplication.delayCall -= ResetActionAppliedFlag;
        }

        public static bool IsShortcutPressed(KeyCode shortcutKey)
        {
            Event currentEvent = Event.current;

            if (shortcutKey >= KeyCode.Alpha0 && shortcutKey <= KeyCode.Menu)
            {
                return currentEvent.type == EventType.KeyDown && currentEvent.keyCode == shortcutKey;
            }

            int mouseButton = GetMouseButtonFromKeyCode(shortcutKey);
            if (mouseButton != -1)
            {
                return currentEvent.type == EventType.MouseDown && currentEvent.button == mouseButton;
            }
            return false;
        }

        private static int GetMouseButtonFromKeyCode(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Mouse0: return 0;
                case KeyCode.Mouse1: return 1;
                case KeyCode.Mouse2: return 2;
                case KeyCode.Mouse3: return 3;
                case KeyCode.Mouse4: return 4;
                case KeyCode.Mouse5: return 5;
                case KeyCode.Mouse6: return 6;
                default: return -1;
            }
        }
    }
}
#endif