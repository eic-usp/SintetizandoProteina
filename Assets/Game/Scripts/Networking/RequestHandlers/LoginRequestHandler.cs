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

        public static void SaveAuthCookie(UnityWebRequest req, string token, bool remember = false)
        {
            var authHeaderValue = $"Bearer {token}";
            
            if (remember)
            {
                PlayerPrefs.SetString(AuthKey, authHeaderValue);
                TempAuth = null;
            }
            else
            {
                TempAuth = authHeaderValue;
                PlayerPrefs.DeleteKey(AuthKey);
                UnityWebRequest.ClearCookieCache();
            }
        }

        public static IEnumerator Login(LoginData data, Action<PlayerStatus> onSuccess, Action<UnityWebRequest> onFailure = null)
        {
            RaycastBlockEvent.Invoke(true);
            
            var req = WebRequestFactory.PostJson(Endpoints.Login, JsonUtility.ToJson(data));
            yield return req.SendWebRequest();

#if UNITY_EDITOR
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
#endif

            RaycastBlockEvent.Invoke(false);

            if (req.result == UnityWebRequest.Result.Success)
            {
                var loginReturnData = JsonUtility.FromJson<LoginReturnData>(req.downloadHandler.text);
                var playerStatus = loginReturnData.player;
                var token = loginReturnData.token;

                // if (Cryptography.IsSignatureValid(playerStatus))
                    // {
                        SaveAuthCookie(req, token, data.remember);
                        onSuccess?.Invoke(playerStatus);
                        req.Dispose();
                        // }
                    // else
                    // {
                    //     onFailure?.Invoke(req);
                    // }
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