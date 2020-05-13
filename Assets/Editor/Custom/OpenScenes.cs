using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OpenScenes
{
    [MenuItem("Custom/Run from preloader")]
    static void Run()
    {
        var newSettings = new EditorBuildSettingsScene[2];
        var sceneToAdd = new EditorBuildSettingsScene("Assets/Scenes/Preloader.unity", true);
        newSettings[0] = sceneToAdd;
        sceneToAdd = new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true);
        newSettings[1] = sceneToAdd;
        EditorBuildSettings.scenes = newSettings;

        OpenPreloader();
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    [MenuItem("Custom/Open Preloader scene")]
    static void OpenPreloader()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Preloader.unity");
    }

    [MenuItem("Custom/Open Main Menu scene")]
    static void OpenPhoneMob()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }
}