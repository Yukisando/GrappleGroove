#region

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

[CreateAssetMenu(fileName = "New checkpoint data", menuName = "New checkpoint data", order = 0)]
public class CheckpointData : ScriptableObject
{
    [ReadOnly] public string lastCheckPointID = "";
    [SerializeField] [ReadOnly] List<string> generatedIDs = new List<string>();
    [SerializeField] List<CheckpointVolume> checkpoints;

    public void ValidateCheckpoints() {
        checkpoints = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        // Sort checkpoints based on their positions
        checkpoints = checkpoints.OrderBy(_cp => _cp.transform.position.sqrMagnitude).ToList();

        var checkpointToIDMap = new Dictionary<CheckpointVolume, string>();

        // Preserve existing IDs
        for (var i = 0; i < checkpoints.Count; i++) {
            if (i < generatedIDs.Count) checkpointToIDMap[checkpoints[i]] = generatedIDs[i];
        }

        generatedIDs.Clear();

        // Generate IDs for new checkpoints and preserve old ones
        foreach (var checkpoint in checkpoints) {
            if (!checkpointToIDMap.ContainsKey(checkpoint))
                checkpointToIDMap[checkpoint] = Random.Range(0, 1000000).ToString();
            generatedIDs.Add(checkpointToIDMap[checkpoint]);
        }

        // Assign IDs back to checkpoints
        for (var i = 0; i < checkpoints.Count; i++) {
            checkpoints[i].id = generatedIDs[i];
        }

        Debug.Log($"{checkpoints.Count} checkpoints validated!");
    }
}