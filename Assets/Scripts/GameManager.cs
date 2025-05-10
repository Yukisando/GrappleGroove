#region

using PrototypeFPC;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Game dependencies")]
    [SerializeField] bool autoStartHost = true;
    [SerializeField] CheckpointManager checkpointManager;
    [SerializeField] Transform respawnPoint;
    [SerializeField] InfoPopup infoPopup;
    [SerializeField] GameObject endUI;
    [SerializeField] GameObject playerUI;

    [Header("Settings")]
    [SerializeField] KeyCode respawnKey = KeyCode.Q;
    [SerializeField] KeyCode restartKey = KeyCode.F5;
    [SerializeField] KeyCode quitKey = KeyCode.Escape;
    [SerializeField] KeyCode clearSaveKey = KeyCode.F6;

    [Header("Audio")]
    public AudioClip resetSound;
    public AudioClip checkpointSound;
    public AudioClip platformSound;

    CheckpointVolume[] checkpointVolumes;
    EmancipationVolume[] emancipationVolumes;
    Grabbable[] grabbableObjects;
    KillVolume[] killVolumes;
    Move[] movingObjects;
    ResetVolume[] resetVolumes;

    PlayerDependencies playerDependencies;

    [Header("Performance")]
    [SerializeField] int targetFrameRate = 120;
    [SerializeField] bool limitFrameRateForWebGL = true;

    void Awake() {
        if (I == null) I = this;
        else Destroy(gameObject);
        Application.targetFrameRate = targetFrameRate;
    }

    void Start() {
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();
        InitializeWorldObjects();
        LoadLastCheckpoint();
    }

    void Update() {
        CheckInputs();
    }

    void CheckInputs() {
        if (Input.GetKeyDown(quitKey)) {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if (Input.GetKeyDown(clearSaveKey)) checkpointManager.DeleteSaveFile();
        if (Input.GetKeyDown(respawnKey)) ResetGameState(false);
        if (Input.GetKeyDown(restartKey)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void InitializeWorldObjects() {
        checkpointVolumes = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        resetVolumes = FindObjectsByType<ResetVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        killVolumes = FindObjectsByType<KillVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        emancipationVolumes = FindObjectsByType<EmancipationVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        movingObjects = FindObjectsByType<Move>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        grabbableObjects = FindObjectsByType<Grabbable>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var checkpointVolume in checkpointVolumes) {
            checkpointVolume.onEnterVolume += OnPlayerEnteredCheckpointVolume;
        }

        foreach (var resetVolume in resetVolumes) {
            resetVolume.onEnterVolume += ResetGameState;
        }

        foreach (var killVolume in killVolumes) {
            killVolume.onEnterVolume += OnPlayerEnteredKillVolume;
        }

        foreach (var emancipationVolume in emancipationVolumes) {
            emancipationVolume.onEnterVolume += OnPlayerEnterEmancipationVolume;
        }
    }

    void LoadLastCheckpoint() {
        var checkpointPosition = checkpointManager.LoadLastCheckpoint();
        if (checkpointPosition != Vector3.zero) {
            respawnPoint.position = checkpointPosition;
            SafeTeleportToCheckpoint(respawnPoint.position, respawnPoint.rotation);
            Debug.Log($"Loaded checkpoint at {checkpointPosition}!");
        }
        else {
            Debug.Log("No checkpoint found.");
            respawnPoint.position = playerDependencies.transform.position;
        }
    }

    void SafeTeleportToCheckpoint(Vector3 position, Quaternion rotation) {
        playerDependencies.grapplingHook.DestroyRopes();
        playerDependencies.rb.linearVelocity = Vector3.zero;
        playerDependencies.rb.angularVelocity = Vector3.zero;
        playerDependencies.rb.MovePosition(position);
        playerDependencies.perspective.ForceOrientation(rotation);
    }

    void ResetGameState(bool _playSound = true) {
        if (_playSound)
            playerDependencies.audioSourceTop.PlayOneShot(resetSound);

        SafeTeleportToCheckpoint(respawnPoint.position, respawnPoint.rotation);

        foreach (var movingObject in movingObjects) {
            movingObject.ResetObject();
        }

        foreach (var grabbableObject in grabbableObjects) {
            grabbableObject.ResetObject();
        }
    }

    void OnPlayerEnterEmancipationVolume(RopeType _ropeType) {
        playerDependencies.grapplingHook.DestroyRopes(_ropeType);
    }

    public void LoadLevel(string levelName) {
        if (string.IsNullOrEmpty(levelName)) return;
        SceneManager.LoadScene(levelName);
    }

    void OnPlayerEnteredCheckpointVolume(Transform _spawnPoint) {
        playerDependencies.audioSourceTop.PlayOneShot(checkpointSound);
        checkpointManager.SaveCheckpoint(_spawnPoint.position);
        respawnPoint.position = _spawnPoint.position;
        respawnPoint.forward = _spawnPoint.forward;
        infoPopup.ShowPopup("Checkpoint reached!");
    }

    void OnPlayerEnteredKillVolume() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        infoPopup.ShowPopup("Crap!");
    }

    public void PopupMessage(string _message) {
        infoPopup.ShowPopup(_message);
    }
}