using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vevidi.FindDiff.UI
{
    public class UI_WinPopup : UI_BaseWinLosePopup
    {
#pragma warning disable 0649
        private Button nextLvButton;
#pragma warning restore 0649

        private void Awake()
        {
            nextLvButton.onClick.AddListener(OnNextLvButtonClick);

        }

        private void OnDestroy()
        {
            nextLvButton.onClick.RemoveListener(OnNextLvButtonClick);
        }

        private void OnNextLvButtonClick()
        {

        }
    }
}