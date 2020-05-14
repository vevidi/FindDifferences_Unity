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
                Debug.LogWarning("ZZZZ " + lDescrModel.LevelImage);
                allLevels.Add(lDescrModel);
            }
        }

        public void InitFromLevelsSave(GameSaveModel model)
        {

        }

        public List<LevelDescriptionModel> GetAllLevels()
        {
            return allLevels;
        }

        public LevelDescriptionModel GetLevelByID(int id)
        {
            if (id >= 0 && id < allLevels.Count)
                return allLevels[id];
            return null;
        }
    }
}