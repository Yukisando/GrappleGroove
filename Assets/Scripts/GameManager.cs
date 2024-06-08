#region

using PrototypeFPC;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] Transform respawnPoint;
    [SerializeField] Transform playerTransform;
    [SerializeField] ScratchManager scratchManager;
    [SerializeField] Dependencies playerDependencies;

    [Header("Audio")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip checkpointSound;
    [SerializeField] AudioClip nodeSound;

    void Awake() {
        var checkpointVolumes = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var resetVolumes = FindObjectsByType<ResetVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var nodeVolumes = FindObjectsByType<NodePickupVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var killVolumes = FindObjectsByType<KillVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var checkpointVolume in checkpointVolumes) {
            checkpointVolume.onEnterVolume += OnPlayerEnteredCheckpointVolume;
        }

        foreach (var resetVolume in resetVolumes) {
            resetVolume.onEnterVolume += OnPlayerEnteredResetVolume;
        }

        foreach (var nodeVolume in nodeVolumes) {
            nodeVolume.onEnterVolume += OnPlayerEnteredNodePickupVolume;
        }

        foreach (var killVolume in killVolumes) {
            killVolume.onEnterVolume += OnPlayerEnteredKillVolume;
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) Application.Quit();

        if (Input.GetKeyDown(KeyCode.F5)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnPlayerEnteredResetVolume() {
        playerDependencies.GetComponent<GrapplingHook>().ResetHook();
        playerDependencies.audioSourceTop.PlayOneShot(deathSound);
        playerTransform.SetPositionAndRotation(respawnPoint.position, quaternion.identity);
        playerDependencies.rb.linearVelocity = Vector3.zero;
        playerDependencies.rb.angularVelocity = Vector3.zero;
        Debug.Log("Player got reset!");
    }

    void OnPlayerEnteredCheckpointVolume(Transform _t) {
        respawnPoint.position = _t.transform.position;
        playerDependencies.audioSourceTop.PlayOneShot(checkpointSound);
        Debug.Log("Checkpoint reached!");
    }

    void OnPlayerEnteredNodePickupVolume(NodeData _nodeData) {
        scratchManager.AddNode(_nodeData);
        playerDependencies.audioSourceTop.PlayOneShot(nodeSound);
        Debug.Log("Node collected!");
    }

    void OnPlayerEnteredKillVolume() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Player died!");
    }
}