using UnityEngine;
using EIC.Quiz;

namespace Phases.Destination
{
    public class DestinationManager : PhaseManagerMono
    {
        [Space] [Header("Destination Manager Variables")] [Space]
        [SerializeField] private QuizManager quizManager;
        [SerializeField] private GameObject destinationQuiz;
        [SerializeField] private GameObject destinationPanel;
        [SerializeField] private GameObject gameOverPanel;
        
        private void Start()
        {
            destinationPanel.SetActive(true);
            destinationQuiz.SetActive(true);
            
            var protein = FindObjectOfType<GeneralScripts.Player.PlayerInfo>().ProteinName;
            quizManager.Setup("QuizData" + protein[0].ToString().ToUpper() + protein[1..], 1);
            quizManager.PopQuestion();
        }

        private void OnEnable()
        {
            quizManager.OnChoose += OnChoose;
        }
        
        private void OnDisable()
        {
            quizManager.OnChoose -= OnChoose;
        }

        private void OnChoose(bool correct, QuizResult quizResult)
        {
            Audio.AudioManager.Instance.Play(correct ? Audio.SoundEffectTrack.RightAnswer : Audio.SoundEffectTrack.WrongAnswer);
            
            if (correct)
            {
                EndPhase();
            }
        }

        public new void EndPhase()
        {
            gameOverPanel.SetActive(true);
            base.EndPhase();
        }
    }
}