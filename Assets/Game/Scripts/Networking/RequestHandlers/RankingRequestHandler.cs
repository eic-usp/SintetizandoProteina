using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Networking.DataStructs;

namespace Networking.RequestHandlers
{
    public static class RankingRequestHandler
    {
        public static IEnumerator GetRankings(Action<RankingData> onSuccess, Action<UnityWebRequest> onFailure = null)
        {
            RaycastBlockEvent.Invoke(true);

            var request = WebRequestFactory.AuthGet(Endpoints.Rankings);
            yield return request.SendWebRequest();

            RaycastBlockEvent.Invoke(false);

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Debug.Log(request.downloadHandler.text);
                var rankingData = JsonUtility.FromJson<RankingData>(request.downloadHandler.text);
                onSuccess?.Invoke(rankingData);
            }
            else
            {
                onFailure?.Invoke(request);
            }
        }
    }
}