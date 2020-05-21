using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.NetworkModel
{
    [Serializable]
    public class LevelsModel : IJsonEncodable
    {
#pragma warning disable 0649
        [SerializeField]
        private List<LevelInfoModel> levels;
        [SerializeField]
        private int version;
#pragma warning restore 0649

        public LevelsModel() => levels = new List<LevelInfoModel>();
        public LevelsModel( string json) : this() => Decode(json);

        public List<LevelInfoModel> Levels { get => levels; set => levels = value; }
        public int Version { get => version; set => version = value; }

        public void Decode(string json) => JsonUtility.FromJsonOverwrite(json, this);

        public string Encode() => JsonUtility.ToJson(this,true);

        public override string ToString() => string.Join("\n", levels);
    }
}
