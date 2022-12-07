using UnityEngine;

namespace Phases.Cell
{
    public class FindNucleusManager : PhaseManagerMono
    {
        [Space] [Header("Find Nucleus Manager Attributes")] [Space]
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private Wait.MissionManager missionManager;

        public void SetFound()
        {
            if (gameplayManager.GetCurrentPhase() > 0 || !missionManager.FinishedInstructions) return;
            EndPhase();
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}