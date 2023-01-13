using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Phases.Destination
{
    public class QuizManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Player needs to select all right answers, not just one")] private bool multiple;
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private Color rightAnswerColor;
        [SerializeField] private Color wrongAnswerColor;
        
        private int _correctAnswersTotal;
        private int _correctAnswers;
        private CanvasGroup _canvasGroup;
        public UnityAction OnComplete;

        private List<QuizOption> _options;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _options = new List<QuizOption>(GetComponentsInChildren<QuizOption>());
            Setup();
        }

        private void Setup()
        {
            var protein = FindObjectOfType<GeneralScripts.Player.PlayerInfo>().ProteinName;
            var json = Resources.Load<TextAsset>("QuizData");
            var qd = JsonUtility.FromJson<QuizData>(json.text);
            
            // Debug.Log($"Protein: {protein}");

            var qdiList = protein switch
            {
                "glucagon" => qd.glucagon,
                "insulina" => qd.insulina,
                "hidrolas" => qd.hidrolas,
                "tirosinas" => qd.tirosinas,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var qdi = qdiList[UnityEngine.Random.Range(0, qdiList.Count)]; 

            questionText.text = qdi.question;

            for (var i = 0; i < _options.Count; i++)
            {
                _options[i].Answer = qdi.options[i].answer;
                // Debug.Log($"option {i} answer is {qdi.options[i].answer} => {(qdi.options[i].correct ? "Correct!" : "Incorrect")}");
                _options[i].Correct = qdi.options[i].correct;
                if (_options[i].Correct) _correctAnswersTotal++;
            }
        }

        public Color Choose(bool correct)
        {
            // Debug.Log($"{(correct ? "Right" : "Wrong")} answer");

            if (correct)
            {
                _correctAnswers++;
                
                if (!multiple || _correctAnswers == _correctAnswersTotal)
                {
                    // Debug.Log("Clear!");
                    _canvasGroup.interactable = false;
                    OnComplete?.Invoke();
                }
                
                Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.RightAnswer);
                
                return rightAnswerColor;
            }
            
            Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.WrongAnswer);
            
            return wrongAnswerColor;
        }
    }
}