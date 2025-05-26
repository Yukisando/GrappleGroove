#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

#endregion

/// <summary>
/// A Unity Editor Window to simplify building the game for different platforms.
/// It provides a single interface to select the platform, build options, and app name.
/// </summary>
public class PlatformBuilder : EditorWindow
{
    // Enum to define the target platforms for easier selection.
    enum BuildPlatform
    {
        Windows,
        MacOS,
        Android,
        WebGL,
    }

    // Fields to store the user's selections in the popup window.
    BuildPlatform selectedPlatform = BuildPlatform.Windows;
    bool isDebugBuild;
    bool autoRunPlayer;
    bool buildAllScenes;
    string appName = "";

    #region Menu Items

    /// <summary>
    /// Creates a menu item to open the main build window.
    /// </summary>
    [MenuItem("Tools/Build Game/Build Game &#B", priority = 0)] // Added hotkey Alt+Shift+B
    public static void ShowBuildWindow() {
        // Get existing open window or if none, make a new one.
        var window = GetWindow<PlatformBuilder>(true, "Build Game", true);
        window.minSize = new Vector2(350, 280); // <-- MODIFIED: Increased height
        window.maxSize = new Vector2(350, 280); // <-- MODIFIED: Increased height
        window.appName = GetDefaultAppName(); // Set a default name when opening
        window.ShowUtility(); // Show as a floating utility window.
    }

    /// <summary>
    /// Creates a menu item to directly open the main 'Builds' folder.
    /// </summary>
    [MenuItem("Tools/Build Game/Open Build Folder", priority = 100)]
    public static void OpenBuildFolderMenu() {
        OpenBuildLocation("./Builds/");
    }

    #endregion

    #region Editor Window GUI

    /// <summary>
    /// Renders the GUI for the build window.
    /// This is where we draw the dropdowns, toggles, text fields, and buttons.
    /// </summary>
    void OnGUI() {
        GUILayout.Label("Build Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // Platform Selection Dropdown
        selectedPlatform = (BuildPlatform)EditorGUILayout.EnumPopup("Platform:", selectedPlatform);

        // App Name Input
        appName = EditorGUILayout.TextField("App Name:", appName);
        EditorGUILayout.HelpBox("Avoid spaces in the app name for best results.", MessageType.Info);

        EditorGUILayout.Space(10);

        // Build Options Toggles
        buildAllScenes = EditorGUILayout.Toggle("Build All Scenes", buildAllScenes);
        isDebugBuild = EditorGUILayout.Toggle("Debug Build", isDebugBuild);
        autoRunPlayer = EditorGUILayout.Toggle("Auto Start Game", autoRunPlayer);

        EditorGUILayout.Space(15);

        // Build Button
        GUI.backgroundColor = new Color(0.6f, 1f, 0.6f); // Make the build button green!
        if (GUILayout.Button("Build Game", GUILayout.Height(30))) {
            if (string.IsNullOrEmpty(appName))
                EditorUtility.DisplayDialog("Error", "App Name cannot be empty.", "OK");
            else {
                TriggerBuild(); // Start the build process
                Close(); // Close the popup window after starting the build
            }
        }
        GUI.backgroundColor = Color.white; // Reset background color

        EditorGUILayout.Space(5);

        // Cancel Button
        if (GUILayout.Button("Cancel")) Close(); // Simply close the window.
    }

    #endregion

    #region Build Logic

    /// <summary>
    /// Gathers all selected options and initiates the build process.
    /// </summary>
    void TriggerBuild() {
        // Convert our enum selection to Unity's BuildTarget.
        var target = GetBuildTarget(selectedPlatform);

        // Determine the file extension based on the platform.
        string extension = GetExtension(selectedPlatform);

        // Set up the build options based on toggles.
        var options = BuildOptions.None;
        if (isDebugBuild) options |= BuildOptions.Development;
        if (autoRunPlayer) options |= BuildOptions.AutoRunPlayer;

        // Call the main build function.
        BuildGame(target, options, buildAllScenes, appName, extension);
    }

    /// <summary>
    /// The core build function that configures and runs the Unity Build Pipeline.
    /// </summary>
    /// <param name="target">The target platform (Windows, MacOS, etc.).</param>
    /// <param name="options">Build options (Debug, Autostart).</param>
    /// <param name="buildAll">Whether to build all scenes or just the current one.</param>
    /// <param name="name">The name for the application executable/folder.</param>
    /// <param name="extension">The file extension for the build (e.g., .exe, .app).</param>
    static void BuildGame(BuildTarget target, BuildOptions options, bool buildAll, string name, string extension) {
        PlayerSettings.productName = name; // Set the product name in Player Settings.

        // Platform-specific adjustments
        if (target == BuildTarget.Android)
            PlayerSettings.Android.bundleVersionCode++; // Increment Android version code.
        else if (target == BuildTarget.StandaloneOSX)

            // Ensure MacOS builds support both x64 and ARM64.
            EditorUserBuildSettings.SetPlatformSettings(
                "Standalone",
                "OSXUniversal",
                "Architecture",
                "x64ARM64"
            );
        else if (target == BuildTarget.WebGL)

            // WebGL doesn't support AutoRunPlayer, so remove it if set.
            options &= ~BuildOptions.AutoRunPlayer;

        // Get a friendly name for the platform to use in the build path.
        string platformName = GetFriendlyPlatformName(target);

        // Define the base path for our builds.
        string buildPath = Path.GetFullPath($"./Builds/{platformName}/");

        // Define the specific folder for this app build.
        string appFolder = Path.Combine(buildPath, name);

        // Create the directory if it doesn't exist.
        Directory.CreateDirectory(appFolder);

        // Define the final output path/file.
        string outputPath = target == BuildTarget.WebGL
            ? appFolder // WebGL builds output to a folder.
            : Path.Combine(appFolder, name + extension); // Other platforms output an executable.

        // Get the list of scenes to build.
        string[] scenes = buildAll
            ? GetEnabledScenes() // Get all scenes from Build Settings.
            : new[] {
                SceneManager.GetActiveScene().path,
            }; // Get only the current active scene.

        // Check if any scenes were found.
        if (scenes.Length == 0) {
            Debug.LogError("No scenes found to build. Ensure scenes are added and enabled in Build Settings, or that a scene is active.");
            return;
        }

        // Log the scenes being built for clarity.
        Debug.Log($"Starting build for {name} on {platformName} with scenes: {string.Join(", ", scenes)}");

        // Run the build!
        var report = BuildPipeline.BuildPlayer(scenes, outputPath, target, options);

        // Check the build result and open the folder if successful.
        if (report.summary.result == BuildResult.Succeeded) {
            Debug.Log($"Build successful! Output: {outputPath}");
            OpenBuildLocation(buildPath); // Open the folder containing the build.
        }
        else
            Debug.LogError($"Build failed: {report.summary.result}");
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Opens the specified folder path in the system's file explorer.
    /// </summary>
    /// <param name="path">The folder path to open.</param>
    static void OpenBuildLocation(string path) {
        string normalizedPath = Path.GetFullPath(path);

        // Ensure the directory exists before trying to open it.
        if (Directory.Exists(normalizedPath))
            Process.Start(new ProcessStartInfo {
                FileName = normalizedPath,
                UseShellExecute = true,
                Verb = "open",
            });
        else
            Debug.LogWarning($"Build folder not found at: {normalizedPath}");
    }

    /// <summary>
    /// Gets the default application name, either from the active scene or the project folder.
    /// </summary>
    /// <returns>A default application name.</returns>
    static string GetDefaultAppName() {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!string.IsNullOrEmpty(sceneName)) return char.ToUpper(sceneName[0]) + sceneName[1..];

        // Fallback to project folder name if scene name is empty.
        return Directory.GetParent(Application.dataPath).Name;
    }

    /// <summary>
    /// Gets all scenes that are added and enabled in the Build Settings.
    /// </summary>
    /// <returns>An array of scene paths.</returns>
    static string[] GetEnabledScenes() {
        return EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();
    }

    /// <summary>
    /// Converts our BuildPlatform enum to Unity's BuildTarget enum.
    /// </summary>
    static BuildTarget GetBuildTarget(BuildPlatform platform) {
        switch (platform) {
            case BuildPlatform.Windows: return BuildTarget.StandaloneWindows64;
            case BuildPlatform.MacOS: return BuildTarget.StandaloneOSX;
            case BuildPlatform.Android: return BuildTarget.Android;
            case BuildPlatform.WebGL: return BuildTarget.WebGL;
            default: throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
        }
    }

    /// <summary>
    /// Gets the file extension for the build based on the platform.
    /// </summary>
    static string GetExtension(BuildPlatform platform) {
        switch (platform) {
            case BuildPlatform.Windows: return ".exe";
            case BuildPlatform.MacOS: return ".app";
            case BuildPlatform.Android: return ".apk";
            case BuildPlatform.WebGL: return ""; // WebGL doesn't have a single file extension.
            default: return "";
        }
    }

    /// <summary>
    /// Gets a user-friendly name for the platform, used for folder creation.
    /// </summary>
    static string GetFriendlyPlatformName(BuildTarget target) {
        switch (target) {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "MacOS";
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.WebGL:
                return "WebGL";
            default:
                return target.ToString(); // Fallback to the standard name.
        }
    }

    #endregion
}