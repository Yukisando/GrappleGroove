#region

using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.OSXStandalone;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

#endregion

static class PlatformBuilder
{
    const string BUILDS_FOLDER = "./Builds/";
    const string SEVEN_ZIP_PATH = @"C:\Program Files\7-Zip\7z.exe";
    const string SEVEN_ZIP_SFX_MODULE = @"C:\Program Files\7-Zip\7z.sfx";

    [MenuItem("Build/Windows/All selected scenes", priority = 100)]
    static void AllWindows() {
        BuildWindows(PlayerSettings.productName, true);
    }

    [MenuItem("Build/Windows/All selected scenes (packed)", priority = 100)]
    static void AllWindowsPacked() {
        BuildWindows(PlayerSettings.productName, true, true);
    }

    [MenuItem("Build/Windows/Current scene", priority = 0)]
    static void Windows() {
        BuildWindows(PlayerSettings.productName, false);
    }

    [MenuItem("Build/Windows/Current scene (packed)", priority = 0)]
    static void WindowsPacked() {
        BuildWindows(PlayerSettings.productName, false, true);
    }

    [MenuItem("Build/Windows/Current scene (dev autostart)", priority = 1)]
    static void WindowsDev() {
        BuildWindows(PlayerSettings.productName, false, false, true);
    }

    [MenuItem("Build/OSX/All selected scenes", priority = 2)]
    static void MacOSAll() {
        BuildMacOS(PlayerSettings.productName, true);
    }

    [MenuItem("Build/OSX/Current scene", priority = 2)]
    static void MacOS() {
        BuildMacOS(PlayerSettings.productName, false);
    }

    [MenuItem("Build/OSX/Current scene (dev)", priority = 3)]
    static void MacOSDev() {
        BuildMacOS(PlayerSettings.productName, false, true);
    }

    [MenuItem("Build/Android/All selected scenes", priority = 4)]
    static void AndroidAll() {
        BuildAndroid(PlayerSettings.productName, true);
    }

    [MenuItem("Build/Android/Current scene", priority = 4)]
    static void Android() {
        BuildAndroid(PlayerSettings.productName, false);
    }

    [MenuItem("Build/Android/Current scene (dev autostart)", priority = 5)]
    static void AndroidDev() {
        BuildAndroid(PlayerSettings.productName, false, true);
    }

    [MenuItem("Build/WebGL/Current scene", priority = 6)]
    static void WebGL() {
        BuildWebGL(PlayerSettings.productName, false);
    }

    [MenuItem("Build/WebGL/Current scene (dev autostart)", priority = 7)]
    static void WebGLDev() {
        BuildWebGL(PlayerSettings.productName, false, true);
    }

    static void BuildWindows(string productName, bool allScenes, bool packed = false, bool dev = false) {
        SwitchBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
        string outputName = GetOutputName(allScenes, productName);
        string appName = $"{outputName}.exe";
        string buildPath = Path.GetFullPath($"{BUILDS_FOLDER}Windows/{(dev ? "DEV/" : "")}{outputName}/");

        var buildPlayerOptions = new BuildPlayerOptions {
            scenes = allScenes
                ? EditorBuildSettings.scenes.Select(s => s.path).ToArray()
                : new[] {
                    SceneManager.GetActiveScene().path,
                },
            locationPathName = buildPath + appName,
            target = BuildTarget.StandaloneWindows64,
            options = dev ? BuildOptions.Development | BuildOptions.AutoRunPlayer : BuildOptions.None,
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (packed) {
            Create7ZipSFX(buildPath, appName, outputName);
        }

        if (!dev) OpenBuildLocation(BUILDS_FOLDER);
    }

    static void BuildMacOS(string productName, bool allScenes, bool dev = false) {
        SwitchBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
        UserBuildSettings.architecture = OSArchitecture.x64ARM64;
        string outputName = GetOutputName(allScenes, productName);
        string appName = $"{outputName}.app";
        string buildPath = $"{BUILDS_FOLDER}OSX/{(dev ? "DEV/" : "")}";

        var buildPlayerOptions = new BuildPlayerOptions {
            scenes = allScenes
                ? EditorBuildSettings.scenes.Select(s => s.path).ToArray()
                : new[] {
                    SceneManager.GetActiveScene().path,
                },
            locationPathName = buildPath + appName,
            target = BuildTarget.StandaloneOSX,
            options = dev ? BuildOptions.Development | BuildOptions.AutoRunPlayer : BuildOptions.None,
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (!dev) OpenBuildLocation(BUILDS_FOLDER);
    }

    static void BuildAndroid(string productName, bool allScenes, bool dev = false) {
        SwitchBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.Android.bundleVersionCode++;
        string outputName = GetOutputName(allScenes, productName);
        string apkName = $"{outputName}.apk";
        string buildPath = $"{BUILDS_FOLDER}Android/{(dev ? "DEV/" : "")}";

        var buildPlayerOptions = new BuildPlayerOptions {
            scenes = allScenes
                ? EditorBuildSettings.scenes.Select(s => s.path).ToArray()
                : new[] {
                    SceneManager.GetActiveScene().path,
                },
            locationPathName = buildPath + apkName,
            target = BuildTarget.Android,
            options = dev ? BuildOptions.Development | BuildOptions.AutoRunPlayer : BuildOptions.None,
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (!dev) OpenBuildLocation(BUILDS_FOLDER);
    }

    static void BuildWebGL(string productName, bool allScenes, bool dev = false) {
        SwitchBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        string outputName = GetOutputName(allScenes, productName);
        string folderName = $"{outputName}{(dev ? "_dev" : "")}";
        string buildPath = $"{BUILDS_FOLDER}WebGL/{folderName}";

        var buildPlayerOptions = new BuildPlayerOptions {
            scenes = allScenes
                ? EditorBuildSettings.scenes.Select(s => s.path).ToArray()
                : new[] {
                    SceneManager.GetActiveScene().path,
                },
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = dev ? BuildOptions.Development | BuildOptions.AutoRunPlayer : BuildOptions.None,
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (!dev) OpenBuildLocation(BUILDS_FOLDER);
    }

    static void SwitchBuildTarget(BuildTargetGroup group, BuildTarget target) {
        if (EditorUserBuildSettings.activeBuildTarget != target) {
            EditorUserBuildSettings.SwitchActiveBuildTarget(group, target);
        }
    }

    static void OpenBuildLocation(string path) {
        string normalizedPath = Path.GetFullPath(path);
        Process.Start(new ProcessStartInfo {
            FileName = normalizedPath,
            UseShellExecute = true,
            Verb = "open",
        });
    }

    static void Create7ZipSFX(string buildPath, string appName, string outputName) {
        Debug.Log("Starting 7-Zip SFX creation...");
        Debug.Log($"Build Path: {buildPath}");
        Debug.Log($"App Name: {appName}");

        string sfxFileName = $"{outputName}_packed.exe";
        string sfxPath = Path.Combine(buildPath, sfxFileName);
        string configPath = Path.Combine(buildPath, "config.txt");
        string tempArchivePath = Path.Combine(buildPath, "temp.7z");

        Debug.Log($"SFX File Name: {sfxFileName}");

        // Write config file
        File.WriteAllText(configPath, $@";!@Install@!UTF-8!
Title=""Extracting {PlayerSettings.productName}""
BeginPrompt=""Do you want to extract {PlayerSettings.productName}?""
ExecuteFile=""{appName}""
Silent=1
OverwriteMode=""1""
;!@InstallEnd@!");

        // Create archive with faster compression
        RunProcess(SEVEN_ZIP_PATH, $"a -t7z \"{tempArchivePath}\" \"{buildPath}*\" -r -mx=1", buildPath);

        // Combine SFX module, config, and archive
        using (var stream = new FileStream(sfxPath, FileMode.Create)) {
            stream.Write(File.ReadAllBytes(SEVEN_ZIP_SFX_MODULE), 0, (int)new FileInfo(SEVEN_ZIP_SFX_MODULE).Length);
            stream.Write(File.ReadAllBytes(configPath), 0, (int)new FileInfo(configPath).Length);
            stream.Write(File.ReadAllBytes(tempArchivePath), 0, (int)new FileInfo(tempArchivePath).Length);
        }

        // Cleanup temporary files
        File.Delete(tempArchivePath);
        File.Delete(configPath);

        CleanupBuildDirectory(buildPath, sfxFileName);
    }

    static void RunProcess(string fileName, string arguments, string workingDirectory) {
        var startInfo = new ProcessStartInfo {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = true,
            CreateNoWindow = false,
            WorkingDirectory = workingDirectory,
        };

        using var process = new Process();
        process.StartInfo = startInfo;

        process.Start();
        process.WaitForExit();

        Debug.Log($"7-Zip process exited with code: {process.ExitCode}");
    }

    static void CleanupBuildDirectory(string buildPath, string sfxFileName) {
        var directoryInfo = new DirectoryInfo(buildPath);
        foreach (var file in directoryInfo.GetFiles()) {
            if (file.Name != sfxFileName) {
                file.Delete();
            }
        }
        foreach (var subDirectory in directoryInfo.GetDirectories()) {
            subDirectory.Delete(true);
        }
        Debug.Log($"Cleanup complete, only {sfxFileName} remains in the build directory.");
    }

    static string GetOutputName(bool allScenes, string productName) {
        return allScenes ? productName : GetSceneName();
    }

    static string GetSceneName() {
        return SceneManager.GetActiveScene().name;
    }
}