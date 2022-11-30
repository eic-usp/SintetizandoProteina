using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phases.Cell
{
    public class FindNucleusManager : PhaseManagerMono
    {
        // [Space]
        // [Header("Find Nucleus Manager Attributes")]
        // [Space]

        public void SetFound()
        {
            EndPhase();
        }

        public new void EndPhase()
        {
            base.EndPhase();
        }
    }
}