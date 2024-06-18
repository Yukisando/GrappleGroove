#region

using UnityEditor;
using UnityEngine;

#endregion

public class TeleportPlayerOnPlay : MonoBehaviour
{
    // static bool teleportOnPlay;
    //
    // [MenuItem("Tools/Teleport Player On Play")]
    // static void ToggleTeleportOnPlay() {
    //     teleportOnPlay = !teleportOnPlay;
    //     Menu.SetChecked("Tools/Teleport Player On Play", teleportOnPlay);
    //     Debug.Log("Teleport On Play is " + (teleportOnPlay ? "enabled" : "disabled"));
    // }
    //
    // [InitializeOnLoadMethod]
    // static void OnLoad() {
    //     EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    // }
    //
    // static void OnPlayModeStateChanged(PlayModeStateChange state) {
    //     if (state == PlayModeStateChange.EnteredPlayMode && teleportOnPlay) {
    //         TeleportPlayer();
    //     }
    // }
    //
    // static void TeleportPlayer() {
    //     var player = GameObject.Find("Player");
    //     if (player == null) {
    //         Debug.LogWarning("No GameObject named 'Player' found in the scene.");
    //         return;
    //     }
    //     
    //     var sceneView = SceneView.lastActiveSceneView;
    //     if (sceneView == null) {
    //         Debug.LogWarning("No active SceneView found.");
    //         return;
    //     }
    //     
    //     var sceneViewCamera = sceneView.camera;
    //     if (sceneViewCamera == null) {
    //         Debug.LogWarning("No camera found in the active SceneView.");
    //         return;
    //     }
    //     
    //     var cameraPosition = sceneViewCamera.transform.position;
    //     var cameraRotation = sceneViewCamera.transform.rotation;
    //     
    //     // Log the camera's position and rotation
    //     Debug.Log("SceneView Camera Position: " + cameraPosition);
    //     Debug.Log("SceneView Camera Rotation: " + cameraRotation);
    //     
    //     player.transform.position = cameraPosition;
    //     player.transform.rotation = cameraRotation;
    //     
    //     // Confirm the player's new position and rotation
    //     Debug.Log("Player teleported to: " + player.transform.position);
    // }
}