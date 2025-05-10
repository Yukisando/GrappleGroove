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
    [SerializeField] Transform spawnPoint;
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
        bool validCheckpointLoaded = checkpointManager.TryLoadCheckpoint(out var checkpointPosition, out var checkpointRotation);

        if (validCheckpointLoaded) {
            // Valid checkpoint found, use it
            spawnPoint.position = checkpointPosition;
            spawnPoint.rotation = checkpointRotation;
            SafeTeleportToCheckpoint(spawnPoint.position, spawnPoint.rotation);
            Debug.Log($"Loaded checkpoint at {checkpointPosition}!");
        }
        else {
            // No valid checkpoint found, use the existing spawnPoint
            Debug.Log("No valid checkpoint found. Using default spawn point.");

            // The spawnPoint is already set to its default position in the scene
            SafeTeleportToCheckpoint(spawnPoint.position, spawnPoint.rotation);

            // Update the lastCheckpointTransform to match the default spawn point
            checkpointManager.lastCheckpointTransform.position = spawnPoint.position;
            checkpointManager.lastCheckpointTransform.rotation = spawnPoint.rotation;
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

        SafeTeleportToCheckpoint(spawnPoint.position, spawnPoint.rotation);

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
        checkpointManager.SaveCheckpoint(_spawnPoint);
        spawnPoint.position = _spawnPoint.position;
        spawnPoint.rotation = _spawnPoint.rotation;
        infoPopup.ShowPopup("Checkpoint reached!");
    }

    void OnPlayerEnteredKillVolume() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PopupMessage(string _message) {
        infoPopup.ShowPopup(_message);
    }
}