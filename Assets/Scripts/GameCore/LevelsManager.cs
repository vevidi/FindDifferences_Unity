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
            for (int i=0; i<levels.Count; ++i)
            {
                var lDescrModel = new LevelDescriptionModel(levels[i], i == 0);
                lDescrModel.LoadImage();
                allLevels.Add(lDescrModel);
            }
            // save loaded info
            GameSaveModel saveModel = new GameSaveModel(allLevels, model.Version);
            GameManager.Instance.SaveManager.SaveGame(saveModel);
            Debug.LogWarning("->>>> InitFromLevelsModel");
        }

        public void InitFromLevelsModelAndSave(LevelsModel lModel, GameSaveModel sModel)
        {
            InitFromLevelsModel(lModel);
            for (int i=0; i< allLevels.Count;++i)
            {
                var savedLevel = sModel.GetLevel(allLevels[i].Id);
                allLevels[i].IsEnded = savedLevel.IsEnded;
                allLevels[i].IsOpened = savedLevel.IsOpened;
            }
            // resave with new info
            GameSaveModel saveModel = new GameSaveModel(allLevels, lModel.Version);
            GameManager.Instance.SaveManager.SaveGame(saveModel);
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

        public LevelDescriptionModel GetLevelByID(int id)
        {
            if (id >= 0 && id < allLevels.Count)
                return allLevels[id];
            return null;
        }
    }
}