using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vevidi.FindDiff.UI
{
    public class UI_GameEndedPopup : UI_BasePopup
    {
#pragma warning disable 0649
        [SerializeField]
        private Button backToMenuButton;
#pragma warning restore 0649
        //private Button popupBackround;

        private void Awake()
        {
            //popupBackround = GetComponent<Button>();
            //popupBackround.onClick.AddListener(OnCloseButtonClick);
            backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
        }

        private void OnDestroy()
        {
            //popupBackround.onClick.RemoveListener(OnCloseButtonClick);
            backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClick);
        }

        public void OnBackToMenuButtonClick()
        {
            base.OnCloseButtonClick();
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }
    }
}
