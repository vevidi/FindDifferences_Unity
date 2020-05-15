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
        private Image[] livesImages;
        [SerializeField]
        private Button hintButton;
#pragma warning restore 0649
        private Mediator gameEvents;

        private void Awake()
        {
            gameEvents = GameManager.Instance.gameEventSystem;
            homeButton.onClick.AddListener(OnHomeButtonClick);
            gameEvents.Subscribe<UpdateDiffCountCommand>(OnDifferenceFound);
            gameEvents.Subscribe<UpdateLivesCountCommand>(OnLivesCountChanged);
        }

        private void OnDestroy()
        {
            homeButton.onClick.RemoveListener(OnHomeButtonClick);
            gameEvents.DeleteSubscriber<UpdateDiffCountCommand>(OnDifferenceFound);
            gameEvents.DeleteSubscriber<UpdateLivesCountCommand>(OnLivesCountChanged);
        }

        private void OnDifferenceFound(UpdateDiffCountCommand command)
        {
            Debug.Log(command.DiffFoundValue);
            diffFoundText.text = string.Format("Differences found: {0}/{1}", command.DiffFoundValue, command.MaxValue);
        }

        private void OnLivesCountChanged(UpdateLivesCountCommand command)
        {
            // TODO: add some stuff here
            Debug.Log("Lives count -> " + command.LivesCount);
        }

        private void OnHomeButtonClick()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }
    }
}
