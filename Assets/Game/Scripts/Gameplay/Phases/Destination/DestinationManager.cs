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

        private int _tries;
        
        private void Start()
        {
            destinationPanel.SetActive(true);
            destinationQuiz.SetActive(true);
            
            var protein = FindObjectOfType<GeneralScripts.Player.PlayerInfo>().ProteinName;
            quizManager.SetUp("QuizData" + protein[0].ToString().ToUpper() + protein[1..], 1);
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
            if (correct)
            {
                Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.RightAnswer);
                
                ScoreManager.Instance.UpdateScore(ScoreManager.ScoreContext.InMissionHit);
                
                if (_tries == 0)
                {
                    ScoreManager.Instance.UpdateScore(ScoreManager.ScoreContext.InMissionHitBonus);
                }
                
                EndPhase();
            }
            else
            {
                Audio.AudioManager.Instance.Play(Audio.SoundEffectTrack.WrongAnswer);
                
                _tries++;

                ScoreManager.Instance.UpdateScore(ScoreManager.ScoreContext.InMissionMiss);

                if (_tries % ScoreManager.Instance.DefaultPenaltyRequirement == 0)
                {
                    ScoreManager.Instance.UpdateScore(ScoreManager.ScoreContext.InMissionMissPenalty);
                }
            }
        }

        public new void EndPhase()
        {
            gameOverPanel.SetActive(true);
            base.EndPhase();
        }
    }
}