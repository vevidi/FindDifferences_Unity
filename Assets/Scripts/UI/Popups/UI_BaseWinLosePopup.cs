using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vevidi.FindDiff.UI
{
    public class UI_BaseWinLosePopup : UI_BasePopup
    {
#pragma warning disable 0649
        [SerializeField]
        private Button backToMenuButton;
#pragma warning restore 0649

        private void Awake()
        {
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
        }

        private void OnDestroy()
        {
            backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClick);
        }

        private void OnBackToMenuButtonClick()
        {
            base.OnCloseButtonClick();
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }
    }
}
