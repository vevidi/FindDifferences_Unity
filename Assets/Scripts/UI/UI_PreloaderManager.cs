﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vevidi.FindDiff.NetworkModel;
using Vevidi.FindDiff.Network;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator;
using TMPro;
using eLoadingStatus = Vevidi.FindDiff.GameMediator.LoadingStatusCommand.eLoadingStatus;

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
            gameEvents.Subscribe<LoadingStatusCommand>(OnLoadingStatusChange);
        }

        private async void Start()
        {
            bool result = await NetworkManager.Instance.CheckInternetConnection(StartDataLoad);
            if (!result)
            {
                //Debug.LogError("Preloader -> No internet connction!");
                gameEvents.Publish(new LoadingStatusCommand(eLoadingStatus.Error, "No internet connction!"));
            }
        }
               
        private void OnLoadingStatusChange(LoadingStatusCommand command)
        {
            string prefix = command.status == eLoadingStatus.Ok ? "OK -> " : "ERROR -> ";
            loadingStatus.text = "Status: " + prefix + " " + command.message;
        }

        private void StartDataLoad()
        {
            // TODO: add correct callbacks with notifications
            //gameEvents
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
            gameEvents.DeleteSubscriber<LoadingStatusCommand>(OnLoadingStatusChange);
        }
    }
}