using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Vevidi.FindDiff.GameEditor
{
//    [ExecuteInEditMode]
    [InitializeOnLoad]
    [CustomEditor(typeof(GameEditorManager))]
    public class GameEditorManagerEditor : Editor
    {
        private GameEditorManager manager;

        private GameEditorManagerEditor()
        {
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void DrawLevelList()
        {
            GUILayout.Space(20);
            GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
            if (GUILayout.Button("Open levels JSON", GUILayout.Height(30)))
                OpenLevelsJsonClick();
            GUI.backgroundColor = Color.white;
            GUILayout.Space(20);

            if (manager.IsJsonOpened && manager.levelsModel.Levels.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Current version:", GUILayout.Width(162));
                manager.levelsModel.Version = Convert.ToInt32(EditorGUILayout.TextField(manager.levelsModel.Version.ToString(), GUILayout.Width(100)));
                EditorGUILayout.EndHorizontal();
                //EditorGUILayout.BeginHorizontal();
                var levels = manager.levelsModel.Levels;
                for (int i = 0; i < levels.Count; ++i) //var level in levels.Levels)
                {
                    var level = levels[i];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("ID: " + level.Id, GUILayout.Width(60));
                    EditorGUILayout.LabelField("Image name:", GUILayout.Width(100));
                    //GUILayout.Space(20);
                    level.Image = EditorGUILayout.TextField(level.Image, GUILayout.Width(100)); //, GUILayout.MaxWidth(250));
                    GUILayout.Space(20);
                    GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
                    if (GUILayout.Button("Open"))
                        manager.OpenLevel(level.Id);
                    GUI.backgroundColor = Color.white;
                    GUILayout.Space(20);
                    GUI.backgroundColor = new Color(222 / 255f, 62 / 255f, 62 / 255f);
                    if (GUILayout.Button("Remove"))
                        manager.RemoveLevel(level.Id);
                    GUI.backgroundColor = Color.white;
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.Space(20);
                GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
                if (GUILayout.Button("Add new level", GUILayout.Height(30)))
                    manager.CreateLevel();

                GUILayout.Space(20);
                GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
                if (GUILayout.Button("Save to JSON", GUILayout.Height(30)))
                    SaveLevelsJsonClick();
                GUI.backgroundColor = Color.white;
            }
        }

        private void DrawDifferencesList()
        {
            var sLevel = manager.SelectedLevel;
            if (sLevel != null)
            {
                var differences = sLevel.Differences;
                foreach (var difference in differences)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("ID: " + difference.Id, GUILayout.Width(60));
                    GUILayout.Space(20);
                    GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
                    if (GUILayout.Button("Select"))
                        manager.SelectClickArea(difference.Id);
                    GUI.backgroundColor = Color.white;
                    GUILayout.EndHorizontal();
                }
                //CreateClickArea
                GUILayout.Space(20);
                GUI.backgroundColor = new Color(52 / 255f, 145 / 255f, 55 / 255f);
                if (GUILayout.Button("Add new"))
                    manager.CreateClickArea();
                GUILayout.Space(40);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save level"))
                    manager.SaveLevel();
                GUILayout.Space(10);
                GUI.backgroundColor = new Color(222 / 255f, 62 / 255f, 62 / 255f);
                if (GUILayout.Button("Close level"))
                    manager.CloseLevel();
                GUI.backgroundColor = Color.white;
                GUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Go back"))
                    manager.CloseLevel();
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (manager == null)
                manager = target as GameEditorManager;

            if (!manager.IsLevelOpened)
                DrawLevelList();
            else
                DrawDifferencesList();
        }

        private void OpenLevelsJsonClick()
        {
            string path = EditorUtility.OpenFilePanel("Select level json", "", "txt,json");
            if (!string.IsNullOrEmpty(path))
            {
                manager.LoadAllLevels(path);
                manager.IsJsonOpened = true;
            }
        }

        private void SaveLevelsJsonClick()
        {
            string path = EditorUtility.SaveFilePanel("Save level json", "", "", "txt,json");
            if (!string.IsNullOrEmpty(path))
            {
                manager.SaveAllLevels(path);
                manager.IsJsonOpened = false;
            }
        }

        private void OnSelectionChanged()
        {
            Transform activeTrans = Selection.activeTransform;
            if (activeTrans != null && activeTrans.tag == GameVariables.DiffAreaTag)
            {
                DifferenceArea dArea = activeTrans.GetComponent<DifferenceArea>();
                if (dArea != null)
                {
                    manager.SelectedAreaId = dArea.Id;
                    dArea.SetSelected();
                }
            }
        }
    }
}