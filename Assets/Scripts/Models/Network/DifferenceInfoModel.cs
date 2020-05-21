using System;
using UnityEngine;

namespace Vevidi.FindDiff.NetworkModel
{
    [Serializable]
    public class DifferenceInfoModel : ICloneable
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private float x;
        [SerializeField]
        private float y;
        [SerializeField]
        private float r;

        public DifferenceInfoModel(int id, float x, float y, float r)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.r = r;
        }

        public float X { get => x; }
        public float Y { get => y; }
        public float Radius { get => r; set => r = value; }
        public int Id { get => id; set => id = value; }

        public object Clone() => MemberwiseClone();
 
        public override string ToString() => "ID:" + id + " X: " + x + " Y: " + y + " R: " + r;
    }
}
