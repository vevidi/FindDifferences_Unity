// some useful functions class

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vevidi.FindDiff.GameUtils
{
    public enum eLogType
    {
        Log,
        Warning,
        Error
    }

    public static class Utils
    {
        public struct SpritePivot
        {
            public static Vector2 BottomLeft = Vector2.zero;
            public static Vector2 TopRight = Vector2.one;
            public static Vector2 Center = Vector2.one * 0.5f;
            public static Vector2 TopLeft = new Vector2(0, 1);
            public static Vector2 BottomRight = new Vector2(1, 0);
            public static Vector2 CenterLeft = new Vector2(0, 0.5f);
            public static Vector2 CenterRight = new Vector2(1, 0.5f);
            public static Vector2 CenterTop = new Vector2(0.5f,1);
            public static Vector2 CenterBottom = new Vector2(0.5f, 0);
        }

        public static Sprite GetSpriteFromTex2D(Texture2D texture) => GetSpriteFromTex2D(texture, SpritePivot.Center);

        public static Sprite GetSpriteFromTex2D(Texture2D texture, Rect rect) => Sprite.Create(texture, rect, SpritePivot.Center);

        public static Sprite GetSpriteFromTex2D(Texture2D texture, Vector2 pivot)
        {
            Rect newRect = new Rect(0, 0, texture.width, texture.height);
            return Sprite.Create(texture, newRect, pivot);
        }

        public static string GetTimeAsStringFormatted(float time)
        {
            TimeSpan ts = TimeSpan.FromSeconds(time);
            if (time > 99 * 24 * 60 * 60)
                return "error";
            else if(time>=24*60*60)
                return ts.ToString(@"dd\:hh\:mm\:ss");
            else if(time>=60*60)
                return ts.ToString(@"hh\:mm\:ss");
            else
                return ts.ToString(@"mm\:ss");            
        }

        public static float GetAspectRatio() => (float)Screen.width / Screen.height;

        public static void PrintDictionary<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            foreach (KeyValuePair<T1, T2> kvp in dictionary)
                DebugLog("Dict: " + string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }

        public static void ClearLog()
        {
#if UNITY_EDITOR
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
#endif
        }

        public static void DebugLog(object message, eLogType logType = eLogType.Log)
        {
#if UNITY_EDITOR
            switch(logType)
            {
                case eLogType.Log:
                    Debug.Log(message); break;
                case eLogType.Warning:
                    Debug.LogWarning(message); break;
                case eLogType.Error:
                    Debug.LogError(message); break;
            }
#endif
        }

        public static bool IsMobile() => Application.isMobilePlatform;

        public static string GetHash(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return Convert.ToBase64String(hash);
        }
    }
}
