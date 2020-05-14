using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.NetworkModel;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.GameModel
{
    [Serializable]
    public class LevelDescriptionModel
    {
        [SerializeField]
        private bool isOpened;
        [SerializeField]
        private bool isEnded;
        [SerializeField]
        private LevelInfoModel levelInfo;
        private Texture2D levelImage;

        public LevelDescriptionModel() { }

        public LevelDescriptionModel(LevelInfoModel levelInfo, bool isOpened = false, bool isEnded = false)
        {
            this.isOpened = isOpened;
            this.isEnded = isEnded;
            this.levelInfo = levelInfo;
            //SaveLoadUtility.LoadImage(levelInfo.Id + ".jpg");
        }

        public LevelDescriptionModel(LevelInfoModel levelInfo, Texture2D image, bool isOpened = false, bool isEnded = false)
            :this(levelInfo,isOpened,isEnded)
        {
            levelImage = image;
        }

        public int Id { get => levelInfo.Id;}
        public List<DifferenceInfoModel> Differences { get => levelInfo.Differences; }
        public bool IsOpened { get => isOpened; set => isOpened = value; }
        public bool IsEnded { get => isEnded; set => isEnded = value; }
        public Texture2D LevelImage { get => levelImage; }
        public LevelInfoModel LevelInfo { get => levelInfo; }

        public void LoadImage()
        {
            levelImage = SaveLoadUtility.LoadImage(levelInfo.Id + ".jpg");
        }

        public override string ToString()
        {
            return "Is opened: " + isOpened + " is ended: " + isEnded + " level info:\n" + levelInfo;
        }
    }
}