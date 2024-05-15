#if UNITY_EDITOR
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_EditorState
    {
        #region Properties
        public static bool IsPlaying { get; private set; }
        #endregion

        static HierarchyDesigner_Shared_EditorState()
        {
            UpdatePlayingState();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            UpdatePlayingState();
        }

        private static void UpdatePlayingState()
        {
            IsPlaying = EditorApplication.isPlaying;
        }
    }
}
#endif