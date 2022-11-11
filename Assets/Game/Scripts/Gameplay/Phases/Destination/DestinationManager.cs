using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using Phases.RNA.DNA;

namespace Phases.Destination
{
    public class DestinationManager : PhaseManagerMono
    {
        [Space] [Header("Destination Manager Variables")] [Space]
        [SerializeField] private CellAnimator cellReference; //Used for the single purpose of animation
        
        [SerializeField] private GameObject destinationDescriptionObject; 
        [SerializeField] private Button endDestinationButton;

        private void Start()
        {
            cellReference.SetAnimatorStatus(true);
            PlayAMNQueueTransformation();
        }

        private async void PlayAMNQueueTransformation()
        {
            var time = cellReference.AMNTransformation();
            await UniTask.Delay(Util.ConvertToMili(time / 0.5f));
            endDestinationButton.onClick.AddListener(delegate {destinationDescriptionObject.SetActive(false);});
            endDestinationButton.onClick.AddListener(EndPhase);
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }

    }
}