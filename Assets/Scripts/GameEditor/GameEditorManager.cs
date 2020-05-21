using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameEditor
{
    [ExecuteInEditMode]
    public class GameEditorManager : MonoBehaviour
    {
#pragma warning disable 0649

        [SerializeField]
        private DifferenceArea clickableAreaPrefab;

        [SerializeField]
        private RectTransform playFieldArea;

#pragma warning restore 0649

        private const string PATH = "Assets/GameResources/Sprites/GameEditor/";

        private Image playFieldBg;
        private int maxLevelId = -1;
        private int selectedLevelId = -1;
        //private int selectedAreaId = -1;
        private List<DifferenceArea> areas;

        public bool IsJsonOpened { get; set; } = false;
        public bool IsLevelOpened { get; set; } = false;
        public LevelInfoModel SelectedLevel { get; private set; }
        public int SelectedAreaId { get; set; }

        //TODO: refactor
        [HideInInspector]
        public LevelsModel levelsModel;

        //END TODO

        private void Awake()
        {
            if (playFieldBg == null)
                playFieldBg = playFieldArea.GetComponent<Image>();
            areas = new List<DifferenceArea>();
            levelsModel = new LevelsModel();
        }

        private void UpdateMaxLevelId()
        {
            //levelsModel.Levels.ForEach((l)=> { if (maxLevelID < l.Id) maxLevelID = l.Id; });
            maxLevelId = levelsModel.Levels.Count - 1;
        }

        public void LoadAllLevels(string path)
        {
            levelsModel = new LevelsModel(SaveLoadUtility.LoadTextFile(path));
            UpdateMaxLevelId();
        }

        public void SaveAllLevels(string path)
        {
            SaveLoadUtility.SaveTextFile(path, levelsModel.Encode());
        }

        public void CreateLevel()
        {
            levelsModel.Levels.Add(new LevelInfoModel(maxLevelId + 1));
            ++maxLevelId;
            EditorUtility.SetDirty(this);
        }

        public void RemoveLevel(int levelId)
        {
            if (levelId <= levelsModel.Levels.Count - 1)
            {
                var level = levelsModel.Levels.Find((l) => l.Id == levelId);
                levelsModel.Levels.Remove(level);
                for (int i = 0; i < levelsModel.Levels.Count; ++i)
                    levelsModel.Levels[i].Id = i;
                UpdateMaxLevelId();
            }
        }

        private void ClearAreas()
        {
            foreach (var area in areas)
                if (area != null && area.gameObject != null)
                    DestroyImmediate(area.gameObject);
            areas.Clear();
        }

        public void OpenLevel(int levelId)
        {
            SelectedAreaId = -1;
            if (areas != null && areas.Count > 0)
                ClearAreas();

            SelectedLevel = levelsModel.Levels.Find((l) => l.Id == levelId);
            if (SelectedLevel != null)
            {
                string imageName = SelectedLevel.Image;
                var image = AssetDatabase.LoadAssetAtPath(PATH + imageName, typeof(Sprite)) as Sprite;
                if (image != null)
                {
                    playFieldBg.overrideSprite = image;
                    EditorUtility.SetDirty(playFieldBg);
                    IsLevelOpened = true;
                    selectedLevelId = levelId;

                    var differences = SelectedLevel.Differences;
                    foreach (var difference in differences)
                        CreateAreasPair(difference);
                }
                else
                    Debug.LogWarning("Cannot find image at path: " + PATH + imageName);
            }
            else
                Debug.LogWarning("Cannot find image for level: " + levelId);
        }

        private void CreateAreasPair(DifferenceInfoModel difference)
        {
            var areaOne = CreateClickArea(difference, false, -(int)playFieldBg.rectTransform.rect.width / 2);
            var areaTwo = CreateClickArea(difference);
            areaOne.SetPair(areaTwo);
        }

        public void SaveLevel()
        {
            IsLevelOpened = false;
            playFieldBg.overrideSprite = null;
            if (selectedLevelId != -1)
            {
                var differences = levelsModel.Levels[selectedLevelId].Differences;
                differences.Clear();
                foreach (var area in areas)
                {
                    if (area.IsSelectable)
                    {
                        (int id, float x, float y, float r) = area.GetInfo();
                        DifferenceInfoModel model = new DifferenceInfoModel(id, x, y, r);
                        differences.Add(model);
                    }
                }
            }
            else
                Debug.LogError("Something wrong in save level! Incorrect level ID");
            ClearAreas();
            SelectedLevel = null;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        public void CloseLevel()
        {
            IsLevelOpened = false;
            playFieldBg.overrideSprite = null;
            ClearAreas();
            SelectedLevel = null;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        }

        private DifferenceArea CreateClickArea(DifferenceInfoModel model, bool hided = true, int offsetX = 0, int offsetY = 0)
        {
            DifferenceArea area = Instantiate(clickableAreaPrefab);
            area.transform.SetParent(playFieldArea);
            if (hided)
            {
                //area.gameObject.hideFlags = HideFlags.HideInHierarchy;
                area.gameObject.layer = LayerMask.NameToLayer("NotPickable");
                area.IsSelectable = false;
            }
            else
                area.IsSelectable = true;
            area.Init(model, this, offsetX, offsetY);
            areas.Add(area);
            return area;
        }

        public void SelectClickArea(int areaId)
        {
            var currArea = areas.Find((ar) => ar.Id == areaId);
            if (currArea != null)
            {
                SelectedAreaId = areaId;
                currArea.SetSelected();
                Selection.SetActiveObjectWithContext(currArea.gameObject, null);
            }
            else
                Debug.LogWarning("Something wrong in select area! Cannot find area: " + areaId);
        }

        public void DeleteClickArea()
        {
            if (SelectedAreaId >= 0)
            {
                var currAreas = areas.FindAll((ar) => ar.Id == SelectedAreaId);
                foreach (var area in currAreas)
                {
                    area.SetSelected(false);
                    DestroyImmediate(area.gameObject);
                }
                areas.RemoveAll((ar) => ar.Id == SelectedAreaId);
                var differences = levelsModel.Levels[selectedLevelId].Differences;
                var currDiff = differences.Find((d) => d.Id == SelectedAreaId);
                differences.Remove(currDiff);
                SelectedAreaId = -1;

                int counter = 0;
                for (int i = 0; i < areas.Count; ++i)
                {
                    if (areas[i].IsSelectable)
                    {
                        ++counter;
                        differences[counter - 1].Id = counter;
                        areas[i].Id = counter;
                    }
                }
            }
            else
                Debug.LogWarning("Something wrong in delete area!");
        }

        public void CreateClickArea()
        {
            var differences = levelsModel.Levels[selectedLevelId].Differences;
            DifferenceInfoModel model = new DifferenceInfoModel(differences.Count + 1, 0, 0, 20);
            differences.Add(model);
            CreateAreasPair(model);
            SelectClickArea(differences.Count);
        }
    }
}