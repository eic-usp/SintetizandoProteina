using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private bool multiple;
    private int _correctAnswersTotal;
    private int _correctAnswers;
    private CanvasGroup _canvasGroup;
    public UnityAction OnComplete;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        if (!multiple)
        {
            _correctAnswersTotal = 1;
            return;
        }
        
        _correctAnswersTotal = new List<QuizOption>(GetComponentsInChildren<QuizOption>()).FindAll(o => o.Correct).Count;
    }
    
    public void Choose(bool value)
    {
        Debug.Log($"{(value ? "Right" : "Wrong")} answer");
        if (!value) return;
        
        _correctAnswers++;
        
        if (_correctAnswers == _correctAnswersTotal)
        {
            Debug.Log("Clear!");
            OnComplete.Invoke();
            _canvasGroup.interactable = false;
        }
    }
}
