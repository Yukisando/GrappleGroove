#region

using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

#endregion

public class CheckpointManager : MonoBehaviour
{
    const string CheckpointFileFormat = "checkpoint_data_{0}.json";
    [FormerlySerializedAs("lastCheckpointPosition")]
    public Transform lastCheckpointTransform;

    // Folder name to store checkpoints
    const string SaveFolderName = "Checkpoints";

    public void SaveCheckpoint(Vector3 position) {
        var data = CheckpointData.FromVector(position, SceneManager.GetActiveScene().name);
        string json = JsonUtility.ToJson(data);

        // Create directory if it doesn't exist
        string directory = Path.Combine(Application.persistentDataPath, SaveFolderName);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        File.WriteAllText(Path.Combine(directory, fileName), json);

        Debug.Log($"Checkpoint saved for scene: {SceneManager.GetActiveScene().name}");
    }

    public void DeleteSaveFile() {
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        string path = Path.Combine(Application.persistentDataPath, SaveFolderName, fileName);
        if (File.Exists(path)) {
            File.Delete(path);
            Debug.Log($"Checkpoint deleted for scene: {SceneManager.GetActiveScene().name}");
        }
    }

    public void DeleteAllSaveFiles() {
        string directory = Path.Combine(Application.persistentDataPath, SaveFolderName);
        if (Directory.Exists(directory)) {
            Directory.Delete(directory, true);
            Debug.Log("All checkpoint saves deleted");
        }
    }

    // Returns true if a valid checkpoint was loaded, false otherwise
    public bool TryLoadCheckpoint(out Vector3 position) {
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        string path = Path.Combine(Application.persistentDataPath, SaveFolderName, fileName);

        if (File.Exists(path))
            try {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<CheckpointData>(json);

                if (data != null && data.sceneName == SceneManager.GetActiveScene().name) {
                    position = data.ToVector();
                    lastCheckpointTransform.position = position;
                    return true;
                }
            }
            catch (Exception e) {
                Debug.LogWarning($"Error loading checkpoint: {e.Message}");
            }

        // No valid checkpoint found
        position = Vector3.zero;
        return false;
    }

    // For backward compatibility
    public Vector3 LoadLastCheckpoint() {
        Vector3 position;
        if (TryLoadCheckpoint(out position)) return position;
        return lastCheckpointTransform.position;
    }

    // Get the save directory path
    public static string GetSaveDirectory() {
        return Path.Combine(Application.persistentDataPath, SaveFolderName);
    }
}

[Serializable]
public class CheckpointData
{
    public float x, y, z; // Position of the checkpoint
    public string sceneName; // Name of the scene
    public string timestamp; // When the checkpoint was saved

    public static CheckpointData FromVector(Vector3 position, string scene) {
        return new CheckpointData {
            x = position.x,
            y = position.y,
            z = position.z,
            sceneName = scene,
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        };
    }

    public Vector3 ToVector() {
        return new Vector3(x, y, z);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CheckpointManager))]
public class CheckpointManagerEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        var manager = (CheckpointManager)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Editor Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Delete Current Scene Checkpoint")) manager.DeleteSaveFile();

        if (GUILayout.Button("Delete All Checkpoints"))
            if (EditorUtility.DisplayDialog("Delete All Checkpoints",
                    "Are you sure you want to delete all checkpoint saves?",
                    "Yes", "Cancel"))
                manager.DeleteAllSaveFiles();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Save Location:", EditorStyles.miniLabel);
        EditorGUILayout.SelectableLabel(CheckpointManager.GetSaveDirectory());
    }

    [MenuItem("Tools/Game/Delete All Checkpoint Saves", false, 100)]
    static void DeleteAllCheckpointSaves() {
        if (EditorUtility.DisplayDialog("Delete All Checkpoints",
                "Are you sure you want to delete all checkpoint saves?",
                "Yes", "Cancel")) {
            string directory = Path.Combine(Application.persistentDataPath, "Checkpoints");
            if (Directory.Exists(directory)) {
                Directory.Delete(directory, true);
                Debug.Log("All checkpoint saves deleted");
            }
            else
                Debug.Log("No checkpoint saves found");
        }
    }
#endif
}