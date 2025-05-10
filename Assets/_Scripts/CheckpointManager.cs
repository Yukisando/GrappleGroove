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

    public void SaveCheckpoint(Vector3 position) {
        var data = CheckpointData.FromVector(position, SceneManager.GetActiveScene().name);
        string json = JsonUtility.ToJson(data);
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.SetString("Checkpoint_" + SceneManager.GetActiveScene().name, json);
        PlayerPrefs.Save();
        #else
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName), json);
        #endif
        
        Debug.Log($"Checkpoint saved for scene: {SceneManager.GetActiveScene().name}");
    }

    public void DeleteSaveFile() {
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path)) {
            File.Delete(path);
            Debug.Log($"Checkpoint deleted for scene: {SceneManager.GetActiveScene().name}");
        }
    }

    public Vector3 LoadLastCheckpoint() {
        #if UNITY_WEBGL && !UNITY_EDITOR
        string key = "Checkpoint_" + SceneManager.GetActiveScene().name;
        if (PlayerPrefs.HasKey(key)) {
            string json = PlayerPrefs.GetString(key);
            var data = JsonUtility.FromJson<CheckpointData>(json);
            if (data.sceneName == SceneManager.GetActiveScene().name) {
                lastCheckpointTransform.position = data.ToVector();
                return data.ToVector();
            }
        }
        #else
        string fileName = string.Format(CheckpointFileFormat, SceneManager.GetActiveScene().name);
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<CheckpointData>(json);
            if (data.sceneName == SceneManager.GetActiveScene().name) {
                lastCheckpointTransform.position = data.ToVector();
                return data.ToVector();
            }
        }
        #endif
        
        return lastCheckpointTransform.position;
    }
}

[Serializable]
public class CheckpointData
{
    public float x, y, z; // Position of the checkpoint
    public string sceneName; // Name of the scene

    public static CheckpointData FromVector(Vector3 position, string scene) {
        return new CheckpointData {
            x = position.x,
            y = position.y,
            z = position.z,
            sceneName = scene,
        };
    }

    public Vector3 ToVector() {
        return new Vector3(x, y, z);
    }
}