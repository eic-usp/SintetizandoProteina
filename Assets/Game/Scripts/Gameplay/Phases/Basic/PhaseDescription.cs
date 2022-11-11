using UnityEngine;

/*
    Basic information of the phase
    See Wait->WaitManager or Wait->MissionManager
*/

namespace Phases{

    [System.Serializable]
    public class PhaseDescription{
        [SerializeField] string namePhase;
        [SerializeField] string descriptionPhase;
        [SerializeField] GameObject additionalInfo;

        public string GetName(){
            return this.namePhase;
        }

        public string GetDescription(){
            return this.descriptionPhase;
        }

        public GameObject GetAdditionalInfo(){
            return this.additionalInfo;
        }
    }  
}

