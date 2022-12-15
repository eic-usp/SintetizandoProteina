using UnityEngine;

namespace Phases.Destination
{
    public class DestinationManager : PhaseManagerMono
    {
        [Space] [Header("Destination Manager Variables")] [Space]
        [SerializeField] private QuizManager quizManager;
        [SerializeField] private Wait.MissionManager missionManager;
        [SerializeField] private GameObject destinationQuiz;
        [SerializeField] private GameObject destinationPanel;

        private void Start()
        {
            destinationPanel.SetActive(false);
        }
        
        private void Update()
        {
            if (!missionManager.FinishedInstructions) return;
            destinationPanel.SetActive(true);
            destinationQuiz.SetActive(true);
        }
        
        private void OnEnable()
        {
            quizManager.OnComplete += EndPhase;
        }
        
        private void OnDisable()
        {
            quizManager.OnComplete -= EndPhase;
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}