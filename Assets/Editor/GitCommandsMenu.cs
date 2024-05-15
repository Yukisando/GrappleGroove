using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class GitCommandsMenu : Editor
{
    [MenuItem("Git/Add All and Commit", priority = 0)]
    static void GitAddAndCommit() {
        GitAddAll();
        GitCommit();
    }


    [MenuItem("Git/Revert to Previous Commit", priority = 2)]
    static void GitRevertToPreviousCommit() {
        if (EditorUtility.DisplayDialog("Revert to Previous Commit",
                "Are you sure you want to revert to the previous commit?",
                "Yes", "No"))
            ExecuteGitCommand("reset --hard HEAD~1");
    }

    static void GitAddAll() {
        ExecuteGitCommand("add .");
    }

    static void GitCommit() {
        CommitMessageWindow.Init();
    }

    public static void ExecuteGitCommand(string arguments) {
        var projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length);

        var process = new Process();
        process.StartInfo.FileName = "git";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.WorkingDirectory = projectPath;
        process.Start();
    }
}

// Custom window for entering the commit message
public class CommitMessageWindow : EditorWindow
{
    string commitMessage = "";
    Vector2 scrollPos;

    void OnGUI() {
        // Set up scroll view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // Adjust text area height
        var rect = EditorGUILayout.GetControlRect(GUILayout.Height(position.height - 80f));
        commitMessage = EditorGUI.TextArea(rect, commitMessage);

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Commit")) {
            if (commitMessage.Trim() != "") {
                GitCommandsMenu.ExecuteGitCommand($"commit -m \"{commitMessage}\"");
                Close();
            }
            else {
                EditorUtility.DisplayDialog("Error", "Commit message cannot be empty.", "OK");
            }
        }

        if (GUILayout.Button("Commit and Push")) {
            if (commitMessage.Trim() != "") {
                GitCommandsMenu.ExecuteGitCommand($"commit -m \"{commitMessage}\"");
                GitCommandsMenu.ExecuteGitCommand("push");
                Close();
            }
            else {
                EditorUtility.DisplayDialog("Error", "Commit message cannot be empty.", "OK");
            }
        }

        GUILayout.EndHorizontal();
    }

    public static void Init() {
        var window = GetWindow<CommitMessageWindow>("Commit Message");
        window.Show();
    }
}