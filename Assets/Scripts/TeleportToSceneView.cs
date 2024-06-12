#if UNITY_EDITOR

#region

using System.Collections;
using UnityEditor;
using UnityEngine;

#endregion

public class TeleportToSceneView : MonoBehaviour
{
    IEnumerator Start() {
        yield return null;
        Teleport();
    }

    [ContextMenu("Teleport To Scene View")]
    void Teleport() {
        if (SceneView.lastActiveSceneView == null || SceneView.lastActiveSceneView.camera == null) {
            Debug.LogWarning("No active scene view or camera is available.");
            return;
        }

        var sceneCameraTransform = SceneView.lastActiveSceneView.camera.transform;
        Debug.Log($"Teleporting to scene view at {sceneCameraTransform.position}.");
        GetComponent<Rigidbody>().MovePosition(sceneCameraTransform.position);
    }
}
#endif