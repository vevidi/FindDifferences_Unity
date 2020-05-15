using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator;

namespace Vevidi.FindDiff.UI
{
    public class UI_BaseWinLosePopup : UI_BasePopup
    {
#pragma warning disable 0649
        [SerializeField]
        private Button backToMenuButton;
#pragma warning restore 0649

        protected override void Awake()
        {
            base.Awake();
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
        }

        protected virtual void OnDestroy()
        {
            backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClick);
        }

        protected void OnBackToMenuButtonClick()
        {
            base.OnCloseButtonClick();
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }
    }
}
