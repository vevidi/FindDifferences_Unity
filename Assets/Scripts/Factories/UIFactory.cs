using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.UI;
using Vevidi.Experimental;

namespace Vevidi.FindDiff.Factories
{
    public class UIFactory : BaseFactory
    {
#pragma warning disable 0649
        [SerializeField]
        private UI_LevelButton levelButtonPrefab;
        [SerializeField]
        private ScrollView3DItem levelButton3DPrefab;
#pragma warning restore 0649

        public UI_LevelButton CreateLevelButton(LevelDescriptionModel model)
        {
            UI_LevelButton button = CreateItem(levelButtonPrefab);
            button.Init(model);
            return button;
        }

        public ScrollView3DItem CreateLevelButton3D(LevelDescriptionModel model)
        {
            ScrollView3DItem button = CreateItem(levelButton3DPrefab);
            button.Init(model);
            return button;
        }
    }
}