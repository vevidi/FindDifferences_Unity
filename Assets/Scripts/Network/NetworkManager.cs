using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.Network
{
    public class NetworkManager : MonoBehaviour
    {
        public enum RequestType
        {
            Get,
            Post
        }

        private static NetworkManager instance = null;

        public static NetworkManager Instance
        {
            get
            {
                Init();
                return instance;
            }
            private set => instance = value;
        }

        protected static void Init()
        {
            if (instance == null)
            {
                GameObject go = new GameObject("NetworkManager");
                instance = go.AddComponent<NetworkManager>();
                DontDestroyOnLoad(go);
            }
        }

        public async Task<T> RequestTaskJson<T>(RequestType type, string url, WWWForm form = null, Action<string> onError = null)
        {
            return JsonUtility.FromJson<T>(await RequestTaskVal(type, url, form, onError));
        }

        private bool CheckForError(UnityWebRequest req, Action<string> onError)
        {
            if (req.isHttpError)
            {
                Utils.DebugLog("Http error: " + req.error + " " + req.url, eLogType.Warning);
                onError?.Invoke("Http error: " + req.error + " " + req.url);
                return true;
            }
            if (req.isNetworkError)
            {
                Utils.DebugLog("Network error: " + req.error + " " + req.url, eLogType.Warning);
                onError?.Invoke("Network error: " + req.error + " " + req.url);
                return true;
            }
            return false;
        }

        public async Task<Texture2D> RequestTaskTex2D(string url, Action<string> onError)
        {
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
            await req.SendWebRequest();
            if (!CheckForError(req, onError))
            {
                Utils.DebugLog("Texture request success " + url);
                DownloadHandlerTexture texHandler = req.downloadHandler as DownloadHandlerTexture;
                return texHandler.texture;
            }
            return default;
        }

        public async Task<string> RequestTaskVal(RequestType type, string url, WWWForm form = null, Action<string> onError = null)
        {
            UnityWebRequest req;

            if (type == RequestType.Get)
                req = UnityWebRequest.Get(url);
            else
                req = UnityWebRequest.Post(url, form);
            await req.SendWebRequest();
            if (!CheckForError(req,onError))
            {
                Utils.DebugLog("Http request success " + url);
                Utils.DebugLog(req.downloadHandler.text,eLogType.Warning);
                return req.downloadHandler.text;
            }
            return default;
        }

        private async Task<bool> CheckConnection(string url)
        {
            UnityWebRequest req = UnityWebRequest.Get(url);
            await req.SendWebRequest();
            if (req.isHttpError || req.isNetworkError)
            {
                Utils.DebugLog("Check connection error: " + req.error, eLogType.Warning);
                return false;
            }
            else
                return true;
        }

        public async Task<bool> CheckInternetConnection(Action callback)
        {
            bool result = false;
            try
            {
                result = await CheckConnection(@"https://2xdgames.ru/web-games/other/test.php" );
                if (result)
                    callback?.Invoke();
                else
                    Utils.DebugLog("Network Manager -> No internet connection!",eLogType.Warning);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return result;
        }
    }
}