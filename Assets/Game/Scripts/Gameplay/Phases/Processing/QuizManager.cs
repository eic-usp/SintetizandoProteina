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

    private List<QuizOption> _options;

    [System.Serializable]
    public struct QuizDataStruct
    {
        public string name;
        public QuizDataItem item;
    }

    [System.Serializable]
    public struct QuizDataItem
    {
        public string name;
        public string question;
        public QuizDataOption[] options;
    }

    [System.Serializable]
    public struct QuizDataOption
    {
        public string question;
        public bool correct;
    }

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        
        if (!multiple)
        {
            _correctAnswersTotal = 1;
            return;
        }
        
        _options = new List<QuizOption>(GetComponentsInChildren<QuizOption>());
        
        var protein = FindObjectOfType<GeneralScripts.Player.PlayerInfo>().GetActualProtein();
        var json = Resources.Load<TextAsset>("QuizData");
        var qd = JsonUtility.FromJson<QuizDataStruct>(json.text);

        foreach (var o in _options)
        {

        }

        _correctAnswersTotal = _options.FindAll(o => o.Correct).Count;
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
