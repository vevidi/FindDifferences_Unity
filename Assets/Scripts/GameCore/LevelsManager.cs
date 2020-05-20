using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class LevelsManager
    {
        private List<LevelDescriptionModel> allLevels;
        private int selectedLevel = 0;

        public LevelsManager()
        {
            allLevels = new List<LevelDescriptionModel>();
        }

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
            SelectLevel(0);
            GameManager.Instance.SaveManager.SaveGame(allLevels, model.Version);
            Utils.DebugLog("->>>> InitFromLevelsModel");

            GameManager.Instance.gameEventSystem.Subscribe<NextLevelCommand>(OnSelectLevel);
        }

        public void InitFromLevelsModelAndSave(LevelsModel lModel, GameSaveModel sModel)
        {
            InitFromLevelsModel(lModel);
            for (int i = 0; i < allLevels.Count; ++i)
            {
                var savedLevel = sModel.GetLevel(allLevels[i].Id);
                allLevels[i].IsEnded = savedLevel != null ? savedLevel.IsEnded : false;
                allLevels[i].IsOpened = savedLevel != null ? savedLevel.IsOpened : false;
            }
            // resave with new info
            SelectLevel(sModel.SelectedLevel);
            GameManager.Instance.SaveManager.SaveGame(allLevels, lModel.Version, sModel.SelectedLevel);
            Utils.DebugLog("->>>> InitFromLevelsModelAndSave");
        }

        public void InitFromLevelsSave(GameSaveModel model)
        {
            allLevels = model.Levels;
            foreach (var level in allLevels)
                level.LoadImage();
            SelectLevel(model.SelectedLevel);
            Utils.DebugLog("->>>> InitFromLevelsSave");
        }

        public List<LevelDescriptionModel> GetAllLevels()
        {
            return allLevels;
        }

        private void OnSelectLevel(NextLevelCommand command)
        {
            SelectLevel(command.LevelID);
        }

        public void SelectLevel(int id)
        {
            selectedLevel = id;
            if (selectedLevel > allLevels.Count - 1)
                selectedLevel = allLevels.Count - 1;
        }

        public int GetSelectedLevel()
        {
            if (selectedLevel == -1)
                Utils.DebugLog("LevelsManager -> Level NOT selected!",eLogType.Error);
            return selectedLevel;
        }

        public void EndLevel(int levelID)
        {
            var currentLevel = GetLevelByID(levelID);
            currentLevel.IsEnded = true;
            var nextLevel = GetLevelByID(levelID + 1);
            if (nextLevel != null)
                nextLevel.IsOpened = true;
            GameManager.Instance.SaveManager.SaveGame(allLevels, -1, levelID + 1);
        }

        public LevelDescriptionModel GetLevelByID(int id)
        {
            return allLevels.Find((l) => l.Id == id);
        }
    }
}