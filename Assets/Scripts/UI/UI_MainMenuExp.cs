using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.UI
{
    public class UI_MainMenuExp : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private ScrollView3D levelListView;
#pragma warning restore 0649

        private GameManager gManager;

        private void Awake() => gManager = GameManager.Instance;

        private void Start()
        {
            List<LevelDescriptionModel> allLevels = gManager.LevelsManager.GetAllLevels();
            List<Transform> listItems = new List<Transform>();
            foreach (LevelDescriptionModel level in allLevels)
            {
                ScrollView3DItem button = gManager.UiFactory.CreateLevelButton3D(level);
                listItems.Add(button.transform);
            }
            levelListView.Initialize(listItems, gManager.LevelsManager.GetSelectedLevel());
        }
    }
}