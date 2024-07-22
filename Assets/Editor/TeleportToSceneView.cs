#region

using UnityEditor;
using UnityEngine;

#endregion

[InitializeOnLoad]
public class TeleportPlayerOnPlay
{
    static GameObject player;
    static bool isTeleportEnabled;

    static TeleportPlayerOnPlay() {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorApplication.delayCall += () => {
            isTeleportEnabled = EditorPrefs.GetBool("TeleportPlayerOnPlay_Enabled", false);
        };
    }

    [MenuItem("Tools/Toggle Teleport Player On Play")]
    static void ToggleTeleportOnPlay() {
        isTeleportEnabled = !isTeleportEnabled;
        EditorPrefs.SetBool("TeleportPlayerOnPlay_Enabled", isTeleportEnabled);
        Debug.Log(isTeleportEnabled ? "Teleport on Play enabled" : "Teleport on Play disabled");
    }

    [MenuItem("Tools/Toggle Teleport Player On Play", true)]
    static bool ToggleTeleportOnPlayValidate() {
        Menu.SetChecked("Tools/Toggle Teleport Player On Play", isTeleportEnabled);
        return true;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode && isTeleportEnabled)
            TeleportPlayer();
    }

    static void TeleportPlayer() {
        player = GameObject.Find("Player");
        if (player == null) {
            Debug.LogError("Player GameObject not found. Make sure it's named 'Player' in the scene.");
            return;
        }

        var sceneView = SceneView.lastActiveSceneView;
        if (sceneView != null) {
            var cameraPosition = sceneView.camera.transform.position;

            // Move the player to the scene view camera's position
            player.transform.position = cameraPosition;

            Debug.Log("Player teleported to SceneView camera position.");
        }
        else
            Debug.LogWarning("No active SceneView found. Make sure you have a SceneView open.");
    }
}