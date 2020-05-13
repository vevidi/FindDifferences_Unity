using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.FindDiff.Model
{
    [Serializable]
    public class LevelInfoModel
    {
        [SerializeField]
        int id;
        [SerializeField]
        string img;
        [SerializeField]
        private List<DifferenceInfoModel> differences;

        public List<DifferenceInfoModel> Differences { get => differences; }
        public int Id { get => id; set => id = value; }
        public string Img { get => img; set => img = value; }

        public override string ToString()
        {
            return String.Join(" : ", differences);
        }
    }
}
