using System;
using System.Collections;
using Networking.DataStructs;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking.RequestHandlers
{
    public static class PlayerStatusRequestHandler
    {
        public static IEnumerator GetUserStatus(Action<PlayerStatus> onSuccess,
            Action<UnityWebRequest> onFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            var request = WebRequestFactory.AuthGet(Endpoints.PlayerStatus);
            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);

            if (request.result == UnityWebRequest.Result.Success)
            {
                var userStatus = JsonUtility.FromJson<PlayerStatus>(request.downloadHandler.text);

                if (Cryptography.IsSignatureValid(userStatus))
                {
                    onSuccess?.Invoke(userStatus);
                }
                else
                {
                    onFailure?.Invoke(request);
                }
            }
            else
            {
                onFailure?.Invoke(request);
            }
        }
    }
}