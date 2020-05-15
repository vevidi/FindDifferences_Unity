using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;

namespace Vevidi.FindDiff.UI
{
    public class UI_LevelManager : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        public Button homeButton;
        [SerializeField]
        public TextMeshProUGUI diffFoundText;
#pragma warning restore 0649
        private Mediator gameEvents;
        private int diffCount;

        public void Init(int diffCount)
        {
            this.diffCount = diffCount;
            diffFoundText.text = string.Format("Differences found: {0}/{1}", 0, diffCount);
        }

        private void Awake()
        {
            gameEvents = GameManager.Instance.gameEventSystem;
            homeButton.onClick.AddListener(OnHomeButtonClick);
            gameEvents.Subscribe<UpdateLevelUiCommand>(OnDifferenceFound);
        }

        private void OnDestroy()
        {
            homeButton.onClick.RemoveListener(OnHomeButtonClick);
            gameEvents.DeleteSubscriber<UpdateLevelUiCommand>(OnDifferenceFound);
        }

        private void OnDifferenceFound(UpdateLevelUiCommand command)
        {
            Debug.Log(command.diffFoundValue);
            diffFoundText.text = string.Format("Differences found: {0}/{1}", command.diffFoundValue, diffCount);
        }

        private void OnHomeButtonClick()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }
    }
}
