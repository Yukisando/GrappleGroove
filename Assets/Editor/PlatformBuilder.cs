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
    [MenuItem("Build/All Windows", priority = 100)]
    static void AllWindows() {
        SwitchBuildTargetWindows();
        PlayerSettings.productName = "CPPS_Bundle";
        var appName = "CPPS_Bundle.exe";
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, $"./Builds/Windows/Bundle/{appName}", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("Build/Windows", priority = 0)]
    static void Windows() {
        SwitchBuildTargetWindows();
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}.exe";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/Windows/{GetSceneNameDecorated()}/{appName}", BuildTarget.StandaloneWindows64,
            BuildOptions.None);
    }
    
    [MenuItem("Build/Windows (packed)", priority = 0)]
    static void WindowsPacked() {
        SwitchBuildTargetWindows();
        string sceneName = GetSceneNameDecorated();
        string appName = $"{sceneName}.exe";
        PlayerSettings.productName = GetSceneName();
        string buildPath = Path.GetFullPath($"./Builds/Windows/{sceneName}/"); // Get absolute path
        
        BuildPipeline.BuildPlayer(new[] {
            SceneManager.GetActiveScene().path,
        }, buildPath + appName, BuildTarget.StandaloneWindows64, BuildOptions.None);
        
        CreateWinRARSFX(buildPath, appName);
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
            if (file.Name != sfxFileName) { // Skip the SFX file
                file.Delete();
            }
        }
        
        foreach (var subDirectory in directoryInfo.GetDirectories()) {
            subDirectory.Delete(true); // Recursively delete all directories
        }
        
        Debug.Log("Cleanup complete, only " + sfxFileName + " remains in the build directory.");
    }
    
    static void CreateWinRARSFX(string buildPath, string appName) {
        string sfxFileName = $"{Path.GetFileNameWithoutExtension(appName)}_packed.exe";
        string sfxPath = Path.Combine(buildPath, sfxFileName);
        string winRarPath = @"C:\Program Files\WinRAR\WinRAR.exe";
        string configPath = Path.Combine(buildPath, "config.txt");
        
        string cmdArguments = $"a -m1 -sfx \"{sfxPath}\" \"*.*\" -r -z\"{configPath}\"";
        
        Debug.Log("WinRAR Command: " + winRarPath + " " + cmdArguments);
        
        WriteConfigFile(configPath, buildPath, appName);
        RunCommand(winRarPath, cmdArguments, buildPath);
        
        // Call cleanup function after creating SFX
        CleanupBuildDirectory(buildPath, sfxFileName);
    }
    
    static void RunCommand(string winRarPath, string arguments, string workingDirectory) {
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
        if (!string.IsNullOrEmpty(errors)) {
            Debug.LogError("WinRAR Error: " + errors);
        }
    }

    [MenuItem("Build/Windows Dev", priority = 1)]
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

    [MenuItem("Build/MacOS", priority = 2)]
    static void MacOSIntel() {
        SwitchBuildTargetMacOS();
        UserBuildSettings.architecture = OSArchitecture.x64ARM64;
        string sceneName = GetSceneNameDecorated();
        var appName = $"{sceneName}.app";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/OSX/{appName}", BuildTarget.StandaloneOSX, BuildOptions.None);
    }

    [MenuItem("Build/MacOS Dev", priority = 3)]
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
    }

    [MenuItem("Build/Android", priority = 4)]
    static void Android() {
        SwitchBuildTargetAndroid();
        PlayerSettings.Android.bundleVersionCode++;
        string sceneName = GetSceneNameDecorated();
        var apkName = $"{sceneName}.apk";
        PlayerSettings.productName = GetSceneName();

        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/Android/{apkName}", BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Build/Android Dev", priority = 5)]
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

    [MenuItem("Build/WebGL", priority = 6)]
    static void WebGL() {
        SwitchBuildTargetWebGL();
        string sceneName = GetSceneNameDecorated();
        var folderName = $"{sceneName}";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/WebGL/{folderName}", BuildTarget.WebGL, BuildOptions.None);
    }

    [MenuItem("Build/WebGL Dev", priority = 7)]
    static void WebGLDev() {
        SwitchBuildTargetWebGL();
        const BuildOptions options = BuildOptions.Development;
        string sceneName = GetSceneNameDecorated();
        var folderName = $"{sceneName}_dev";
        PlayerSettings.productName = GetSceneName();
        BuildPipeline.BuildPlayer(new[] {
                SceneManager.GetActiveScene().path,
            }, $"./Builds/WebGL/{folderName}", BuildTarget.WebGL, options);
    }

    [MenuItem("Build/Set target: Windows", priority = 8)]
    static void SwitchBuildTargetWindows() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Build/Set target: MacOS", priority = 9)]
    static void SwitchBuildTargetMacOS() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
    }

    [MenuItem("Build/Set target: Android", priority = 10)]
    static void SwitchBuildTargetAndroid() {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
    }

    [MenuItem("Build/Set target: WebGL", priority = 11)]
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

    static string GetSceneName() {
        return SceneManager.GetActiveScene().name;
    }

    static string GetSceneNameDecorated() {
        string name = GetSceneName();
        string finalName = char.ToUpper(name[0]) + name[1..] + "_v0";
        return finalName;
    }
}