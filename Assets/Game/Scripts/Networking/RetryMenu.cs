using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class RetryMenu : MonoBehaviour
{
    [SerializeField] private LocalizeStringEvent errorMessage;
    [SerializeField] private Button retryButton, cancelButton;
    
    private enum Mode
    {
        Confirm,
        CloseableConfirm,
        Alert,
        Return
    }

    private Mode _mode;
    private IEnumerator _retryEnumerator;
    
    private void Open(Mode mode, IEnumerator retryEnumerator = null)
    {
        _mode = mode;
        _retryEnumerator = retryEnumerator;
        gameObject.SetActive(true);
        cancelButton.interactable = true;
        retryButton.interactable = (mode == Mode.Confirm || mode == Mode.CloseableConfirm);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        switch (_mode)
        {
            case Mode.Alert:
            case Mode.CloseableConfirm:
                Close();
                break;
            case Mode.Return:
            case Mode.Confirm:
                SceneManager.LoadScene(0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Retry()
    {
        cancelButton.interactable = false;
        retryButton.interactable = false;
        
        if (_retryEnumerator != null)
        {
            StartCoroutine(_retryEnumerator);
        }
    }

    public void UserIsNotLoggedIn()
    {
        SetMessage("id_not_logged_in");

        Open(Mode.Alert);
    }
    
    public void InvalidLoginCredentials()
    {
        SetMessage("id_invalid_data");

        Open(Mode.Alert);
    }
    
    public void SessionExpiredInGame()
    {
        SetMessage("id_score_error");

        Open(Mode.Return);
    }

    public void SessionExpiredInMenu()
    {
        SetMessage("id_expired_session");
        Open(Mode.Alert);
    }
    
    public void InternetConnectionLost(IEnumerator retryEnumerator, bool closeable = false)
    {
        SetMessage("id_connection_error");
        Open(closeable ? Mode.CloseableConfirm : Mode.Confirm, retryEnumerator);
    }

    private void SetMessage(string errorCode)
    {
        errorMessage.StringReference = new LocalizedString(LocalizationManager.MainMenuAndGameplayRandoms, errorCode);
    }
}