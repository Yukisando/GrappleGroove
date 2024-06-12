#region

using UnityEditor;
using UnityEngine;

#endregion

public class TeleportToSceneView : MonoBehaviour
{
#if UNITY_EDITOR
    void Start() {
        if (SceneView.lastActiveSceneView == null) return;

        var sceneCameraTransform = SceneView.lastActiveSceneView.camera.transform;
        transform.position = sceneCameraTransform.position;
        transform.rotation = sceneCameraTransform.rotation;
    }
#endif
}