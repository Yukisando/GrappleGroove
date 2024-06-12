#region

using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.OSXStandalone;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

#endregion

static class PlatformBuilder
{
    [MenuItem("Build/Go to Builds Folder", priority = 101)]
    static void GoToBuildsFolder() {
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Windows/All selected scenes", priority = 100)]
    static void AllWindows() {
        SwitchBuildTargetWindows();
        PlayerSettings.productName = "Bundle";
        var appName = "Bundle.exe";
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, $"./Builds/Windows/Bundle/{appName}", BuildTarget.StandaloneWindows64, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Windows/All selected scenes (packed)", priority = 100)]
    static void AllWindowsPacked() {
        SwitchBuildTargetWindows();
        PlayerSettings.productName = "Bundle_packed";
        var appName = "Bundle_packed.exe";
        string buildPath = Path.GetFullPath("./Builds/Windows/Bundle/");
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath + appName, BuildTarget.StandaloneWindows64, BuildOptions.None);

        CreateWinRARSFX(buildPath, appName);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Windows/Current scene", priority = 0)]
    static void Windows() {
        SwitchBuildTargetWindows();
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}.exe";
        PlayerSettings.productName = GetSceneName();
        string buildPath = Path.GetFullPath($"./Builds/Windows/{sceneName}/");

        BuildPipeline.BuildPlayer(new[] {
            SceneManager.GetActiveScene().path,
        }, buildPath + appName, BuildTarget.StandaloneWindows64, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Windows/Current scene (packed)", priority = 0)]
    static void WindowsPacked() {
        SwitchBuildTargetWindows();
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}.exe";
        PlayerSettings.productName = GetSceneName();
        string buildPath = Path.GetFullPath($"./Builds/Windows/{sceneName}/");

        BuildPipeline.BuildPlayer(new[] {
            SceneManager.GetActiveScene().path,
        }, buildPath + appName, BuildTarget.StandaloneWindows64, BuildOptions.None);

        CreateWinRARSFX(buildPath, appName);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Windows/Current scene (dev autostart)", priority = 1)]
    static void WindowsDev() {
        SwitchBuildTargetWindows();
        const BuildOptions options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}_dev.exe";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/Windows_DEV/{GetSceneNameDecorated()}/{appName}", BuildTarget.StandaloneWindows64, options);
    }

    [MenuItem("Build/OSX/All selected scenes", priority = 2)]
    static void MacOSAll() {
        SwitchBuildTargetMacOS();
        UserBuildSettings.architecture = OSArchitecture.x64ARM64;
        var appName = "Bundle.app";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/OSX/{appName}", BuildTarget.StandaloneOSX, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/OSX/Current scene", priority = 2)]
    static void MacOS() {
        SwitchBuildTargetMacOS();
        UserBuildSettings.architecture = OSArchitecture.x64ARM64;
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}.app";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/OSX/{appName}", BuildTarget.StandaloneOSX, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/OSX/Current scene (dev)", priority = 3)]
    static void MacOSDev() {
        SwitchBuildTargetMacOS();
        UserBuildSettings.architecture = OSArchitecture.x64ARM64;
        const BuildOptions options = BuildOptions.Development | BuildOptions.AutoRunPlayer;
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}_dev.app";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/OSX_DEV/{appName}", BuildTarget.StandaloneOSX, options);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Android/All selected scenes", priority = 4)]
    static void AndroidAll() {
        SwitchBuildTargetAndroid();
        PlayerSettings.productName = "Bundle";
        PlayerSettings.Android.bundleVersionCode++;
        var apkName = "Bundle.apk";
        PlayerSettings.productName = GetSceneName();

        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, $"./Builds/Android/{apkName}", BuildTarget.Android, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Android/Current scene", priority = 4)]
    static void Android() {
        SwitchBuildTargetAndroid();
        PlayerSettings.Android.bundleVersionCode++;
        string sceneName = GetSceneNameDecorated();
        var apkName = $"{sceneName}.apk";
        PlayerSettings.productName = GetSceneName();

        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/Android/{apkName}", BuildTarget.Android, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/Android/Current scene (dev autostart)", priority = 5)]
    static void AndroidDev() {
        SwitchBuildTargetAndroid();
        PlayerSettings.Android.bundleVersionCode++;
        const BuildOptions options = BuildOptions.AutoRunPlayer | BuildOptions.Development;
        string sceneName = GetSceneNameDecorated();
        var apkName = $"{sceneName}_dev.apk";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/Android_DEV/{apkName}", BuildTarget.Android, options);
    }

    [MenuItem("Build/WebGL/Current scene", priority = 6)]
    static void WebGL() {
        SwitchBuildTargetWebGL();
        string sceneName = GetSceneNameDecorated();
        var folderName = $"{sceneName}";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/WebGL/{folderName}", BuildTarget.WebGL, BuildOptions.None);
        OpenBuildLocation("./Builds/");
    }

    [MenuItem("Build/WebGL/Current scene (dev autostart)", priority = 7)]
    static void WebGLDev() {
        SwitchBuildTargetWebGL();
        const BuildOptions options = BuildOptions.AutoRunPlayer | BuildOptions.Development;
        string sceneName = GetSceneNameDecorated();
        var folderName = $"{sceneName}_dev";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/WebGL/{folderName}", BuildTarget.WebGL, options);
    }

    static void SwitchBuildTargetWindows() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
    }

    static void SwitchBuildTargetMacOS() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
    }

    static void SwitchBuildTargetAndroid() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
    }

    static void SwitchBuildTargetWebGL() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
    }

    static void OpenBuildLocation(string path) {
        // Normalize the path to ensure it's using the correct directory separator for the platform
        string normalizedPath = Path.GetFullPath(path);

        // Open the folder in explorer
        Process.Start(new ProcessStartInfo {
            FileName = normalizedPath,
            UseShellExecute = true,
            Verb = "open",
        });
    }

    static void WriteConfigFile(string configPath, string buildPath, string appName) {
        string configContents = ";The comment below contains SFX script commands\n"
                                + $"Path={buildPath}\n"
                                + "SavePath\n"
                                + $"Setup={appName}\n"
                                + "TempMode\n"
                                + "Silent=1\n"
                                + "Overwrite=1";
        File.WriteAllText(configPath, configContents);
    }

    static void CleanupBuildDirectory(string buildPath, string sfxFileName) {
        var directoryInfo = new DirectoryInfo(buildPath);

        foreach (var file in directoryInfo.GetFiles()) {
            if (file.Name != sfxFileName) // Skip the SFX file
                file.Delete();
        }

        foreach (var subDirectory in directoryInfo.GetDirectories()) {
            subDirectory.Delete(true); // Recursively delete all directories
        }

        Debug.Log("Cleanup complete, only " + sfxFileName + " remains in the build directory.");
    }

    static void CreateWinRARSFX(string buildPath, string appName) {
        var sfxFileName = $"{Path.GetFileNameWithoutExtension(appName)}_packed.exe";
        string sfxPath = Path.Combine(buildPath, sfxFileName);
        var winRarPath = @"C:\Program Files\WinRAR\WinRAR.exe";
        string configPath = Path.Combine(buildPath, "config.txt");

        var cmdArguments = $"a -m1 -sfx \"{sfxPath}\" \"*.*\" -r -z\"{configPath}\"";

        Debug.Log("WinRAR Command: " + winRarPath + " " + cmdArguments);

        WriteConfigFile(configPath, buildPath, appName);
        StartPackingProcess(winRarPath, cmdArguments, buildPath);

        // Call cleanup function after creating SFX
        CleanupBuildDirectory(buildPath, sfxFileName);
    }

    static void StartPackingProcess(string winRarPath, string arguments, string workingDirectory) {
        var startInfo = new ProcessStartInfo {
            FileName = winRarPath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory, // Set working directory here
        };

        using var process = Process.Start(startInfo);
        string output = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Debug.Log("WinRAR Output: " + output);
        if (!string.IsNullOrEmpty(errors)) Debug.LogError("WinRAR Error: " + errors);
    }

    static string GetSceneName() {
        return SceneManager.GetActiveScene().name;
    }

    static string GetSceneNameDecorated() {
        string name = GetSceneName();
        string finalName = char.ToUpper(name[0]) + name[1..] + "_v0";
        return finalName;
    }
}