using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameMediator.Commands;

namespace Vevidi.FindDiff.UI
{
    public class UI_LosePopup : UI_BaseWinLosePopup
    {
#pragma warning disable 0649
        [SerializeField]
        private Button restartButton;
#pragma warning restore 0649

        protected override void Awake()
        {
            base.Awake();
            restartButton.onClick.AddListener(OnRestartButtonClick);

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            restartButton.onClick.RemoveListener(OnRestartButtonClick);
        }

        private void OnRestartButtonClick()
        {
            OnCloseButtonClick();
            gameEvents.Publish(new RestartLevelCommand());
        }
    }
}