using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.GameLogic
{
    public class LevelController : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private Image backgroundImage;
#pragma warning restore 0649

        private LevelDescriptionModel levelInfo;
        private LevelsManager lManager;

        private void Awake()
        {
            lManager = GameManager.Instance.LevelsManager;
            levelInfo = lManager.GetLevelByID(lManager.GetSelectedLevel());

            Debug.Log("Level controller -> Loaded level: " + levelInfo);
        }

        private void InitLevel()
        {            
            backgroundImage.overrideSprite = Utils.GetSpriteFromTex2D(levelInfo.LevelImage);
        }

        private void Start()
        {
            InitLevel();
        }

    }
}