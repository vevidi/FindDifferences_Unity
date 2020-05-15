using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vevidi.FindDiff.UI
{
    public class UI_LosePopup : UI_BaseWinLosePopup
    {
#pragma warning disable 0649
        private Button restartButton;
#pragma warning restore 0649

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);

        }

        private void OnDestroy()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnRestartButtonClick()
        {

        }
    }
}