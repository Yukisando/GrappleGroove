#region

using System;
using System.IO;
using UnityEngine;

#endregion

public class CheckpointManager : MonoBehaviour
{
    const string CheckpointFile = "checkpoint_data.json";
    
    public void SaveCheckpoint(Vector3 position) {
        var data = CheckpointData.FromVector(position);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/" + CheckpointFile, json);
        Debug.Log("Checkpoint saved");
    }
    
    public void DeleteSaveFile() {
        string path = Application.persistentDataPath + "/" + CheckpointFile;
        if (File.Exists(path)) {
            File.Delete(path);
            Debug.Log("Checkpoint deleted");
        }
    }
    
    public Vector3 LoadLastCheckpoint() {
        string path = Application.persistentDataPath + "/" + CheckpointFile;
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<CheckpointData>(json);
            return data.ToVector();
        }
        return Vector3.zero; // Return a default position if no checkpoint is found
    }
}

[Serializable]
public class CheckpointData
{
    public float x, y, z; // Position of the checkpoint
    
    public static CheckpointData FromVector(Vector3 position) {
        return new CheckpointData {
            x = position.x,
            y = position.y,
            z = position.z,
        };
    }
    
    public Vector3 ToVector() {
        return new Vector3(x, y, z);
    }
}