using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class LevelsManager
    {
        private List<LevelDescriptionModel> allLevels;
        private int selectedLevel = -1;

        public LevelsManager()
        {
            allLevels = new List<LevelDescriptionModel>();
        }

        // TODO: add save check to update opened and ended levels
        public void InitFromLevelsModel(LevelsModel model)
        {
            var levels = model.Levels;
            for (int i = 0; i < levels.Count; ++i)
            {
                var lDescrModel = new LevelDescriptionModel(levels[i], i == 0 /*|| i == 1*/);
                lDescrModel.LoadImage();
                allLevels.Add(lDescrModel);
            }
            // save loaded info
            GameManager.Instance.SaveManager.SaveGame(allLevels, model.Version);
            Debug.LogWarning("->>>> InitFromLevelsModel");
        }

        public void InitFromLevelsModelAndSave(LevelsModel lModel, GameSaveModel sModel)
        {
            InitFromLevelsModel(lModel);
            for (int i = 0; i < allLevels.Count; ++i)
            {
                var savedLevel = sModel.GetLevel(allLevels[i].Id);
                allLevels[i].IsEnded = savedLevel.IsEnded;
                allLevels[i].IsOpened = savedLevel.IsOpened;
            }
            // resave with new info
            GameManager.Instance.SaveManager.SaveGame(allLevels, lModel.Version);
            Debug.LogWarning("->>>> InitFromLevelsModelAndSave");
        }

        public void InitFromLevelsSave(GameSaveModel model)
        {
            allLevels = model.Levels;
            foreach (var level in allLevels)
                level.LoadImage();
            Debug.LogWarning("->>>> InitFromLevelsSave");
        }

        public List<LevelDescriptionModel> GetAllLevels()
        {
            return allLevels;
        }

        public void SelectLevel(int id)
        {
            selectedLevel = id;
        }

        public int GetSelectedLevel()
        {
            if (selectedLevel == -1)
                Debug.LogError("LevelsManager -> Level NOT selected!");
            return selectedLevel;
        }

        public void EndLevel(int levelID)
        {
            var currentLevel = GetLevelByID(levelID);
            currentLevel.IsEnded = true;
            var nextLevel = GetLevelByID(levelID + 1);
            if (nextLevel != null)
                nextLevel.IsOpened = true;
            GameManager.Instance.SaveManager.SaveGame(allLevels);
        }

        public LevelDescriptionModel GetLevelByID(int id)
        {
            return allLevels.Find((l) => l.Id == id);
        }
    }
}