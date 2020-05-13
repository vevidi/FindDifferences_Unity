using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.Model;

namespace Vevidi.FindDiff.UI
{
    public class PreloaderManager : MonoBehaviour
    {
        [SerializeField]
        private Button testButton;

        private void Start()
        {
            testButton.onClick.AddListener(TestClick);
        }

        private void OnDestroy()
        {
            testButton.onClick.RemoveListener(TestClick);
        }

        private void TestClick()
        {
            LevelsModel testModel = new LevelsModel();
            LevelInfoModel l1 = new LevelInfoModel();
            l1.Id = 1;
            l1.Img = "test.png";
            l1.Differences.Add(new DifferenceInfoModel(1, 1, 1));
            l1.Differences.Add(new DifferenceInfoModel(2, 2, 2));
            l1.Differences.Add(new DifferenceInfoModel(3, 3, 3));
            testModel.levels.Add(new LevelInfoModel());
                            LevelInfoModel l2 = new LevelInfoModel();
            l2.Id = 2;
            l2.Img = "test_2.png";
            l2.Differences.Add(new DifferenceInfoModel(11, 11, 11));
            l2.Differences.Add(new DifferenceInfoModel(22, 22, 22));
            l2.Differences.Add(new DifferenceInfoModel(33, 33, 33));
            testModel.levels.Add(new LevelInfoModel());

            Debug.Log("Result: ->> " + testModel.Encode());
        }
    }
}
