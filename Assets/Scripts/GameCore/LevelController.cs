using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class LevelController : MonoBehaviour
    {
        private LevelDescriptionModel levelInfo;
        private LevelsManager lManager;

        private void Awake()
        {
            lManager = GameManager.Instance.LevelsManager;
            levelInfo = lManager.GetLevelByID(lManager.GetSelectedLevel());

            Debug.Log("Level controller -> Loaded level: " + levelInfo);
        }

        private void Start()
        {

        }

    }
}