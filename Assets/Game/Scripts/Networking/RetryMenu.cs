using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RetryMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;
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
        message.SetText("Parece que você não está logado.\nRetorne ao menu e entre com seu usuário e senha para registrar sua pontuação e competir com seus amigos!");
        Open(Mode.Alert);
    }
    
    public void InvalidLoginCredentials()
    {
        message.SetText("E-mail, nome de usuário ou senha inválidos. Se esqueceu sua senha, clique em \"esqueci a senha\"");
        Open(Mode.Alert);
    }
    
    public void SessionExpiredInGame()
    {
        message.SetText("Não foi possível registrar sua pontuação. Por favor, retorne ao menu e faça login novamente");
        Open(Mode.Return);
    }

    public void SessionExpiredInMenu()
    {
        message.SetText("Sua sessão expirou. Por favor, faça login novamente");
        Open(Mode.Alert);
    }
    
    public void InternetConnectionLost(IEnumerator retryEnumerator, bool closeable = false)
    {
        message.SetText($"Erro de conexão\nNão foi possível se conectar ao nosso servidor");
        Open(closeable ? Mode.CloseableConfirm : Mode.Confirm, retryEnumerator);
    }
}