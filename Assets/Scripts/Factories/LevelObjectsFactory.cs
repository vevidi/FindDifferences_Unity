using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.Factories
{
    public class LevelObjectsFactory : BaseFactory
    {
#pragma warning disable 0649
        [SerializeField]
        private TouchableArea touchableAreaPrefab;
        [SerializeField]
        private RectTransform checkMarkPrefab;
        [SerializeField]
        private MissTapCheckmark missTapMarkPrefab;
#pragma warning restore 0649

        public TouchableArea CreateTouchableArea(RectTransform parent, DifferenceInfoModel model, int offsetX = 0, int offsetY = 0)
        {
            TouchableArea area = CreateItem(touchableAreaPrefab);
            area.transform.SetParent(parent);
            area.Init(model, offsetX, offsetY);
            area.gameObject.name = "Area " + model.Id;
            return area;
        }

        public RectTransform CreateCheckmark(RectTransform parent)
        {
            RectTransform checkmark = CreateItem(checkMarkPrefab);
            checkmark.SetParent(parent);
            checkmark.localScale = Vector3.one;
            return checkmark;
        }

        public MissTapCheckmark CreateMissTapCheckmark(RectTransform parent) => CreateItem(missTapMarkPrefab, parent);
    }
}