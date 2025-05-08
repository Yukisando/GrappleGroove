#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

#endregion

public class PlatformBuilder : EditorWindow
{
    static Action<string, BuildTarget, BuildOptions> onNameConfirmed;
    string appName;
    BuildTarget target;
    BuildOptions options;

    static void ShowDialog(string defaultName, BuildTarget target, BuildOptions options, Action<string, BuildTarget, BuildOptions> onConfirm) {
        var window = GetWindow<PlatformBuilder>(true, "Enter App Name", true);
        window.appName = defaultName;
        window.target = target;
        window.options = options;
        onNameConfirmed = onConfirm;
        window.minSize = new Vector2(300, 100);
        window.ShowUtility();
    }

    void OnGUI() {
        GUILayout.Label("Name of the app will be: (Avoid spaces)", EditorStyles.boldLabel);
        appName = EditorGUILayout.TextField(appName);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("OK")) {
            if (!string.IsNullOrEmpty(appName)) onNameConfirmed?.Invoke(appName, target, options);
            Close();
        }

        if (GUILayout.Button("Cancel")) Close();

        GUILayout.EndHorizontal();
    }

    // --------------------------- BUILD SYSTEM ---------------------------

    static void BuildForPlatform(BuildTarget target, BuildOptions options, string extension) {
        string defaultAppName = GetSceneNameDecorated();
        ShowDialog(defaultAppName, target, options, (appName, buildTarget, buildOptions) => {
            if (string.IsNullOrEmpty(appName)) return;

            PlayerSettings.productName = appName;

            // Get a more user-friendly platform name
            string platformName = GetFriendlyPlatformName(buildTarget);

            string buildPath = Path.GetFullPath($"./Builds/{platformName}/");
            string appFolder = Path.Combine(buildPath, appName);
            Directory.CreateDirectory(appFolder);

            string outputPath = buildTarget == BuildTarget.WebGL
                ? appFolder
                : Path.Combine(appFolder, appName + extension);

            string[] scenes = {
                SceneManager.GetActiveScene().path,
            };

            if (buildTarget == BuildTarget.WebGL) {
                // Warn if scene isn't in Build Settings
                if (!EditorBuildSettings.scenes.Any(s => s.enabled && s.path == scenes[0])) Debug.LogWarning("The active scene is not listed/enabled in Build Settings. WebGL build may fail.");

                // AutoRunPlayer is not supported in WebGL
                buildOptions &= ~BuildOptions.AutoRunPlayer;
            }

            BuildPipeline.BuildPlayer(scenes, outputPath, buildTarget, buildOptions);
            OpenBuildLocation(buildPath);
        });
    }

    static void BuildAllSelectedScenesForPlatform(BuildTarget target, string extension) {
        // Remove any existing "_AllScenes" suffix before adding it once
        string baseName = Application.productName.Replace("_AllScenes", "");
        string defaultAppName = baseName + "_AllScenes";

        ShowDialog(defaultAppName, target, BuildOptions.None, (appName, buildTarget, buildOptions) => {
            if (string.IsNullOrEmpty(appName)) return;

            PlayerSettings.productName = appName;

            // Get a more user-friendly platform name
            string platformName = GetFriendlyPlatformName(buildTarget);

            string buildPath = Path.GetFullPath($"./Builds/{platformName}/");
            string appFolder = Path.Combine(buildPath, appName);
            Directory.CreateDirectory(appFolder);

            string outputPath = buildTarget == BuildTarget.WebGL
                ? appFolder
                : Path.Combine(appFolder, appName + extension);

            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0) {
                Debug.LogWarning("No scenes selected in Build Settings!");
                return;
            }

            if (buildTarget == BuildTarget.WebGL) buildOptions &= ~BuildOptions.AutoRunPlayer;

            BuildPipeline.BuildPlayer(scenes, outputPath, buildTarget, buildOptions);
            OpenBuildLocation(buildPath);
        });
    }

    // ---- Windows ----
    [MenuItem("Tools/Platform Builder/Windows/Current scene", priority = 0)]
    static void Windows() {
        BuildForPlatform(BuildTarget.StandaloneWindows64, BuildOptions.None, ".exe");
    }

    [MenuItem("Tools/Platform Builder/Windows/Current scene (autostart)", priority = 1)]
    static void WindowsAutostart() {
        BuildForPlatform(BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer, ".exe");
    }

    [MenuItem("Tools/Platform Builder/Windows/Current scene (dev autostart)", priority = 2)]
    static void WindowsDevAutostart() {
        BuildForPlatform(BuildTarget.StandaloneWindows64, BuildOptions.Development | BuildOptions.AutoRunPlayer, ".exe");
    }

    [MenuItem("Tools/Platform Builder/Windows/Build All Selected scenes", priority = 11)]
    static void WindowsAllScenes() {
        BuildAllSelectedScenesForPlatform(BuildTarget.StandaloneWindows64, ".exe");
    }

    // ---- MacOS ----
    [MenuItem("Tools/Platform Builder/MacOS/Current scene", priority = 3)]
    static void MacOS() {
        BuildForPlatform(BuildTarget.StandaloneOSX, BuildOptions.None, ".app");
    }

    [MenuItem("Tools/Platform Builder/MacOS/Current scene (autostart)", priority = 4)]
    static void MacOSAutostart() {
        BuildForPlatform(BuildTarget.StandaloneOSX, BuildOptions.AutoRunPlayer, ".app");
    }

    [MenuItem("Tools/Platform Builder/MacOS/Current scene (dev autostart)", priority = 5)]
    static void MacOSDevAutostart() {
        BuildForPlatform(BuildTarget.StandaloneOSX, BuildOptions.Development | BuildOptions.AutoRunPlayer, ".app");
    }

    [MenuItem("Tools/Platform Builder/MacOS/Build All Selected scenes", priority = 12)]
    static void MacOSAllScenes() {
        BuildAllSelectedScenesForPlatform(BuildTarget.StandaloneOSX, ".app");
    }

    // ---- Android ----
    [MenuItem("Tools/Platform Builder/Android/Current scene", priority = 6)]
    static void Android() {
        PlayerSettings.Android.bundleVersionCode++;
        BuildForPlatform(BuildTarget.Android, BuildOptions.None, ".apk");
    }

    [MenuItem("Tools/Platform Builder/Android/Current scene (autostart)", priority = 7)]
    static void AndroidAutostart() {
        PlayerSettings.Android.bundleVersionCode++;
        BuildForPlatform(BuildTarget.Android, BuildOptions.AutoRunPlayer, ".apk");
    }

    [MenuItem("Tools/Platform Builder/Android/Current scene (dev autostart)", priority = 8)]
    static void AndroidDevAutostart() {
        PlayerSettings.Android.bundleVersionCode++;
        BuildForPlatform(BuildTarget.Android, BuildOptions.Development | BuildOptions.AutoRunPlayer, ".apk");
    }

    [MenuItem("Tools/Platform Builder/Android/Build All Selected scenes", priority = 13)]
    static void AndroidAllScenes() {
        PlayerSettings.Android.bundleVersionCode++;
        BuildAllSelectedScenesForPlatform(BuildTarget.Android, ".apk");
    }

    // ---- WebGL ----
    [MenuItem("Tools/Platform Builder/WebGL/Current scene", priority = 9)]
    static void WebGL() {
        BuildForPlatform(BuildTarget.WebGL, BuildOptions.None, "");
    }

    [MenuItem("Tools/Platform Builder/WebGL/Build All Selected scenes", priority = 14)]
    static void WebGLAllScenes() {
        BuildAllSelectedScenesForPlatform(BuildTarget.WebGL, "");
    }

    // --------------------------- HELPER FUNCTIONS ---------------------------

    static void OpenBuildLocation(string path) {
        string normalizedPath = Path.GetFullPath(path);
        Process.Start(new ProcessStartInfo {
            FileName = normalizedPath,
            UseShellExecute = true,
            Verb = "open",
        });
    }

    [MenuItem("Tools/Platform Builder/Open build folder", priority = 100)]
    public static void OpenBuildFolder() {
        OpenBuildLocation("./Builds/");
    }

    static string GetSceneName() {
        return SceneManager.GetActiveScene().name;
    }

    static string GetSceneNameDecorated() {
        string name = GetSceneName();
        return char.ToUpper(name[0]) + name[1..];
    }

    // Helper method to convert BuildTarget to a friendly name
    static string GetFriendlyPlatformName(BuildTarget target) {
        switch (target) {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.WebGL:
                return "WebGL";
            default:
                return target.ToString();
        }
    }
}