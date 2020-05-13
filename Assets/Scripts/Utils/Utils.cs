// some useful functions class

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vevidi.FindDiff.Utils
{
    public static class Utils
    {
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

        public static float GetAspectRatio()
        {
            return (float)Screen.width / Screen.height;
        }

        public static void PrintDictionary<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            foreach (KeyValuePair<T1, T2> kvp in dictionary)
            {
                Debug.LogWarning("Dict: " + string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            }
        }

        public static bool IsMobile()
        {
            return Application.isMobilePlatform;
        }

        public static string GetHash(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            return Convert.ToBase64String(hash);
        }
    }
}
