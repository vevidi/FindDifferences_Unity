using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vevidi.FindDiff.NetworkModel;
using Vevidi.FindDiff.Network;
using Vevidi.FindDiff.GameLogic;

namespace Vevidi.FindDiff.UI
{
    public class UI_PreloaderManager : MonoBehaviour
    {

        private async void Start()
        {
            bool result = await NetworkManager.Instance.CheckInternetConnection(StartDataLoad);
            if (!result)
                Debug.LogError("Preloader -> No internet connction!");
        }

        private void StartDataLoad()
        {
            // TODO: add correct callbacks with notifications
            GameDataDownloader.LoadGameData(OnLoadComplete, OnLoadError);
        }

        private void OnLoadComplete(LevelsModel model)
        {
            GameManager.Instance.InitLevelsManager(model);
            SceneManager.LoadScene(GameVariables.MainMenuScene);
        }

        private void OnLoadError(string error)
        {
            Debug.LogError("Data load error: " + error);
        }
    }
}