using System;
using System.Threading.Tasks;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.NetworkModel;
using RequestType = Vevidi.FindDiff.Network.NetworkManager.RequestType;

namespace Vevidi.FindDiff.Network
{
    public class GameDataDownloader
    {
        private static void PrintError(string error)
        {
            GameManager.Instance.gameEventSystem.Publish(new LoadingStatusCommand(LoadingStatusCommand.eLoadingStatus.Error, error));
        }

        private static async Task<LevelsModel> LoadGameInfo()
        {
            GameManager.Instance.gameEventSystem.Publish(new LoadingStatusCommand(LoadingStatusCommand.eLoadingStatus.Ok, "Loading game info"));
            LevelsModel result = await NetworkManager.Instance.RequestTaskJson<LevelsModel>(RequestType.Get, GameVariables.LoadUrl, null, PrintError);
            return result;
        }

        private static async Task<(bool, string)> LoadImages(LevelsModel levelsInfo)
        {
            foreach (LevelInfoModel model in levelsInfo.Levels)
            {
                string loadUrl = GameVariables.ImageLoadUrl + model.Image;
                var command = new LoadingStatusCommand(LoadingStatusCommand.eLoadingStatus.Ok, "LOADING: " + loadUrl);
                GameManager.Instance.gameEventSystem.Publish(command);
                Texture2D tex = await NetworkManager.Instance.RequestTaskTex2D(loadUrl, PrintError);
                if (tex == null)
                    return (true, model.Image);
                else
                    SaveLoadUtility.SaveImage(tex, model.Id + ".jpg");
            }
            return (false, "");
        }

        public static async void LoadGameData(Action<LevelsModel> onComplete, Action<string> onError)
        {
            var sManager = GameManager.Instance.SaveManager;
            var gameInfo = await LoadGameInfo();

            if (gameInfo == null)
                onError?.Invoke("Info not loaded!");
            else if (sManager.SaveLoaded && sManager.SaveVersion == gameInfo.Version)
            {
                // TODO: think about better way to make this logic
                onComplete?.Invoke(gameInfo);
                return; // prevent redownloading images
            }

            (bool isError, string fName) loadImgResult = await LoadImages(gameInfo);
            if (loadImgResult.isError == true)
                onError?.Invoke("Image not loaded: " + loadImgResult.fName);
            else
                onComplete?.Invoke(gameInfo);

            Debug.LogWarning("------ LOADED ------");
        }
    }
}