using System;
using System.Collections;
using Networking.DataStructs;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking.RequestHandlers
{
    public static class PlayerStatusRequestHandler
    {
        public static IEnumerator GetPlayerStatus(Action<PlayerStatus> onSuccess, Action<UnityWebRequest> onFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            var req = WebRequestFactory.AuthGet(Endpoints.PlayerStatus);
            yield return req.SendWebRequest();

            RaycastBlockEvent.Invoke(false);

            if (req.result == UnityWebRequest.Result.Success)
            {
                var playerStatus = JsonUtility.FromJson<PlayerStatus>(req.downloadHandler.text);
                onSuccess?.Invoke(playerStatus);

                // if (Cryptography.IsSignatureValid(playerStatus))
                // {
                //     onSuccess?.Invoke(playerStatus);
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
    }
}