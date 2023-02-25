using UnityEngine;
using TMPro;

/*Appear between phases of the Gameplay*/

namespace Phases.Wait
{
    public class MissionManager : WaitManager
    {
        [SerializeField] private string mission;
        [SerializeField] private TextMeshProUGUI missionName;
        [SerializeField] private TextMeshProUGUI missionDescription;
        [SerializeField] private Transform additionalInformation;

        public bool FinishedInstructions { get; private set; }

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

            Instantiate(information, additionalInformation);
        }

        //Ok not to be async
        public void OnClickUnBlock()
        {
            FinishedInstructions = true;
            WaitCheck(); //Protected function
        }
    }
}