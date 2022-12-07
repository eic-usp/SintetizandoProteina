using UnityEngine;
using TMPro;

/*Appear between phases of the Gameplay*/

namespace Phases.Wait
{
    public class MissionManager : WaitManager
    {
        [SerializeField] string mission;
        [SerializeField] TextMeshProUGUI missionName;
        [SerializeField] TextMeshProUGUI missionDescription;
        [SerializeField] Transform additionalInformation;

        public bool FinishedInstructions { get; set; }

        public void Setup(int numberPhase, string missionName, string missionDescription, GameObject information)
        {
            FinishedInstructions = false;
            this.missionName.text = mission + "  " + (numberPhase + 1) + "  (" + missionName + ") ";
            this.missionDescription.text = missionDescription;
            SetNumberPhase(numberPhase);

            if (information == null || additionalInformation == null)
            {
                return;
            }

            Instantiate<GameObject>(information, this.additionalInformation);
        }

        //Ok not to be async
        public void OnClickUnBlock()
        {
            FinishedInstructions = true;
            WaitCheck(); //Protected function
        }
    }
}