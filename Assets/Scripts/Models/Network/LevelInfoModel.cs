using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.NetworkModel
{
    [Serializable]
    public class LevelInfoModel
    {
        [SerializeField]
        int id;
        [SerializeField]
        string img;
#pragma warning disable 0649
        [SerializeField]
        private List<DifferenceInfoModel> differences;
#pragma warning restore 0649

        public List<DifferenceInfoModel> Differences { get => differences; set => differences = value; }
        public int Id { get => id; set => id = value; }
        public string Image { get => img; set => img = value; }

        public override string ToString()
        {
            string result = "";
            result += id + " " + img + " \n ";
            result += String.Join("\n", differences);
            return result;
        }
    }
}
