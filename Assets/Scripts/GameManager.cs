#region

using PrototypeFPC;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    [Header("Game dependencies")]
    [SerializeField] PlayerDependencies playerDependencies;
    [SerializeField] Transform respawnPoint;
    [SerializeField] ScratchManager scratchManager;
    [SerializeField] InfoPopup infoPopup;

    [Header("Settings")]
    [SerializeField] bool loadLastCheckpointOnStart = true;
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
    }

    void Start() {
        if (loadLastCheckpointOnStart) LoadLastCheckpoint();
    }

    void Update() {
        if (Input.GetKeyDown(quitKey)) Application.Quit();
        if (Input.GetKeyDown(respawnKey)) ResetPlayer();
        if (Input.GetKeyDown(restartKey)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnApplicationQuit() {
        SavePosition(respawnPoint.position);
    }

    void LoadLastCheckpoint() {
        if (!loadLastCheckpointOnStart) return;
        var lastCheckpointPosition = new Vector3(PlayerPrefs.GetFloat("cp.x"), PlayerPrefs.GetFloat("cp.y"), PlayerPrefs.GetFloat("cp.z"));
        respawnPoint.position = lastCheckpointPosition;
        playerDependencies.rb.position = respawnPoint.position;
    }

    void OnPlayerEnterEmancipationVolume(RopeType _ropeType) {
        playerDependencies.GetComponent<GrapplingHook>().DestroyRopes(_ropeType);
    }

    void ResetPlayer() {
        playerDependencies.GetComponent<GrapplingHook>().DestroyRopes();
        playerDependencies.audioSourceTop.PlayOneShot(deathSound);
        playerDependencies.rb.linearVelocity = Vector3.zero;
        playerDependencies.rb.angularVelocity = Vector3.zero;
        playerDependencies.rb.position = respawnPoint.position;
        Debug.Log("Player got reset!");
    }

    void OnPlayerEnteredCheckpointVolume(CheckpointVolume _v) {
        SavePosition(_v.transform.position);

        playerDependencies.audioSourceTop.PlayOneShot(checkpointSound);
        _v.gameObject.SetActive(false);
        infoPopup.ShowPopup("Checkpoint reached!");
        Debug.Log("Checkpoint reached!");
    }

    void OnPlayerEnteredNodePickupVolume(NodeData _nodeData) {
        scratchManager.AddNode(_nodeData);
        infoPopup.ShowPopup($"Node collected: {_nodeData.id}");
        playerDependencies.audioSourceTop.PlayOneShot(nodeSound);
        Debug.Log("Node collected!");
    }

    void OnPlayerEnteredKillVolume() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Player died!");
    }

    void SavePosition(Vector3 _pos) {
        PlayerPrefs.SetFloat("cp.x", _pos.x);
        PlayerPrefs.SetFloat("cp.y", _pos.y);
        PlayerPrefs.SetFloat("cp.z", _pos.z);
        respawnPoint.position = _pos;
    }
}