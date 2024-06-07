#region

using System;
using PrototypeFPC;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    [SerializeField] Transform playerTransform;
    [SerializeField] ScratchManager scratchManager;
    [SerializeField] Dependencies playerDependencies;
    
    void Awake() {
        var checkpointVolumes = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var resetVolumes = FindObjectsByType<ResetVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var nodeVolumes = FindObjectsByType<NodePickupVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (var checkpointVolume in checkpointVolumes) {
            checkpointVolume.onEnterVolume +=  OnPlayerEnteredCheckpointVolume;
        }
        
        foreach (var resetVolume in resetVolumes) {
            resetVolume.onEnterVolume += OnPlayerEnteredKillVolume;
        }
        
        foreach (var nodeVolume in nodeVolumes) {
            nodeVolume.onEnterVolume += OnPlayerEnteredNodePickupVolume;
        }
    }
    
    void OnPlayerEnteredKillVolume() {
        playerTransform.position = respawnPoint.position;
        playerDependencies.GetComponent<GrapplingHook>().ResetHook();
        Debug.Log("Player died!");
    }
    
    void OnPlayerEnteredCheckpointVolume(Transform t) {
        respawnPoint.position = t.transform.position;
        Debug.Log("Checkpoint reached!");
    }
    
    void OnPlayerEnteredNodePickupVolume(NodeData nodeData) {
        scratchManager.AddNode(nodeData);
        Debug.Log("Node collected!");
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }
        
        if (Input.GetKeyDown(KeyCode.F5)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}