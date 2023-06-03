using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Networking.DataStructs;

namespace Networking.RequestHandlers
{
    public static class MatchRequestHandler
    {
        public static IEnumerator CreateMatch(MatchData matchData, Action<UnityWebRequest> onSuccess = null,
            Action<UnityWebRequest> onFailure = null)
        {
            // matchData.sign = Cryptography.GetSignature(matchData);
            var data = JsonUtility.ToJson(matchData);
            Debug.Log(Endpoints.Match);
            Debug.Log(data);
            
            using var req = WebRequestFactory.AuthPostJson(Endpoints.Match, data);
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(req);
            }
            else
            {
                onFailure?.Invoke(req);
            }
            
            // req.Dispose();
        }
    }
}