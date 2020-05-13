using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameModel
{
    [Serializable]
    public class GameSaveModel : IJsonEncodable
    {
        [SerializeField]
        private List<LevelDescriptionModel> levels;

        public void Decode(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public string Encode()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
