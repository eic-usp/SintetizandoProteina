using UnityEngine;

namespace Phases.Cell
{
    public class FindNucleusManager : PhaseManagerMono
    {
        [Space] [Header("Find Nucleus Manager Attributes")] [Space]
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private Wait.MissionManager missionManager;
        [SerializeField] private DNA.DNAManager dnaManager;
        
        private bool _found;

        public async void SetFound()
        {
            if (gameplayManager.GetCurrentPhase() > 0 || !missionManager.FinishedInstructions || _found) return;
            _found = true;
            await dnaManager.DNANucleusVisibility(true);
            EndPhase();
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}