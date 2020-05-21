using System;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameModel
{
    [Serializable]
    public class GameSaveModel : IJsonEncodable
    {
#pragma warning disable 0649
        [SerializeField]
        private List<LevelDescriptionModel> levels;
        [SerializeField]
        private int version;
        [SerializeField]
        private int selectedLevel;
#pragma warning restore 0649

        public int Version { get => version; set => version = value; }
        public List<LevelDescriptionModel> Levels { get => levels; set => levels = value; }
        public int SelectedLevel { get => selectedLevel; set => selectedLevel = value; }

        public GameSaveModel()
        {
            levels = new List<LevelDescriptionModel>();
            version = 0;
            selectedLevel = 0;
        }

        public GameSaveModel(List<LevelDescriptionModel> levels, int version, int selectedLevel = 0)
        {
            this.levels = levels;
            this.version = version;
            this.selectedLevel = selectedLevel;
        }

        public LevelDescriptionModel GetLevel(int id) => levels.Find((l) => { return l.LevelInfo.Id == id; });

        public void Decode(string json) => JsonUtility.FromJsonOverwrite(json, this);

        public string Encode() => JsonUtility.ToJson(this);
    }
}