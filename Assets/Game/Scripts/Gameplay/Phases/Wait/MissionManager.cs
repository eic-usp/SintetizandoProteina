using System;
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
        private Action _action;

        public void Refresh()
        {
            _action?.Invoke();
        }
        public void Setup(int numberPhase, string missionName, string missionDescription, GameObject information)
        {
            _action = () => Setup(numberPhase, missionName, missionDescription, information);
            
            FinishedInstructions = false;
            this.missionName.text = LocalizationManager.FindStringReference(LocalizationManager.Mission, 
                mission).Value + "  " + (numberPhase + 1) + "  (" + LocalizationManager.FindStringReference(LocalizationManager.Mission, 
                missionName).Value  + ") ";
            //this.missionName.text = mission + "  " + (numberPhase + 1) + "  (" + missionName + ") ";
            
            this.missionDescription.text = LocalizationManager.FindStringReference(LocalizationManager.Mission, 
                missionDescription).Value;
            
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