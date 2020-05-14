using System;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using static Vevidi.FindDiff.GameLogic.SoundsManager;
using static Vevidi.FindDiff.UI.UI_WindowsManager;

namespace Vevidi.FindDiff.UI
{
    public class UI_BasePopup : MonoBehaviour, IBasePopup
    {
        protected bool isBackBtnClosable = false;
        protected eWindowType WType;
        protected Action onCloseCallback;

        public bool IsBackBtnClosable { get { return isBackBtnClosable; } }

        public virtual void Init(UI_WindowConfig cfg)
        {
            WType = cfg.WType;
        }

        public virtual void OnCloseButtonClick()
        {
            SoundsManager.Instance.PlaySound(eSoundType.Click);
            UI_WindowsManager.Instance.HideWindow(WType);
        }

        public virtual void OnOKButtonClick()
        {
            SoundsManager.Instance.PlaySound(eSoundType.Click);
        }
    }
}