﻿using System;
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

    private Mode mode;
    private int numTries = 0;
    private IEnumerator retryEnumerator;
    
    private void Open(Mode mode, IEnumerator retryEnumerator = null)
    {
        this.mode = mode;
        this.retryEnumerator = retryEnumerator;
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
        switch (mode)
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
        
        if (retryEnumerator != null)
        {
            StartCoroutine(retryEnumerator);
        }
    }
    
    public void InvalidLoginCredentials()
    {
        message.SetText("E-mail, nome de usuário ou senha inválidos. Se esqueceu sua senha, clique em \"esqueci a senha\"");
        Open(Mode.Alert);
    }
    
    public void SessionExpiredInGame()
    {
        message.SetText("Sua sessão expirou. Por favor, retorne ao menu e faça login novamente");
        Open(Mode.Return);
    }

    public void SessionExpiredInMenu()
    {
        message.SetText("Sua sessão expirou. Por favor, faça login novamente");
        Open(Mode.Alert);
    }
    
    public void InternetConnectionLost(IEnumerator retryEnumerator, bool closeable = false)
    {
        numTries++;
        message.SetText($"Erro de conexão ({numTries}x)\nNão foi possível se conectar ao nosso servidor");
        Open(closeable ? Mode.CloseableConfirm : Mode.Confirm, retryEnumerator);
    }
}