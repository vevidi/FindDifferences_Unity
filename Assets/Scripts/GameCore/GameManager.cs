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

        public void InitLevelsManager(LevelsModel model)
        {
            LevelsManager = new LevelsManager();
            LevelsManager.InitFromLevelsModel(model);
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
        }

        private void Start()
        {

        }
    }
}
