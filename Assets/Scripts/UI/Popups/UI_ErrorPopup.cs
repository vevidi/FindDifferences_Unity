using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameMediator.Commands;

namespace Vevidi.FindDiff.UI
{
    public class UI_ErrorPopup : UI_BasePopup
    {
#pragma warning disable 0649
        [SerializeField]
        private Button tryAgainButton;
        [SerializeField]
        public TextMeshProUGUI errorText;
#pragma warning restore 0649

        public override void Init(UI_WindowConfig cfg)
        {
            base.Init(cfg);
            if (cfg.WData.ContainsKey("message"))
                errorText.text = cfg.WData["message"] as string;
        }

        protected override void Awake()
        {
            base.Awake();
            tryAgainButton.onClick.AddListener(OnRetryClick);
        }

        private void OnDestroy()
        {
            tryAgainButton.onClick.RemoveListener(OnRetryClick);
        }

        private void OnRetryClick()
        {
            OnCloseButtonClick();
            gameEvents.Publish(new StartLoadAgainCommand());
        }
    }
}
