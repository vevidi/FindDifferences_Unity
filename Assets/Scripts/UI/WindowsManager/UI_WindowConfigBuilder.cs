using System;
using UnityEngine;
using static Vevidi.FindDiff.UI.UI_WindowsManager;

namespace Vevidi.FindDiff.UI
{
    public class UI_WindowConfigBuilder
    {
        private UI_WindowConfig currConfig;

        public UI_WindowConfigBuilder(eWindowType wType)
        {
            currConfig = new UI_WindowConfig(wType);
        }

        public UI_WindowConfigBuilder AddData(string key, object value)
        {
            currConfig.AddWindowData(key, value);
            return this;
        }

        public UI_WindowConfigBuilder AddCallback(string key, Action value)
        {
            currConfig.AddWindowCallback(key, value);
            return this;
        }

        public UI_WindowConfig Build()
        {
            return currConfig;
        }
    }
}
