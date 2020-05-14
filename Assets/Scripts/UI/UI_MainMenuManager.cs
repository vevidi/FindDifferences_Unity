using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.UI
{
    public class UI_MainMenuManager : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private RectTransform levelListView;
#pragma warning restore 0649

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
                UI_LevelButton button = GameManager.Instance.UiFactory.CreateLevelButton(level);
                button.transform.SetParent(levelListView,false);
            }
        }

    }
}