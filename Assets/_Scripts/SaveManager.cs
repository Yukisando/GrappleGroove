#region

using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class SaveManager : MonoBehaviour
{
    const string CheckpointFileFormat = "checkpoint_data_{0}.json";
    public Transform lastCheckpointTransform;

    // Folder name to store checkpoints
    const string SaveFolderName = "Checkpoints";

    public void SaveCheckpoint(Transform checkpointTransform, string checkpointId) {
        var data = CheckpointData.FromTransform(checkpointTransform, SceneManager.GetActiveScene().name, checkpointId);
        string json = JsonUtility.ToJson(data);

        // Create directory if it doesn't exist
        string directory = Path.Combine(Application.persistentDataPath, SaveFolderName);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        File.WriteAllText(Path.Combine(directory, fileName), json);

        Debug.Log($"Checkpoint saved for scene: {SceneManager.GetActiveScene().name} with ID: {checkpointId}");
    }

// Overload the old SaveCheckpoint method for compatibility if needed
    public void SaveCheckpoint(Transform checkpointTransform) {
        var checkpointVolume = checkpointTransform.GetComponent<CheckpointVolume>();
        if (checkpointVolume != null)
            SaveCheckpoint(checkpointTransform, checkpointVolume.checkpointId);
        else
            Debug.LogError("CheckpointVolume component not found on the transform to save!");
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
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Cleared player preferences");
        string directory = Path.Combine(Application.persistentDataPath, SaveFolderName);
        if (Directory.Exists(directory)) {
            Directory.Delete(directory, true);
            Debug.Log("Save data deleted");
        }
    }

    public bool TryLoadCheckpoint(out Vector3 position, out Quaternion rotation, out string checkpointId) {
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        string path = Path.Combine(Application.persistentDataPath, SaveFolderName, fileName);
        checkpointId = null; // Initialize the out parameter

        if (File.Exists(path))
            try {
                string json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<CheckpointData>(json);

                if (data != null && data.sceneName == SceneManager.GetActiveScene().name) {
                    position = data.ToPosition();
                    rotation = data.ToRotation();
                    lastCheckpointTransform.position = position;
                    lastCheckpointTransform.rotation = rotation;
                    checkpointId = data.checkpointId;
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

// Overload the old TryLoadCheckpoint method for compatibility
    public bool TryLoadCheckpoint(out Vector3 position, out Quaternion rotation) {
        return TryLoadCheckpoint(out position, out rotation, out string _);
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
    public string checkpointId; // Unique ID of the checkpoint

    public static CheckpointData FromTransform(Transform transform, string scene, string id) {
        return new CheckpointData {
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z,
            rotX = transform.eulerAngles.x,
            rotY = transform.eulerAngles.y,
            rotZ = transform.eulerAngles.z,
            sceneName = scene,
            checkpointId = id,
        };
    }

    public Vector3 ToPosition() {
        return new Vector3(x, y, z);
    }

    public Quaternion ToRotation() {
        return Quaternion.Euler(rotX, rotY, rotZ);
    }
}