using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Phases.Cell
{
    public class FindNucleusManager : PhaseManagerMono
    {
        [Space] [Header("Find Nucleus Manager Attributes")] [Space]
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private Wait.MissionManager missionManager;
        [SerializeField] DNA.DNAManager dnaManager;

        public async void SetFound()
        {
            if (gameplayManager.GetCurrentPhase() > 0 || !missionManager.FinishedInstructions) return;
            await dnaManager.DNANucleusVisibility(true);
            EndPhase();
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}