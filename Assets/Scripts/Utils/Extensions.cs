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

        public static Texture2D ToTexture2D(this RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
    }
}
