#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrototypeFPC;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    [Header("Game dependencies")]
    [SerializeField] SaveManager saveManager;
    [SerializeField] Transform spawnPoint;
    [SerializeField] InfoPopup infoPopup;
    [SerializeField] GameObject crosshairUI;
    [SerializeField] GameObject playerUI;
    [SerializeField] MenuOverlay menuUI;
    [SerializeField] EndOverlay endUI;

    [Header("Settings")]
    [SerializeField] KeyCode respawnKey = KeyCode.Q;
    [SerializeField] KeyCode restartKey = KeyCode.F5;
    [SerializeField] KeyCode skipLevel = KeyCode.F9;
    [SerializeField] KeyCode menuKey = KeyCode.Escape;
    [SerializeField] KeyCode clearAllSaveKey = KeyCode.F6;
    [SerializeField] KeyCode continueKey = KeyCode.F;

    [Header("Audio")]
    [SerializeField] bool raceWithMusic = true;
    [SerializeField] AudioSource soundEffectSource;
    [SerializeField] AudioSource musicSequenceSource;

    [Header("Performance")]
    [SerializeField] int targetFrameRate = 120;

    //Volumes
    List<ID> allIds;
    List<CheckpointVolume> checkpointVolumes;
    List<RopeEmancipationVolume> emancipationVolumes;
    List<KillVolume> killVolumes;
    List<ResetVolume> resetVolumes;

    //timer
    float elapsedTime;
    bool timerRunning;
    Coroutine timerCoroutine;
    public bool checkpointsActive;

    [HideInInspector] public PlayerDependencies playerDependencies;

    void Awake() {
        I = this;
        playerDependencies = FindAnyObjectByType<PlayerDependencies>();
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
        InitializeWorldObjects();

        // Check if teleport on play is enabled in editor preferences
        var skipCheckpointLoading = false;
#if UNITY_EDITOR
        skipCheckpointLoading = EditorPrefs.GetBool("TeleportPlayerOnPlay_Enabled", false);
#endif

        if (!skipCheckpointLoading) StartCoroutine(DelayedLoadCheckpoint());

        playerDependencies.perspective.sensitivity = PlayerPrefs.HasKey("sensitivity") ? PlayerPrefs.GetFloat("sensitivity") : 180f;
    }

    IEnumerator DelayedLoadCheckpoint() {
        yield return null;
        LoadLastCheckpoint();
    }

    public void ActivateCheckpoints(bool state) {
        foreach (var cv in checkpointVolumes) {
            cv.gameObject.SetActive(state);
        }
        checkpointsActive = state;
    }

    void Update() {
        CheckInputs();
        CheckSpeed();
    }

    public void StartTimer() {
        ResetTimer();
        timerRunning = true;
        timerCoroutine = StartCoroutine(UpdateTimer());
        soundEffectSource.PlayOneShot(AssetManager.I.startTimerSound);
        if (raceWithMusic) BeginMusicSequence(AssetManager.I.defaultRaceClip);
    }

    bool menuShown;

    void ShowMenu(bool _show) {
        Application.runInBackground = _show;
        playerDependencies.grapplingHook.enabled = !_show;
        if (_show) {
            AssetManager.I.PlayClip(AssetManager.I.onClip);
            menuShown = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            menuUI.Populate();
            menuUI.gameObject.SetActive(true);
        }
        else {
            AssetManager.I.PlayClip(AssetManager.I.offClip);
            menuShown = false;
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            menuUI.gameObject.SetActive(false);
        }
    }

    public void SetMouseSensitivity(float value) {
        playerDependencies.perspective.sensitivity = value;
        PlayerPrefs.SetFloat("sensitivity", value);
    }

    public void StopTimer(bool finished = true) {
        musicSequenceSource.Stop();
        if (timerRunning) {
            timerRunning = false;
            if (timerCoroutine != null) StopCoroutine(timerCoroutine);
            soundEffectSource.PlayOneShot(AssetManager.I.stopTimerSound);
            if (finished)
                if (!PlayerPrefs.HasKey($"best_{SceneManager.GetActiveScene().name}") || elapsedTime < PlayerPrefs.GetFloat($"best_{SceneManager.GetActiveScene().name}"))
                    PlayerPrefs.SetFloat($"best_{SceneManager.GetActiveScene().name}", elapsedTime);
        }
    }

    public void ResetTimer() {
        StopTimer();
        elapsedTime = 0f;
        PlayerOverlay.I.SetTimer(elapsedTime);
    }

    void OnApplicationQuit() {
        PlayerPrefs.Save();
    }

    IEnumerator UpdateTimer() {
        float lastSecond = -1f;
        while (timerRunning) {
            elapsedTime += Time.deltaTime;
            PlayerOverlay.I.SetTimer(elapsedTime);

            // Check if we've crossed into a new second
            int currentSecond = Mathf.FloorToInt(elapsedTime);
            if (currentSecond > lastSecond) {
                soundEffectSource.PlayOneShot(AssetManager.I.timerTickSound);
                lastSecond = currentSecond;
            }

            yield return null;
        }
    }

    void CheckSpeed() {
        PlayerOverlay.I.SetSpeed(playerDependencies.rb.linearVelocity.magnitude);
    }

    void CheckInputs() {
        if (Input.GetKeyDown(menuKey)) ShowMenu(!menuShown);
        if (Input.GetKeyDown(clearAllSaveKey)) saveManager.DeleteAllSaveFiles();
        if (Input.GetKeyDown(respawnKey)) ResetGameState();
        if (Input.GetKeyDown(skipLevel)) LoadNextScene();
        if (Input.GetKeyDown(restartKey)) Restart();
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void BeginMusicSequence(AudioClip clip) {
        musicSequenceSource.clip = clip;
        musicSequenceSource.Play();
    }

    public void LoadNextScene() {
        int i = SceneManager.GetActiveScene().buildIndex + 1;
        if (i >= SceneManager.sceneCountInBuildSettings) i = 0;
        SceneManager.LoadScene(i);
    }

    void InitializeWorldObjects() {
        checkpointVolumes = FindObjectsByType<CheckpointVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        resetVolumes = FindObjectsByType<ResetVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        killVolumes = FindObjectsByType<KillVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        emancipationVolumes = FindObjectsByType<RopeEmancipationVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        allIds = FindObjectsByType<ID>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

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

        ActivateCheckpoints(true);
    }

    void LoadLastCheckpoint() {
        if (playerDependencies == null) {
            Debug.LogError("PlayerDependencies not found when loading checkpoint!");
            return;
        }

        bool validCheckpointLoaded = saveManager.TryLoadCheckpoint(out var checkpointPosition, out var checkpointRotation);

        if (validCheckpointLoaded) {
            // Valid checkpoint found, use it
            spawnPoint.position = checkpointPosition;
            spawnPoint.rotation = checkpointRotation;

            // Find and deactivate the checkpoint at this position before resetting
            DeactivateCheckpointAtPosition(checkpointPosition);

            ResetGameState(false); // Don't play sound when loading checkpoint

            Debug.Log($"Loaded checkpoint at {checkpointPosition}!");
        }
        else {
            // No valid checkpoint found, use the existing spawnPoint
            Debug.Log("No valid checkpoint found. Using default spawn point.");

            // Update the lastCheckpointTransform to match the default spawn point
            saveManager.lastCheckpointTransform.position = spawnPoint.position;
            saveManager.lastCheckpointTransform.rotation = spawnPoint.rotation;

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
        StopTimer();
        if (_playSound)
            AssetManager.I.PlayClip(AssetManager.I.spawnClip);

        SafeTeleportToCheckpoint(spawnPoint.position, spawnPoint.rotation);

        foreach (var id in allIds) {
            if (id.id == "Player") continue; // Skip player ID
            id.ResetObject();
        }
    }

    void OnPlayerEnterEmancipationVolume(RopeType _ropeType) {
        playerDependencies.grapplingHook.DestroyRopes(_ropeType);
    }

    public void LoadLevel(string levelName) {
        if (string.IsNullOrEmpty(levelName)) return;
        SceneManager.LoadScene(levelName);
    }

    public void EndReached() {
        crosshairUI.SetActive(false);
        endUI.Populate();
        endUI.gameObject.SetActive(true);
        playerDependencies.rb.gameObject.SetActive(false);
        StartCoroutine(WaitInputNextLevel_());
    }

    IEnumerator WaitInputNextLevel_() {
        while (!Input.GetKeyDown(continueKey)) {
            if (Input.GetKeyDown(KeyCode.X)) {
                PlayerPrefs.DeleteKey("best_" + SceneManager.GetActiveScene().name);
                endUI.Populate();
            }
            if (Input.GetKeyDown(KeyCode.R)) Restart();
            yield return null;
        }
        LoadNextScene();
    }

    void OnPlayerEnteredCheckpointVolume(Transform _spawnPoint) {
        AssetManager.I.PlayClip(AssetManager.I.checkpointSound);
        saveManager.SaveCheckpoint(_spawnPoint);
        spawnPoint.position = _spawnPoint.position;
        spawnPoint.rotation = _spawnPoint.rotation;
        infoPopup.ShowPopup("Checkpoint reached!", false);
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