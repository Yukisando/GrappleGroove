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
        var sceneView = SceneView.lastActiveSceneView ?? SceneView.currentDrawingSceneView;
        
        if (sceneView != null && sceneView.camera != null) {
            var sceneCameraTransform = sceneView.camera.transform;
            Debug.Log($"Teleporting to scene view at {sceneCameraTransform.position}.");
            GetComponent<Rigidbody>().MovePosition(sceneCameraTransform.position);
        }
        else {
            Debug.LogWarning("No active scene view or scene view camera found.");
        }
    }
}
#endif