#region

using PrototypeFPC;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    [Header("Game dependencies")]
    [SerializeField] PlayerDependencies playerDependencies;
    [SerializeField] CheckpointData checkpointData;
    [SerializeField] Transform respawnPoint;
    [SerializeField] ScratchManager scratchManager;
    [SerializeField] PickupPopup pickupPopup;

    [Header("Settings")]
    [SerializeField] KeyCode respawnKey = KeyCode.Q;
    [SerializeField] KeyCode restartKey = KeyCode.F5;
    [SerializeField] KeyCode quitKey = KeyCode.Escape;

    [Header("Audio")]
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip checkpointSound;
    [SerializeField] AudioClip nodeSound;

    CheckpointVolume[] checkpointVolumes;
    EmancipationVolume[] emancipationVolumes;
    KillVolume[] killVolumes;
    NodePickupVolume[] nodeVolumes;
    ResetVolume[] resetVolumes;

    void Awake() {
        checkpointVolumes = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        resetVolumes = FindObjectsByType<ResetVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        nodeVolumes = FindObjectsByType<NodePickupVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        killVolumes = FindObjectsByType<KillVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        emancipationVolumes = FindObjectsByType<EmancipationVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var checkpointVolume in checkpointVolumes) {
            checkpointVolume.onEnterVolume += OnPlayerEnteredCheckpointVolume;
        }

        foreach (var resetVolume in resetVolumes) {
            resetVolume.onEnterVolume += ResetPlayer;
        }

        foreach (var nodeVolume in nodeVolumes) {
            nodeVolume.onEnterVolume += OnPlayerEnteredNodePickupVolume;
        }

        foreach (var killVolume in killVolumes) {
            killVolume.onEnterVolume += OnPlayerEnteredKillVolume;
        }

        foreach (var emancipationVolume in emancipationVolumes) {
            emancipationVolume.onEnterVolume += OnPlayerEnterEmancipationVolume;
        }

        checkpointData.ValidateCheckpoints();
    }

    void Start() {
        LoadLastCheckpoint();
    }

    void Update() {
        if (Input.GetKeyDown(quitKey)) Application.Quit();
        if (Input.GetKeyDown(respawnKey)) ResetPlayer();
        if (Input.GetKeyDown(restartKey)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadLastCheckpoint() {
        if (!PlayerPrefs.HasKey("lastCheckpoint")) return;

        foreach (var c in checkpointVolumes) {
            if (PlayerPrefs.GetString("lastCheckpoint") != c.idd) continue;
            Debug.Log($"Loading last checkpoint: {c.idd}");
            OnPlayerEnteredCheckpointVolume(c.transform);
            break;
        }
    }

    void OnPlayerEnterEmancipationVolume(RopeType _ropeType) {
        playerDependencies.GetComponent<GrapplingHook>().DestroyRopes(_ropeType);
    }

    void ResetPlayer() {
        playerDependencies.GetComponent<GrapplingHook>().DestroyRopes();
        playerDependencies.audioSourceTop.PlayOneShot(deathSound);
        playerDependencies.rb.linearVelocity = Vector3.zero;
        playerDependencies.rb.angularVelocity = Vector3.zero;
        playerDependencies.playerTransform.SetPositionAndRotation(respawnPoint.position, quaternion.identity);
        Debug.Log("Player got reset!");
    }

    void OnPlayerEnteredCheckpointVolume(Transform _t) {
        respawnPoint.position = _t.transform.position;
        playerDependencies.audioSourceTop.PlayOneShot(checkpointSound);
        _t.gameObject.SetActive(false);
        Debug.Log("Checkpoint reached!");
    }

    void OnPlayerEnteredNodePickupVolume(NodeData _nodeData) {
        scratchManager.AddNode(_nodeData);
        pickupPopup.ShowPopup($"Node collected: {_nodeData.id}");
        playerDependencies.audioSourceTop.PlayOneShot(nodeSound);
        Debug.Log("Node collected!");
    }

    void OnPlayerEnteredKillVolume() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Player died!");
    }
}