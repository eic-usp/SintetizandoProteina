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

        private void Awake()
        {
            signInButton.gameObject.SetActive(true);
            signOutButton.gameObject.SetActive(false);
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
                signInButton.gameObject.SetActive(true);
                signOutButton.gameObject.SetActive(false);

                yield break;
            }
            
            yield return LoginRequestHandler.Validate(OnValidateSuccess, OnValidateFailure);
        }

        private void OnValidateSuccess()
        {
            OnSignInSuccess();
        }

        private void OnValidateFailure(UnityWebRequest request)
        {
            using var req = request;
            
            Debug.Log(req.responseCode);
            
            switch (req.responseCode)
            {
                case 401:
                    retryMenu.SessionExpiredInMenu();
                    signInButton.gameObject.SetActive(true);
                    signOutButton.gameObject.SetActive(false);
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

        private void OnSignInSuccess()
        {
            retryMenu.Close();
            signInButton.gameObject.SetActive(false);
            signOutButton.gameObject.SetActive(true);
        }

        private void OnSignInFailure(UnityWebRequest request)
        {
            using var req = request;
            switch (req.responseCode)
            {
                case 401:
                    retryMenu.InvalidLoginCredentials();
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
            retryMenu.Close();
            signInButton.gameObject.SetActive(true);
            signOutButton.gameObject.SetActive(false);
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
            var res = request;
            Debug.Log($"{res.responseCode}: {res.error}");
            // request.Dispose();
        }
    }
}