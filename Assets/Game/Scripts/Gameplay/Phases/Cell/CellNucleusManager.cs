using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Phases.AMN;
using Phases.Cell.DNA;

/*
    Script responsible for all the actions in the nucleus, as: spawn of the RNA, animation of the RNA
    to the RNASpawner, animation of nucleus expansion, animation of rna going through a hole, 
    cell nucleus contraction

    In the CellNucleus there is RNA, but they are fake, just Letters, because i don't need the entire script
    This makes everything easier

    The cellNucleus is a refatoration of the code present in the RNASpawner
*/

namespace Phases.Cell
{
    public class CellNucleusManager : PhaseManagerMono
    {
        //[SerializeField] GameObject visualDNA = default; //Prototype
        [SerializeField] RNASpawner rnaReference; //Sets the RNA and the RNA sets it
        [SerializeField] DNAManager dnaReference; // The original DNA
        [SerializeField] private int timeCountScale = 1;

        private const string DNAtranscriptionBeg = "TAC"; //Always the beg of the DNA
        private string[] DNAtranscriptionEnd = { "ATT", "ATC", "ACT" }; //The end of the DNA

        private static int separationCharacters = 1;

        private static string DNAString; //Original DNA string of the protein

        private static bool random = false; //Sets if the start is a random protein or not

        private int quantity;
        
        private float _time;
        private ScoreManager.ScoreContext? _completionBonusContext;

        private void Start()
        {
            //Separate the DNA and Expand the nucleus 
            DNAAnimations();

            var countTimeCoroutine = StartCoroutine(CountTime(timeCountScale));
            
            rnaReference.OnComplete = () =>
            {
                StopCoroutine(countTimeCoroutine);

                if (_time <= ScoreManager.Instance.DefaultTimeBonusRequirement)
                {
                    _completionBonusContext = ScoreManager.ScoreContext.InMissionHitBonus;
                }
                else if (_time > ScoreManager.Instance.DefaultTimePenaltyRequirement)
                {
                    _completionBonusContext = ScoreManager.ScoreContext.InMissionMissPenalty;
                }

                if (_completionBonusContext != null)
                {
                    ScoreManager.Instance.UpdateScore((ScoreManager.ScoreContext)_completionBonusContext);
                }

                EndPhase();
            };
        }
        
        private IEnumerator CountTime(int scale = 1)
        {
            while (true)
            {
                var delay = 1f / (10f * scale);
                _time += delay;
                yield return new WaitForSeconds(delay);
            }
        }

        private async void DNAAnimations()
        {
            dnaReference.ChangeSecondHalf();
            await dnaReference.RNAVisibility();
            await dnaReference.DNASeparation();
            rnaReference.gameObject.SetActive(true);
        }

        public void SetStructure()
        {
            quantity = AMNManager.GetNumberOfAMN() * AMNManager.GetSizeAMN();

            dnaReference.SetQuantity(quantity);

            string firstCut = CutDNAString();
            string nonUsableCharacter = rnaReference.GetNonUsableCharacter();

            rnaReference.SetQuantity(quantity);
            string endingString = rnaReference.InstantiateAllRNABasedOnDNA(firstCut); //Just to set it first
            dnaReference.SetSeparationInDNAStructure(separationCharacters);
            rnaReference.SetQuantity(quantity + endingString.Length + DNAtranscriptionBeg.Length);

            dnaReference.SetFiniteDNAString(DNAtranscriptionBeg + firstCut + endingString);//Puts sliced DNA on DNA
           
            dnaReference.SetupStructure(DNAtranscriptionBeg.Length, DNAtranscriptionBeg, true); //Instantiate beginning
            SpawnDot(DNAtranscriptionBeg.Length, nonUsableCharacter);
            dnaReference.SetupStructure(quantity, firstCut, true); //Instantiate sliced part of the DNA
            SpawnDot(DNAtranscriptionBeg.Length + quantity + separationCharacters, nonUsableCharacter);
            dnaReference.SetupStructure(endingString.Length, endingString, true); //Instantiate ending

            dnaReference.ChangeSecondHalf();
        }

        private void SpawnDot(int start, string nonUsableCharacter)
        {
            dnaReference.SetupStructure(separationCharacters, nonUsableCharacter, false);

            for (int i = 0; i < separationCharacters; i++)
            {
                //Dots
                dnaReference.ChangeRNAinDNAStructureByChildPosition(start + i, nonUsableCharacter);
                dnaReference.ChangeSecondHalf(start + i, nonUsableCharacter);
            }
        }
        
        public void ChangeRNAinDNAStructure(int index, string text)
        {
            dnaReference.ChangeRNAinDNAStructure(index, text);
        }

        public static int GetNumberOfCharacterInBeginning()
        {
            return separationCharacters;
        }

        public static int GetNumberOfCharacterToEnd()
        {
            return separationCharacters;
        }

        public void ChangeDNAStructure(string cut)
        {
            dnaReference.ChangeFirstHalf(cut);
        }

        //Need to do all the animation of the game
        public new void EndPhase()
        {
            base.EndPhase();
        }

        public string CutDNAString()
        {
            string sub;

            do {
                do {
                    //Cuts a part of the DNA to make the substring
                    sub = Util.RandomSubString(DNAString, quantity, 0, (DNAString.Length - quantity));
                } while(Util.FindOccurrence(sub, DNAtranscriptionEnd, AMNManager.GetSizeAMN()));
            } while (!DNAWithAllBases(sub, rnaReference.GetDictionaryKeysCount()));
            //Tests if it have at least one of all the bases (A,T,C,G)

            return sub;
        }

        public string GetAEndingString()
        {
            return DNAtranscriptionEnd[UnityEngine.Random.Range(0, DNAtranscriptionBeg.Length)];
        }

        public string GetBeginningString()
        {
            return DNAtranscriptionBeg;
        }

        private bool DNAWithAllBases(string cut, int number)
        {
            int i;
            List<char> bases = new List<char>();

            for (i = 0; i < cut.Length; i++)
            {
                if (!bases.Contains(cut[i]))
                {
                    bases.Add(cut[i]);
                    if (bases.Count == number)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetRandom(bool state)
        {
            random = state;
        }

        public static void SetDNAString(string proteinDNA)
        {
            DNAString = proteinDNA;
        }
    }
}
