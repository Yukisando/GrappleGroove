using UnityEditor;
using UnityEngine.UI;

namespace Nobi.UiRoundedCorners.Editor {
    [CustomEditor(typeof(ImageWithRoundedCorners))]
    public class ImageWithRoundedCornersInspector : UnityEditor.Editor {
        ImageWithRoundedCorners script;

        void OnEnable() {
            script = (ImageWithRoundedCorners) target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (!script.TryGetComponent<Image>(out var _) && !script.TryGetComponent<RawImage>(out var _)) EditorGUILayout.HelpBox("This script requires an Image component on the same gameobject", MessageType.Warning);
        }
    }
}