﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameUtils;
using static Vevidi.FindDiff.UI.UI_WindowsManager;

namespace Vevidi.FindDiff.UI
{
    public class UI_WindowConfig
    {
        private eWindowType wType;
        private Dictionary<string, object> wData;
        private Dictionary<string, Action> cData;

        public eWindowType WType { get { return wType; } }
        public Dictionary<string, object> WData { get { return wData; } }
        public Dictionary<string, Action> CData { get { return cData; } }

        public UI_WindowConfig(eWindowType WType) => this.wType = WType;

        public void AddWindowData(string key, object value)
        {
            if (wData == null)
                wData = new Dictionary<string, object>();
            if (!WData.ContainsKey(key))
                WData.Add(key, value);
            else
            {
                WData["key"] = value;
                Utils.DebugLog("Key " + key + " already present in window data! Rewriting.", eLogType.Warning);
            }
        }

        public void AddWindowCallback(string key, Action callback)
        {
            if (cData == null)
                cData = new Dictionary<string, Action>();
            if (!CData.ContainsKey(key))
                CData.Add(key, callback);
            else
            {
                CData["key"] = callback;
                Utils.DebugLog("Key " + key + " already present in window callbacks! Rewriting.",eLogType.Warning);
            }
        }
    }
}