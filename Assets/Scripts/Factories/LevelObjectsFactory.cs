using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.Factories
{
    public class LevelObjectsFactory : BaseFactory
    {
#pragma warning disable 0649
        [SerializeField]
        private TouchableArea touchableAreaPrefab;
#pragma warning restore 0649

        public TouchableArea CreateTouchableArea(RectTransform parent, DifferenceInfoModel model, int offsetX = 0, int offsetY = 0)
        {
            TouchableArea area = CreateItem(touchableAreaPrefab);
            area.transform.SetParent(parent);
            area.Init(model, offsetX, offsetY);
            return area;
        }
    }
}