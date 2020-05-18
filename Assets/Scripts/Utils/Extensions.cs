using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vevidi.FindDiff.GameUtils
{
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static bool IsMore(this Vector3 item1, Vector3 item2)
        {
            return (item1.magnitude > item2.magnitude);
        }
    }
}
