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
        private Button homeButton;
        [SerializeField]
        private TextMeshProUGUI diffFoundText;
        [SerializeField]
        private Image livesImage;
        [SerializeField]
        private Button hintButton;
#pragma warning restore 0649
        private Mediator gameEvents;

        private void Awake()
        {
            gameEvents = GameManager.Instance.gameEventSystem;
            homeButton.onClick.AddListener(OnHomeButtonClick);
            hintButton.onClick.AddListener(OnHintButtonClick);
            gameEvents.Subscribe<UpdateDiffCountCommand>(OnDifferenceFound);
            gameEvents.Subscribe<UpdateLivesCountCommand>(OnLivesCountChanged);
        }

        private void OnDestroy()
        {
            homeButton.onClick.RemoveListener(OnHomeButtonClick);
            hintButton.onClick.RemoveListener(OnHintButtonClick);
            gameEvents.DeleteSubscriber<UpdateDiffCountCommand>(OnDifferenceFound);
            gameEvents.DeleteSubscriber<UpdateLivesCountCommand>(OnLivesCountChanged);
        }

        private void OnDifferenceFound(UpdateDiffCountCommand command)
        {
            diffFoundText.text = string.Format("Differences found: {0}/{1}", command.DiffFoundValue, command.MaxValue);
        }

        private void OnLivesCountChanged(UpdateLivesCountCommand command)
        {
            // TODO: add some stuff here
            livesImage.fillAmount = 1f * command.LivesCount / command.MaxLives;
        }

        private void OnHomeButtonClick()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }

        private void OnHintButtonClick()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
            gameEvents.Publish(new ShowHintCommand());
        }
    }
}
