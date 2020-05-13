using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.UI
{
    public class UI_MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private RectTransform levelListView;

        private GameManager gManager;

        private void Awake()
        {
            gManager = GameManager.Instance;
        }

        private void Start()
        {
            List<LevelDescriptionModel> allLevels = gManager.LevelsManager.GetAllLevels();
            foreach(LevelDescriptionModel level in allLevels)
            {
                Debug.Log("!!! " + GameManager.Instance.UiFactory);
                UI_LevelButton button = GameManager.Instance.UiFactory.CreateLevelButton(level);
                button.transform.parent = levelListView;
            }
        }

    }
}