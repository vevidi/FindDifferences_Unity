using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OpenScenes
{
    private const string SC_PREFIX = "Assets/Scenes/";
    private const string SC_SUFFIX = ".unity";

    [MenuItem("Custom/Run&Stop from preloader")]
    static void RunStop()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("PLAYING");
            EditorApplication.isPlaying = false;// ExecuteMenuItem("Edit/Play");
        }
        else
        {
            var newSettings = new EditorBuildSettingsScene[3];
            var sceneToAdd = new EditorBuildSettingsScene(SC_PREFIX + GameVariables.PreloaderScene + SC_SUFFIX, true);
            newSettings[0] = sceneToAdd;
            sceneToAdd = new EditorBuildSettingsScene(SC_PREFIX + GameVariables.MainMenuScene + SC_SUFFIX, true);
            newSettings[1] = sceneToAdd;
            sceneToAdd = new EditorBuildSettingsScene(SC_PREFIX + GameVariables.LevelScene + SC_SUFFIX, true);
            newSettings[2] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;

            OpenPreloader();
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
    }

    [MenuItem("Custom/Open Preloader scene")]
    static void OpenPreloader()
    {
        EditorSceneManager.OpenScene(SC_PREFIX + GameVariables.PreloaderScene + SC_SUFFIX);
    }

    [MenuItem("Custom/Open Main Menu scene")]
    static void OpenMainMenu()
    {
        EditorSceneManager.OpenScene(SC_PREFIX + GameVariables.MainMenuScene + SC_SUFFIX);
    }

    [MenuItem("Custom/Open Level scene")]
    static void OpenLevel()
    {
        EditorSceneManager.OpenScene(SC_PREFIX + GameVariables.LevelScene + SC_SUFFIX);
    }
}