﻿using UnityEngine;

namespace Networking
{
    [System.Serializable]
    public struct ApiSettings
    {
        public string apiURL;
        public string gameId;
        public string responseSignatureKey;
        public string requestSignatureKey;
    }

    public class LoadApiSettings : MonoBehaviour
    {   
        private const string APISettingsPath = "Secrets/api_settings";
    
        public void Awake()
        {
            var json = Resources.Load<TextAsset>(APISettingsPath);
            var settings = JsonUtility.FromJson<ApiSettings>(json.text);

            Endpoints.Base = settings.apiURL;
            Endpoints.GameId = settings.gameId;
            Cryptography.RequestSignatureKey = settings.requestSignatureKey;
            Cryptography.ResponseSignatureKey = settings.responseSignatureKey;
        }
    }
}