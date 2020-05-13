﻿using System;
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
#pragma warning restore 0649

        public LevelsModel()
        {
            levels = new List<LevelInfoModel>();
        }

        public LevelsModel( string json) : this()
        {
            Decode(json);
        }

        public List<LevelInfoModel> Levels { get => levels; set => levels = value; }

        public void Decode(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public string Encode()
        {
            //foreach (var v in levels)
            //    Debug.Log(v);
            return JsonUtility.ToJson(this);
        }

        public override string ToString()
        {
            return String.Join("\n",levels);
        }
    }
}
