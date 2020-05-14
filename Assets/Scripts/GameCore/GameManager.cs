using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.Factories;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class GameManager : SingletonBase<GameManager>
    {
        public static GameManager Instance
        {
            get
            {
                if (isShuttingDown)
                {
                    Debug.LogWarning("Game manager is shutting down");
                    return null;
                }
                return instance;
            }
        }

        public LevelsManager LevelsManager { get; private set; }
        public UIFactory UiFactory { get; private set; }
        public SaveManager SaveManager { get; private set; }

        public void InitLevelsManager(LevelsModel model)
        {
            LevelsManager = new LevelsManager();
            if(!SaveManager.SaveLoaded)
            {
                LevelsManager.InitFromLevelsModel(model);
            }
            else if(SaveManager.SaveVersion != model.Version)
            {
                LevelsManager.InitFromLevelsModelAndSave(model, SaveManager.GameSave);
            }
            else
            {
                LevelsManager.InitFromLevelsSave(SaveManager.GameSave);
            }

            //if (SaveManager.SaveLoaded && SaveManager.SaveVersion == model.Version)
            //    LevelsManager.InitFromLevelsSave(SaveManager.GameSave);
            //else
            //    LevelsManager.InitFromLevelsModel(model);
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            UiFactory = GetComponent<UIFactory>();
            SaveManager = new SaveManager();
            SaveManager.LoadSave();
        }

        private void OnDestroy()
        {
            GameSaveModel saveGame = new GameSaveModel(LevelsManager.GetAllLevels(), SaveManager.SaveVersion);
            SaveManager.SaveGame(saveGame);
        }
    }
}
