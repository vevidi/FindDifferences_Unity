﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.UI
{
    public class UI_WindowsManager : MonoBehaviour
    {
        [System.Serializable]
        public class WindowDescription
        {
            public eWindowType type;
            public GameObject prefab;
        }

        public enum eWindowType
        {
            Win,
            Lose,
            Error
        }

        public static UI_WindowsManager Instance { get; private set; }

#pragma warning disable 0649
        [SerializeField]
        private List<WindowDescription> windows;
#pragma warning restore 0649

        private Transform popupsRoot;
        private Dictionary<eWindowType, GameObject> openedWindows;
        private Stack<eWindowType> windowsStack;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            openedWindows = new Dictionary<eWindowType, GameObject>();
            windowsStack = new Stack<eWindowType>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void ShowWindow(UI_WindowConfig cfg)
        {
            var type = cfg.WType;
            var windowInfo = windows.Find(rez => rez.type.Equals(type));
            if (windowInfo != null)
            {
                GameObject prefab = windowInfo.prefab;
                if (prefab != null && popupsRoot != null)
                {
                    GameObject wnd = Instantiate(prefab, popupsRoot);
                    IBasePopup popup = wnd.GetComponent<IBasePopup>();
                    if (popup != null)
                        popup.Init(cfg);
                    openedWindows.Add(type, wnd);
#if UNITY_EDITOR
                    Utils.PrintDictionary(openedWindows);
#endif
                    windowsStack.Push(type);
                }
                else
                    Utils.DebugLog("Cannot find prefab for type <" + type + "> or type is incorrect!",eLogType.Error);
            }
            else
                Utils.DebugLog("Windows manager does not contain window info for type: " + type,eLogType.Error);
        }

        public void HideWindow(eWindowType type)
        {
            GameObject result = null;
            bool exists = openedWindows.TryGetValue(type, out result);
            if (exists)
            {
                openedWindows.Remove(type);
                windowsStack.Pop();
                Utils.PrintDictionary(openedWindows);
                Destroy(result);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (windowsStack.Count > 0)
                {
                    eWindowType WType = windowsStack.Peek();
                    GameObject windowGO = null;

#if UNITY_EDITOR
                    Utils.PrintDictionary(openedWindows);
#endif

                    if (openedWindows.TryGetValue(WType, out windowGO))
                    {
                        UI_BasePopup wBehaviour = windowGO.GetComponent<UI_BasePopup>();
                        if (wBehaviour != null && wBehaviour.IsBackBtnClosable)
                        {
                            wBehaviour.OnCloseButtonClick();
                            windowGO = null;
                        }
                    }
                }
                else
                    Application.Quit();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameObject pRoot = GameObject.Find(GameVariables.PopupsRoot);
            if(pRoot!=null)
                popupsRoot = pRoot.transform;
        }
    }
}