#region

using System.Collections;
using PrototypeFPC;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Game dependencies")]
    PlayerDependencies playerDependencies;
    [SerializeField] CheckpointManager checkpointManager;
    [SerializeField] Transform spawnPoint;
    [SerializeField] InfoPopup infoPopup;
    [SerializeField] GameObject crosshairUI;
    [SerializeField] GameObject endUI;

    [Header("Settings")]
    [SerializeField] KeyCode respawnKey = KeyCode.Q;
    [SerializeField] KeyCode restartKey = KeyCode.F5;
    [SerializeField] KeyCode skipLevel = KeyCode.F9;
    [SerializeField] KeyCode quitKey = KeyCode.Escape;
    [SerializeField] KeyCode clearSaveKey = KeyCode.F6;

    [Header("Audio")]
    [SerializeField] AudioSource soundEffectSource;
    public AudioClip resetSound;
    public AudioClip checkpointSound;
    public AudioClip startTimerSound;
    public AudioClip stopTimerSound;
    public AudioClip timerTickSound;

    [Header("Performance")]
    [SerializeField] int targetFrameRate = 120;

    //Volumes
    CheckpointVolume[] checkpointVolumes;
    RopeEmancipationVolume[] emancipationVolumes;
    Grabbable[] grabbableObjects;
    KillVolume[] killVolumes;
    Move[] movingObjects;
    ResetVolume[] resetVolumes;

    //timer
    float elapsedTime;
    bool timerRunning;
    Coroutine timerCoroutine;

    void Awake() {
        if (I == null) I = this;
        else Destroy(gameObject);
        Application.targetFrameRate = targetFrameRate;
    }

    public void DestroyObjectByID(string id) {
        var allIDs = FindObjectsByType<ID>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var i in allIDs) {
            if (i.id.Equals(id)) i.Despawn();
        }
    }

    public void ResetObjectByID(string id) {
        var allIDs = FindObjectsByType<ID>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var i in allIDs) {
            if (i.id.Equals(id)) i.ResetObject();
        }
    }

    void Start() {
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();
        InitializeWorldObjects();

        // Check if teleport on play is enabled in editor preferences
        var skipCheckpointLoading = false;
#if UNITY_EDITOR
        skipCheckpointLoading = EditorPrefs.GetBool("TeleportPlayerOnPlay_Enabled", false);
#endif

        if (!skipCheckpointLoading)

            // Only load checkpoint if teleport on play is disabled
            // Delay the checkpoint loading slightly to ensure all components are initialized
            StartCoroutine(DelayedLoadCheckpoint());
    }

    IEnumerator DelayedLoadCheckpoint() {
        yield return null;
        LoadLastCheckpoint();
    }

    void Update() {
        CheckInputs();
        CheckSpeed();
    }

    public void StartTimer() {
        if (timerRunning) ResetTimer();
        timerRunning = true;
        timerCoroutine = StartCoroutine(UpdateTimer());
        soundEffectSource.PlayOneShot(startTimerSound);
    }

    public void StopTimer() {
        if (timerRunning) {
            timerRunning = false;
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            soundEffectSource.PlayOneShot(stopTimerSound);
        }
    }

    public void ResetTimer() {
        StopTimer();
        elapsedTime = 0f;
        PlayerOverlay.I.SetTimer(elapsedTime);
    }

    IEnumerator UpdateTimer() {
        float lastSecond = -1f;
        while (timerRunning) {
            elapsedTime += Time.deltaTime;
            PlayerOverlay.I.SetTimer(elapsedTime);

            // Check if we've crossed into a new second
            int currentSecond = Mathf.FloorToInt(elapsedTime);
            if (currentSecond > lastSecond) {
                soundEffectSource.PlayOneShot(timerTickSound);
                lastSecond = currentSecond;
            }

            yield return null;
        }
    }

    void CheckSpeed() {
        PlayerOverlay.I.SetSpeed(playerDependencies.rb.linearVelocity.magnitude);
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
        if (Input.GetKeyDown(skipLevel)) LoadNextScene();
        if (Input.GetKeyDown(restartKey)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadNextScene() {
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        if (i >= SceneManager.sceneCountInBuildSettings) i = 0;
        SceneManager.LoadScene(i);
    }

    void InitializeWorldObjects() {
        checkpointVolumes = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        resetVolumes = FindObjectsByType<ResetVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        killVolumes = FindObjectsByType<KillVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        emancipationVolumes = FindObjectsByType<RopeEmancipationVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        movingObjects = FindObjectsByType<Move>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        grabbableObjects = FindObjectsByType<Grabbable>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var checkpointVolume in checkpointVolumes) {
            checkpointVolume.onEnterVolume += OnPlayerEnteredCheckpointVolume;
        }

        foreach (var resetVolume in resetVolumes) {
            resetVolume.onPlayerEntered += ResetGameState;
        }

        foreach (var killVolume in killVolumes) {
            killVolume.onEnterVolume += OnPlayerEnteredKillVolume;
        }

        foreach (var emancipationVolume in emancipationVolumes) {
            emancipationVolume.onEnterVolume += OnPlayerEnterEmancipationVolume;
        }
    }

    void LoadLastCheckpoint() {
        if (playerDependencies == null) {
            Debug.LogError("PlayerDependencies not found when loading checkpoint!");
            return;
        }

        bool validCheckpointLoaded = checkpointManager.TryLoadCheckpoint(out var checkpointPosition, out var checkpointRotation);

        if (validCheckpointLoaded) {
            // Valid checkpoint found, use it
            spawnPoint.position = checkpointPosition;
            spawnPoint.rotation = checkpointRotation;

            // Find and deactivate the checkpoint at this position before resetting
            DeactivateCheckpointAtPosition(checkpointPosition);

            // Use ResetGameState instead of SafeTeleportToCheckpoint
            ResetGameState(false); // Don't play sound when loading checkpoint

            Debug.Log($"Loaded checkpoint at {checkpointPosition}!");
        }
        else {
            // No valid checkpoint found, use the existing spawnPoint
            Debug.Log("No valid checkpoint found. Using default spawn point.");

            // Update the lastCheckpointTransform to match the default spawn point
            checkpointManager.lastCheckpointTransform.position = spawnPoint.position;
            checkpointManager.lastCheckpointTransform.rotation = spawnPoint.rotation;

            // Use ResetGameState instead of SafeTeleportToCheckpoint
            ResetGameState(false); // Don't play sound when loading checkpoint
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

    public void LoadNextLevel() {
        StartCoroutine(LoadNextLevel_());
    }

    IEnumerator LoadNextLevel_() {
        crosshairUI.SetActive(false);
        endUI.SetActive(true);
        playerDependencies.rb.gameObject.SetActive(false);
        while (!Input.GetKeyDown(KeyCode.F)) {
            yield return null;
        }
        LoadNextScene();
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

    // Helper method to find and deactivate the checkpoint at a specific position
    void DeactivateCheckpointAtPosition(Vector3 position) {
        const float positionTolerance = 0.5f; // Adjust this value based on your needs

        foreach (var checkpoint in checkpointVolumes) {
            // Check if this checkpoint's position (plus offset) matches the spawn position
            var checkpointPos = checkpoint.transform.position;

            // Use distance check to account for small floating point differences
            if (Vector3.Distance(checkpointPos, position) < positionTolerance) {
                checkpoint.gameObject.SetActive(false);
                Debug.Log($"Deactivated checkpoint at {checkpointPos}");
                break;
            }
        }
    }
}