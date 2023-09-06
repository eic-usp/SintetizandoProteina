using System.Text;
using UnityEngine.Networking;
using Networking.RequestHandlers;
using UnityEngine;

namespace Networking
{
    internal static class WebRequestFactory
    {
        private const int Timeout = 10;

        public static UnityWebRequest PostJson(string url, string data = "{}")
        {
            var req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);

            var bodyRaw = Encoding.UTF8.GetBytes(data);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            
            req.disposeDownloadHandlerOnDispose = true;
            req.disposeUploadHandlerOnDispose = true;

            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = Timeout;

            return req;
        }

        public static UnityWebRequest AuthPostJson(string url, string data = "{}")
        {
            var req = PostJson(url, data);

#if !UNITY_WEBGL
            var authCookie = PlayerPrefs.GetString(LoginRequestHandler.AuthKey, LoginRequestHandler.TempAuth);
            req.SetRequestHeader("Cookie", authCookie);
#elif UNITY_WEBGL
            if (!string.IsNullOrEmpty(LoginRequestHandler.TempAuth))
            {
                var authCookie = LoginRequestHandler.TempAuth;
                req.SetRequestHeader("Cookie", authCookie);
            }
#endif
            req.timeout = Timeout;

            return req;
        }

        public static UnityWebRequest AuthGet(string url)
        {
            var req = UnityWebRequest.Get(url);

#if !UNITY_WEBGL
            var authCookie = PlayerPrefs.GetString(LoginRequestHandler.AuthKey, LoginRequestHandler.TempAuth);
            req.SetRequestHeader("Cookie", authCookie);
#elif UNITY_WEBGL
            if (!string.IsNullOrEmpty(LoginRequestHandler.TempAuth))
            {
                var authCookie = LoginRequestHandler.TempAuth;
                req.SetRequestHeader("Cookie", authCookie);
            }
#endif
            req.timeout = Timeout;

            return req;
        }
    }
}