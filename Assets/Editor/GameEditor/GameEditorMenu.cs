using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Vevidi.FindDiff.GameEditor
{
    public static class GameEditorMenu
    {
        private const string SC_PREFIX = "Assets/Scenes/GameEditor/";
        private const string SC_SUFFIX = ".unity";

        [MenuItem("Game Editor/Open game editor")]
        static void OpenGameEditor()
        {
            EditorSceneManager.OpenScene(SC_PREFIX + GameVariables.EditorScene + SC_SUFFIX);
            EditorApplication.ExecuteMenuItem("Window/General/Scene");
            GameObject editorManager = GameObject.Find("EditorManager");
            Selection.SetActiveObjectWithContext(editorManager, null);
        }
    }
}
