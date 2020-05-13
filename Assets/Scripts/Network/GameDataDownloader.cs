using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Vevidi.FindDiff.NetworkModel;
using Vevidi.FindDiff.Network;
using RequestType = Vevidi.FindDiff.Network.NetworkManager.RequestType;
using Vevidi.FindDiff.Utils;

namespace Vevidi.FindDiff.Network
{
    public class GameDataDownloader
    {
        private static void PrintError(string error)
        {
            Debug.LogError("Network error: \n" + error);
        }

        private static async Task<LevelsModel> LoadGameInfo()
        {
            LevelsModel result = await NetworkManager.Instance.RequestTaskJson<LevelsModel>(RequestType.Get, GameVariables.LoadUrl, null, PrintError);
            return result;
        }

        private static async Task<(bool, string)> LoadImages(LevelsModel levelsInfo)
        {
            foreach (LevelInfoModel model in levelsInfo.Levels)
            {
                Texture2D tex = await NetworkManager.Instance.RequestTaskTex2D(GameVariables.ImageLoadUrl + model.Image, PrintError);
                if (tex == null)
                    return (true, model.Image);
                else
                    SaveLoadUtility.SaveImage(tex, model.Id + ".jpg");
            }
            return (false, "");
        }

        public static async void LoadGameData(Action<LevelsModel> onComplete, Action<string> onError)
        {
            var gameInfo = await LoadGameInfo();
            if (gameInfo == null)
                onError?.Invoke("Info not loaded!");
            (bool isError, string fName) loadImgResult = await LoadImages(gameInfo);
            if (loadImgResult.isError == true)
                onError?.Invoke("Image not loaded: " + loadImgResult.fName);
            else
                onComplete?.Invoke(gameInfo);

            Debug.LogWarning("------ LOADED ------");
        }
    }
}