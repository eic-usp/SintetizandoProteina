using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Networking.DataStructs;
using Networking.RequestHandlers;

namespace Networking
{
    public class SignInManager : MonoBehaviour
    {
        [SerializeField] private Button signInButton, signOutButton;
        [SerializeField] private RetryMenu retryMenu;
        [SerializeField] private TMP_InputField emailOrId;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private Toggle remember;
        [SerializeField] private GameObject loginPanel;

        public static PlayerStatus PlayerStatus { get; private set; }
        private const string PlayerStatusFilePath = "player.json";

        private enum LoginState
        {
            LoggedIn,
            LoggedOut,
            LoginFailed
        }

        private void Awake()
        {
            SwitchState(LoginState.LoggedOut);
        }

        private void Start()
        {
            StartCoroutine(ValidateCookie());
        }

        private IEnumerator ValidateCookie()
        {
            var authCookie = PlayerPrefs.GetString(LoginRequestHandler.AuthKey, LoginRequestHandler.TempAuth);

            if (string.IsNullOrEmpty(authCookie))
            {
                SwitchState(LoginState.LoginFailed);
                yield break;
            }
            
            yield return LoginRequestHandler.Validate(OnValidateSuccess, OnValidateFailure);
        }

        private void OnValidateSuccess()
        {
            var json = System.IO.File.ReadAllText(Application.persistentDataPath + PlayerStatusFilePath);
            JsonUtility.FromJsonOverwrite(json, PlayerStatus);
            
            SwitchState(LoginState.LoggedIn);
        }

        private void OnValidateFailure(UnityWebRequest request)
        {
            using var req = request;
            
            Debug.Log(req.responseCode);
            
            switch (req.responseCode)
            {
                case 401:
                    retryMenu.SessionExpiredInMenu();
                    SwitchState(LoginState.LoginFailed);
                    break;
                default:
                    retryMenu.InternetConnectionLost(ValidateCookie(), true);
                    break;
            }
            
            // req.Dispose();
        }

        public void SignIn()
        {
            if (string.IsNullOrEmpty(emailOrId.text) || string.IsNullOrEmpty(password.text)) return;
            loginPanel.SetActive(false);
            StartCoroutine(SignInEnumerator());
        }

        private IEnumerator SignInEnumerator()
        {
            var loginData = new LoginData
            {
                emailOrId = emailOrId.text.Trim(),
                password = password.text.Trim()
            };
            
            yield return LoginRequestHandler.Login(loginData, OnSignInSuccess, OnSignInFailure, remember.isOn);
        }

        private void OnSignInSuccess(PlayerStatus playerStatus)
        {
            PlayerStatus = playerStatus;
            var json = JsonUtility.ToJson(playerStatus, true);
            System.IO.File.WriteAllText(Application.persistentDataPath + PlayerStatusFilePath, json);
            
            SwitchState(LoginState.LoggedIn);
        }

        private void OnSignInFailure(UnityWebRequest request)
        {
            using var req = request;
            switch (req.responseCode)
            {
                case 401:
                    retryMenu.InvalidLoginCredentials();
                    SwitchState(LoginState.LoginFailed);
                    break;
                default:
                    retryMenu.InternetConnectionLost(SignInEnumerator(), true);
                    break;
            }
            // req.Dispose();
        }

        public void SignOut()
        {
            UnityWebRequest.ClearCookieCache();
            PlayerPrefs.DeleteKey(LoginRequestHandler.AuthKey);
            LoginRequestHandler.TempAuth = string.Empty;
            
            SwitchState(LoginState.LoggedOut);
        }

        public void TestApi()
        {
            var matchData = new MatchData
            {
                score = 500
            };
            
            StartCoroutine(MatchRequestHandler.CreateMatch(
                    matchData, OnSuccessOrFailureDebug, OnSuccessOrFailureDebug
                )
            );
        }
        private static void OnSuccessOrFailureDebug(UnityWebRequest request)
        {
            Debug.Log($"{request.responseCode}: {request.error}");
            // request.Dispose();
        }

        private void SwitchState(LoginState state)
        {
            switch (state)
            {
                case LoginState.LoggedIn:
                    retryMenu.Close();
                    signInButton.gameObject.SetActive(false);
                    signOutButton.gameObject.SetActive(true);
                    break;
                case LoginState.LoggedOut:
                    retryMenu.Close();
                    signInButton.gameObject.SetActive(true);
                    signOutButton.gameObject.SetActive(false);
                    break;
                case LoginState.LoginFailed:
                    signInButton.gameObject.SetActive(true);
                    signOutButton.gameObject.SetActive(false);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}