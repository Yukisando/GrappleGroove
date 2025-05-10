#region

using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

#endregion

public class CheckpointManager : MonoBehaviour
{
    const string CheckpointFileFormat = "checkpoint_data_{0}.json";
    [FormerlySerializedAs("lastCheckpointPosition")]
    public Transform lastCheckpointTransform;

    // Folder name to store checkpoints
    const string SaveFolderName = "Checkpoints";

    public void SaveCheckpoint(Transform checkpointTransform) {
        var data = CheckpointData.FromTransform(checkpointTransform, SceneManager.GetActiveScene().name);
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
    public bool TryLoadCheckpoint(out Vector3 position, out Quaternion rotation) {
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        string path = Path.Combine(Application.persistentDataPath, SaveFolderName, fileName);

        if (File.Exists(path))
            try {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<CheckpointData>(json);

                if (data != null && data.sceneName == SceneManager.GetActiveScene().name) {
                    position = data.ToPosition();
                    rotation = data.ToRotation();
                    lastCheckpointTransform.position = position;
                    lastCheckpointTransform.rotation = rotation;
                    return true;
                }
            }
            catch (Exception e) {
                Debug.LogWarning($"Error loading checkpoint: {e.Message}");
            }

        // No valid checkpoint found
        position = Vector3.zero;
        rotation = Quaternion.identity;
        return false;
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
    public float rotX, rotY, rotZ; // Rotation of the checkpoint (Euler angles)
    public string sceneName; // Name of the scene
    public string timestamp; // When the checkpoint was saved

    public static CheckpointData FromTransform(Transform transform, string scene) {
        return new CheckpointData {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z,
            rotX = transform.eulerAngles.x,
            rotY = transform.eulerAngles.y,
            rotZ = transform.eulerAngles.z,
            sceneName = scene,
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        };
    }

    public Vector3 ToPosition() {
        return new Vector3(x, y, z);
    }

    public Quaternion ToRotation() {
        return Quaternion.Euler(rotX, rotY, rotZ);
    }
}