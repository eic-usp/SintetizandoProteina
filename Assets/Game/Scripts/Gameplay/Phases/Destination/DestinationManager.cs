using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Phases.Destination
{
    public class DestinationManager : PhaseManagerMono
    {
        [Space] [Header("Destination Manager Variables")] [Space]
        [SerializeField] private QuizManager quizManager;

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