using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

using Phases.Cell;

namespace Phases.Processing
{
    public class ProcessingManager : PhaseManagerMono
    {
        [Space] [Header("Destination Manager Variables")] [Space]
        [SerializeField] private CellAnimator cellReference; //Used for the single purpose of animation
        [SerializeField] private GameObject processingDescriptionObject; 
        [SerializeField] private Button endProcessingButton;
        [SerializeField] private QuizManager quizManager;

        private void OnEnable()
        {
            quizManager.OnComplete += EndPhase;
        }
        
        private void OnDisable()
        {
            quizManager.OnComplete -= EndPhase;
        }

        private void Start()
        {
            processingDescriptionObject.SetActive(true);
            cellReference.SetAnimatorStatus(true);
            PlayAMNQueueTransformation();
        }

        private async void PlayAMNQueueTransformation()
        {
            var time = cellReference.AMNTransformation();
            await UniTask.Delay(Util.ConvertToMili(time / 0.5f));
            endProcessingButton.onClick.AddListener(delegate {processingDescriptionObject.SetActive(false);});
            endProcessingButton.onClick.AddListener(EndPhase);
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}