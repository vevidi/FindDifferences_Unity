using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vevidi.FindDiff.GameEditor
{
    [ExecuteInEditMode]
    [CustomEditor(typeof(DifferenceArea))]
    public class DifferenceAreaEditor : Editor
    {
        private GameEditorManager manager;

        private void Awake()
        {
            if (manager == null)
                manager = FindObjectOfType<GameEditorManager>();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);
            GUI.backgroundColor = new Color(222 / 255f, 62 / 255f, 62 / 255f);
            if (manager.SelectedAreaId != -1 && GUILayout.Button("Remove", GUILayout.Height(30)))
            {
                manager.DeleteClickArea();
                GameObject editorManager = GameObject.Find("EditorManager");
                Selection.SetActiveObjectWithContext(editorManager, null);
            }

            GUILayout.Space(10);
            GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
            if (GUILayout.Button("Create one more", GUILayout.Height(30)))
            {
                GameObject editorManager = GameObject.Find("EditorManager");
                GameEditorManager manager = editorManager.GetComponent<GameEditorManager>();
                manager.CreateClickArea();
                var currArea = manager.GetDifferenceArea(manager.GetDifferencesCount());
                if (currArea != null)
                    Selection.SetActiveObjectWithContext(currArea.gameObject, null);
            }

            GUILayout.Space(10);
            GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
            if (GUILayout.Button("Back to editor", GUILayout.Height(30)))
            {
                GameObject editorManager = GameObject.Find("EditorManager");
                Selection.SetActiveObjectWithContext(editorManager, null);
            }
        }
    }
}