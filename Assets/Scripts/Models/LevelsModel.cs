using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.Model
{
    [Serializable]
    public class LevelsModel : IJsonEncodable
    {
        public List<LevelInfoModel> levels;

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
