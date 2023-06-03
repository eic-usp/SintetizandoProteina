using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Networking.DataStructs;

namespace Networking.RequestHandlers
{
    public static class LoginRequestHandler
    {
        public const string AuthKey = "Auth";
        public static string TempAuth;

        public static void SaveAuthCookie(UnityWebRequest req, bool remember = false)
        {
            var cookie = req.GetResponseHeader("Set-Cookie");

            if (!string.IsNullOrEmpty(cookie))
            {
                if (remember)
                {
                    PlayerPrefs.SetString(AuthKey, cookie);
                }
                else
                {
                    TempAuth = cookie;
                    UnityWebRequest.ClearCookieCache();
                }
            }
        }

        public static IEnumerator Login(LoginData data, Action onSuccess, Action<UnityWebRequest> onFailure = null,
            bool remember = false)
        {
            RaycastBlockEvent.Invoke(true);
            
            var req = WebRequestFactory.PostJson(Endpoints.Login, JsonUtility.ToJson(data));
            yield return req.SendWebRequest();

            try
            {
                foreach (var responseHeader in req.GetResponseHeaders())
                {
                    Debug.Log(responseHeader);
                }
            }
            catch (Exception error)
            {
                Debug.LogError(error);
            }

            RaycastBlockEvent.Invoke(false);

            if (req.result == UnityWebRequest.Result.Success)
            {
                SaveAuthCookie(req, remember);
                onSuccess?.Invoke();
                req.Dispose();
            }
            else
            {
                onFailure?.Invoke(req);
            }
        }

        public static IEnumerator Validate(Action onSuccess, Action<UnityWebRequest> onFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            var req = WebRequestFactory.AuthGet(Endpoints.Validate);
            yield return req.SendWebRequest();

            RaycastBlockEvent.Invoke(false);

            if (req.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke();
                req.Dispose();
            }
            else
            {
                onFailure?.Invoke(req);
            }
        }
    }
}