using System.Collections.Generic;
using UnityEngine;
using Phases.Cell;
using GeneralScripts.Player;

/*
    Part visual of the project, you need to set a protein in the scene
    Then set his parent, this serves to identify his index in the videos of the VideoController
    Set proteinName to be used in the json
    Set synthesizedProteinName to be used in a field in the Gameplay

    After all its used the proteinName to get the DNA string from a json that holds all the DNA strings
*/

namespace UI.Protein.Info
{
    public class Protein : MonoBehaviour
    {

        [SerializeField] private Transform maxParent;

        private static VideoController _videoController;

        [SerializeField] private ProteinDescription proteinDescription; //Using it instead of JSON

        [SerializeField] private string proteinName; //Could use the name of the gameObject, used in json
        [SerializeField] private string synthesizedProteinName; //Different from the protein name

        [System.Serializable]
        public class ProteinDeclaration
        {
            public string name;  
            public string value;
        }
        
        [System.Serializable]
        public class PD
        {
            public List<ProteinDeclaration> proteinValues;
        }

        public static void Setup(VideoController vc)
        {
            _videoController = vc;
        }
        
        public void OnClickSendVideo()
        {
            Audio.AudioManager.Instance.StopMusic();
            _videoController.ChooseProtein(maxParent.GetSiblingIndex());
            CellNucleusManager.SetDNAString(proteinDescription.proteinDNA); //Sends the protein to the gameplay
            FindObjectOfType<PlayerInfo>().SetProteinDisplayName(synthesizedProteinName);
            FindObjectOfType<PlayerInfo>().SetProteinName(proteinName);
            SynthesizingProteinShow.SetProtein(proteinDescription); //Send the info to be seen in the gameplay
        }
    }
}