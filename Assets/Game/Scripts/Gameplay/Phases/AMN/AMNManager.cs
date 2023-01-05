using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Phases.AMN
{
    //Before it was part of the InputPhase childs, but its not needed anymore
    public class AMNManager : MonoBehaviour
    {
        public System.Action OnComplete { set; get; }
        
        [System.Serializable]
        private class AMNDescriber
        {
            public AMN value; //A,G,U,C
            public Sprite table = null; //One quarter of the amino acids circle
        }

        [Space] [Header("AMN Manager Atributes")] [Space]

        [SerializeField] List<AMNDescriber> basics; //The "tables" and their answers
        [SerializeField] AMNTableHolder tableHolder = null;

        private static int numberOfAMN = 7; 
        private int actualCompleted = 0; 

        private static int sizeAMN = 3;
        private const int ribossomeMaxNumber = 3; //Number of ribosome in the transcription
        private bool animationPause = false; //Used in the VerifyAMN to pause the game

        private static string RNAtoAMN; //Sub product of the RNASpawner completion
        private int indexOfRNA = 0; //RNA control of the position

        private string nameAMN; //Change in every input
        
        [SerializeField] GameObject visualAMN = default; //Where all the tables, the ribosome and the transporter will be
        [SerializeField] AMNQueue completedAMNQueue = default; //Push a AMN when its ends
        [SerializeField] AMNSpawner amnLetterQueue = default;

        private void Start()
        {
            // FindObjectOfType<GameplayManager>().ShowInstructionReminder();
            visualAMN.SetActive(true);
            Util.ChangeAlphaCanvasImageAnimation(visualAMN.GetComponent<CanvasGroup>(), 1f, 1f);
            SetAllRibossome();

            SpawnAMN();
            SetSearchAMN();
        }

        private void SpawnAMN()
        {
            amnLetterQueue.SpawnAMN(RNAtoAMN);
        }

        private float SetAMN(bool lastOne)
        {
            SetSearchAMN();
            return amnLetterQueue.NextAMN(lastOne);
        }

        private void SetSearchAMN()
        {
            int i;
            string RNAstring = "";

            for (i = 0; i < sizeAMN; i++)
            {
                RNAstring += RNAtoAMN[indexOfRNA];
                indexOfRNA++;
            }

            SearchAMN(RNAstring);
        }

        private async void SetAllRibossome()
        {
            animationPause = true;
            await completedAMNQueue.SetAllRibossomeEnter(ribossomeMaxNumber);
            animationPause = false;
        }

        public static void SetRNAtoAMNString(string RNA)
        {
            AMNManager.RNAtoAMN = RNA;
        }

        private void SearchAMN(string RNAstring)
        {
            int i = 0;
            AMN perc;
            AMNDescriber hold;
            
            hold = basics.Find(x => x.value.GetValue() == RNAstring[i].ToString()); 

            perc = hold.value;

            if (hold == null || !hold.table)
            {
                print("WTF AMN " + hold.value.GetValue());
                return;
            }

            tableHolder.ChangeTable(hold.table);

            for (i = 1; i < sizeAMN; i++)
            {
                perc = perc.GetNexts().Find(x => {
                    return System.Array.Find(x.GetValue().Split(','), y => {
                        return y == RNAstring[i].ToString();
                    }) != null;
                });
            }


            nameAMN = perc.GetAMN(0).GetValue().ToUpper();
            print(nameAMN);
        }

        public bool VerifyAMN(string AMN) => (AMN.ToUpper() == nameAMN.ToUpper());

        public async UniTask PushNewAMN(string AMN)
        {
            await WaitAnimationFlow();

            actualCompleted++;
            await QueueNewAMN(AMN); //Push AMN to queue and Ribossome animation

            if (actualCompleted < numberOfAMN + 1) return;
            
            ThrownLastRibossome();
            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }

        private async UniTask QueueNewAMN(string amnName)
        {
            animationPause = true;
            UniTask[] UniTaskAnimation = new UniTask[2]; //All animation of the object

            UniTaskAnimation[0] = completedAMNQueue.NewAMNInLine(
                (actualCompleted) >= (numberOfAMN), 
                (actualCompleted) < (numberOfAMN + 1),
                (actualCompleted + ribossomeMaxNumber - 1).ToString(), amnName);
            
            //Move the string to the left
            if (actualCompleted <= numberOfAMN)
            {
                UniTaskAnimation[1] = UniTask.Delay(Util.ConvertToMili(SetAMN(actualCompleted == numberOfAMN)));
            }
            else
            {
                UniTaskAnimation[1] = UniTask.Delay(0);
            }

            await UniTask.WhenAll(UniTaskAnimation);

            animationPause = false;
        }

        private async UniTask WaitAnimationFlow()
        {
            while (animationPause)
            {
                await UniTask.Yield();
            }
        }

        private async void ThrownLastRibossome()
        {
            await completedAMNQueue.NewAMNInLine(
                (actualCompleted) >= (numberOfAMN), 
                (actualCompleted) < (numberOfAMN + 1),
                (actualCompleted + ribossomeMaxNumber - 1).ToString(), "None");
        }

        public static int GetSizeAMN()
        {
            return sizeAMN;
        }

        public static int GetNumberOfAMN()
        {
            return numberOfAMN;
        }

        public static void SetNumberOfAMN(int newNumberOfAMN)
        {
            numberOfAMN = newNumberOfAMN;
        }

    }
}