using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.UI;

namespace Vevidi.FindDiff.Factories
{
    public class UIFactory : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private UI_LevelButton levelButtonPrefab;
#pragma warning restore 0649

        private T CreateItem<T>(T objToClone) where T : UnityEngine.Object
        {
            var rez = Instantiate(objToClone);
            return rez;
        }

        public UI_LevelButton CreateLevelButton(LevelDescriptionModel model)
        {
            UI_LevelButton button = CreateItem<UI_LevelButton>(levelButtonPrefab);
            button.Init(model);
            return button;
        }
    }
}