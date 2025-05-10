#region

using System.IO;
using UnityEditor;
using UnityEngine;

#endregion

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
}