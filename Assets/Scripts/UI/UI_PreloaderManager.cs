using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.Network;
using Vevidi.FindDiff.NetworkModel;
using eLoadingStatus = Vevidi.FindDiff.GameMediator.Commands.LoadingStatusCommand.eLoadingStatus;

namespace Vevidi.FindDiff.UI
{
    public class UI_PreloaderManager : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private TextMeshProUGUI loadingStatus;
#pragma warning restore 0649

        private Mediator gameEvents;

        private void Awake()
        {
            gameEvents = GameManager.Instance.gameEventSystem;
            gameEvents.Subscribe<LoadingStatusCommand>(LoadingStatusChange);
            gameEvents.Subscribe<StartLoadAgainCommand>(TryLoadAgain);
        }

        private async void StartLoad()
        {
            bool result = await NetworkManager.Instance.CheckInternetConnection(StartDataLoad);
            if (!result)
            {
                gameEvents.Publish(new LoadingStatusCommand(eLoadingStatus.Error, "No internet connection!"));
            }
        }

        private void Start()
        {
            StartLoad();
        }
               
        private void LoadingStatusChange(LoadingStatusCommand command)
        {
            string prefix = command.Status == eLoadingStatus.Ok ? "OK -> " : "ERROR -> ";
            loadingStatus.text = "Status: " + prefix + " " + command.Message;
            if(command.Status == eLoadingStatus.Error)
            {
                UI_WindowConfigBuilder wConfig = new UI_WindowConfigBuilder(UI_WindowsManager.eWindowType.Error);
                wConfig.AddData("message", command.Message);
                UI_WindowsManager.Instance.ShowWindow(wConfig.Build());
            }
        }

        private void TryLoadAgain(StartLoadAgainCommand command)
        {
            StartLoad();
        }

        private void StartDataLoad()
        {
            GameDataDownloader.LoadGameData(OnLoadComplete, OnLoadError);
        }

        private void OnLoadComplete(LevelsModel model)
        {
            GameManager.Instance.InitLevelsManager(model);
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }

        private void OnLoadError(string error)
        {
            //gameEvents.Publish(new LoadingStatusCommand(eLoadingStatus.Error, error));
        }

        private void OnDestroy()
        {
            gameEvents.DeleteSubscriber<LoadingStatusCommand>(LoadingStatusChange);
            gameEvents.DeleteSubscriber<StartLoadAgainCommand>(TryLoadAgain);
        }
    }
}