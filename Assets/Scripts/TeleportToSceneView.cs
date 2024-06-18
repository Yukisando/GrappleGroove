#if UNITY_EDITOR

#region

using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#endregion

public class TeleportToSceneView : MonoBehaviour
{
    IEnumerator Start() {
        yield return null;
        Teleport();
    }
    
    [ContextMenu("Teleport To Scene View")] [Button(ButtonSizes.Large)]
    void Teleport() {
        var sceneView = SceneView.lastActiveSceneView ?? SceneView.currentDrawingSceneView;
        
        var sceneCameraTransform = sceneView.camera.transform;
        Debug.Log($"Teleporting to scene view at {sceneCameraTransform.position}.");
        GetComponent<Rigidbody>().MovePosition(sceneCameraTransform.position);
    }
}
#endif