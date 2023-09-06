using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Phases.AMN;

/*
    Spawn all the RNA, based on the original DNA string of the protein
    Has a reference to the NucleusManager because the Nucleus Manager should be the original
    Every visual change in the RNA will be reflect in the Nucleus

    Before string
        sub = DNAtranscriptionBeg + DNA + DNAtranscriptionEnd[Random.Range(0 , DNAtranscriptionEnd.Length)];

    Now
        sub = DNA + DNAtranscriptionEnd[Random.Range(0 , DNAtranscriptionEnd.Length)];
*/

namespace Phases.Cell
{
    public class RNASpawner : InputPhase
    {
        public System.Action OnComplete { set; get; }

        [Space]
        [Header("RNA Manager Atributes")]
        [Space]
        [SerializeField] CellNucleusManager originalPlace; //Where the RNA starts (the nucleus)

        private int quantity; //Needs to be multiple of 3, and it will because of the AMN

        [SerializeField] RNA prefab;
        [SerializeField] TextWithInput middleRNA;
        [SerializeField] string nonUsableCharacter = "-";

        [SerializeField] Transform RNASpawn;
        
        [Space]
        [Header("Colors used in the lights")]
        [Space]
        //Color for the player input
        [SerializeField] Color defColor;
        [SerializeField] Color whenRight;
        [SerializeField] Color whenWrong;
        
        private Dictionary<string, string> validationDNARNA = new()
        {
            {"A", "U"},
            {"T", "A"},
            {"C", "G"},
            {"G", "C"}
        }; //DNA to RNA Correspondence

        private string[] answers; //Save of the answers of the RNA given by the player
        private int nextPhase = 0;
        
        public int WrongAnswersStrike { get; set; }
        public int RightAnswersStrike { get; set; }

        private void Start()
        {
            answers = new string[quantity];
            SetInputOperation();
        }

        private void InstantiateRNAUsingString(int actualNumber, string dataString, TextWithInput prefabT)
        {
            for (int i = 0 ; i < dataString.Length ; i++)
            {
                var hold = Instantiate(prefabT, RNASpawn);
                
                if (hold is RNA rna)
                {
                    rna.Spawner = this;
                }
                
                hold.SetPosition(actualNumber + i); //Puts its original position, so i can build the "replic" vector
                hold.Setup(dataString[i].ToString());
            }
        }

        private string InstantiateAllRNABasedOnDNA()
        {
            return InstantiateAllRNABasedOnDNA(originalPlace.CutDNAString());
        }

        //Get the additional information
        public string InstantiateAllRNABasedOnDNA(string sub)
        {
            string ending = originalPlace.GetAEndingString();
            string beginning = originalPlace.GetBeginningString();

            string separationString = string.Concat(Enumerable.Repeat(
                    nonUsableCharacter, CellNucleusManager.GetNumberOfCharacterToEnd()));
            
            SetInputData(RNASpawn); //Protected function of all the InputPhase manager

            InstantiateRNAUsingString(0, beginning, prefab);

            //Instantiate beginning dot
            InstantiateRNAUsingString(-CellNucleusManager.GetNumberOfCharacterToEnd(), 
                separationString, middleRNA);

            InstantiateRNAUsingString(beginning.Length, sub, prefab);

            //Instantiate end dot
            InstantiateRNAUsingString(-CellNucleusManager.GetNumberOfCharacterToEnd(), 
                separationString, middleRNA);
            
            //Instantiate one of the ending DNA string
            InstantiateRNAUsingString(quantity + beginning.Length,ending, prefab);

            return ending;
        }

        private void DestroyAllRNA()
        {
            foreach (Transform child in RNASpawn)
            {
                Destroy(child.gameObject);
            }
        }

        public void ResetActualValuesInRNA()
        {
            RNA hold;
            foreach (Transform child in RNASpawn)
            {
                hold = child.GetComponent<RNA>();
                if (GetValueValidation(hold.GetValue()) == hold.GetValueInputText())
                {
                    hold.SetValue("", defColor);
                    nextPhase--;
                }
            }
        }

        public void ChangeQuantityToNextPhase(int increase)
        {
            nextPhase += increase;
            
            //To end the game when the player filled everything
            if (nextPhase < quantity) return;

            //Here its change phases
            AMNManager.SetRNAtoAMNString(Util.ConvertToString(answers));
            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }

        public void SetCorrespondentValidation(int index, string value)
        {
            answers[index] = value; //Look RNA original position
            originalPlace.ChangeRNAinDNAStructure(index, value);
        }

        public void SetQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public Color GetColorDef()
        {
            return defColor;
        }

        public Color GetColorRight()
        {
            return whenRight;
        }

        public Color GetColorWrong()
        {
            return whenWrong;
        }

        public string GetValueValidation(string keyPas)
        {
            return validationDNARNA[keyPas];
        }

        public int GetValidationCount()
        {
            return validationDNARNA.Count;
        }

        public string GetKeyByIndex(int index)
        {
            return validationDNARNA.Keys.ElementAt(index);
        }

        public string[] GetDictionaryKeys()
        {
            return validationDNARNA.Keys.ToArray();
        }

        public int GetDictionaryKeysCount()
        {
            return validationDNARNA.Keys.Count;
        }

        public string GetNonUsableCharacter()
        {
            return this.nonUsableCharacter;
        }
    }
}