using System;
using UnityEngine;

namespace Vevidi.FindDiff.Model
{
    [Serializable]
    public class DifferenceInfoModel
    {
        [SerializeField]
        private float x;
        [SerializeField]
        private float y;
        [SerializeField]
        private float r;

        public DifferenceInfoModel(float x, float y, float r)
        {
            this.x = x;
            this.y = y;
            this.r = r;
        }

        public float X { get => x; }
        public float Y { get => y; }
        public float Radius { get => r; }

        public override string ToString()
        {
            return "X: " + x + " Y: " + y + " R: " + r;
        }
    }
}
