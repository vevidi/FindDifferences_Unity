using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;

namespace Vevidi.FindDiff.UI
{
    public class UI_WinPopup : UI_BaseWinLosePopup
    {
#pragma warning disable 0649
        [SerializeField]
        private Button nextLvButton;
#pragma warning restore 0649

        protected override void Awake()
        {
            base.Awake();
            nextLvButton.onClick.AddListener(OnNextLvButtonClick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            nextLvButton.onClick.RemoveListener(OnNextLvButtonClick);
        }

        private void OnNextLvButtonClick()
        {
            OnCloseButtonClick();
            gameEvents.Publish(new NextLevelCommand());
        }
    }
}