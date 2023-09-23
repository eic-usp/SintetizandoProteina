using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UI.Animation;

/*
    Used in the first part of the game
    It control the bolts of selection
    First screem of the game

    Partition Father 
*/

namespace UI.Menu
{
    using Input = UnityEngine.Input;
    
    public class SelectionManager : TransitionUser
    {
        [Space] [Header ("Bolt")] [Space]

        [SerializeField] BoltExecuter boltToSelection = default; //Strategy to use when instantiating bolts

        private Transform childRef; //Get child of this transform

        private int actualSelected; //Bolt
        private int childQTD; //Qtd of childs in this transform

        [Space] [Header ("Partition of the game")] [Space]

        [SerializeField] Transform partitionsFather;

        GameObject father; //Father of this transform

        private void OnEnable()
        {
            StartCoroutine(ChangeBolt());
        }

        private void Start()
        {
            childQTD = this.gameObject.transform.childCount;
            father = this.transform.parent.gameObject;

            actualSelected = 0;
            PositioningBoltInstantiating();
        }

        private void PositioningBolt(int increment)
        {
            DestroyBolt(childRef);
            actualSelected += increment;
            PositioningBoltInstantiating();
        }

        private void PositioningBoltInstantiating()
        {
            childRef = this.transform.GetChild(actualSelected);
            boltToSelection.InstantiateBolt(childRef);
            //Sound when "change bolts"
        }

        public void PositioningBoltByIndex(int index)
        {
            DestroyBolt(childRef);
            actualSelected = index;
            PositioningBoltInstantiating();
        }
        
        private void DestroyBolt(Transform origin)
        {
            for (int i = origin.childCount - 1; i > -1; i--)
            {
                GameObject.Destroy(origin.GetChild(i).gameObject);
            }
        }

        private IEnumerator ChangeBolt()
        {
            int inp = 0;

            while (true)
            {
                inp = (int) (Input.GetAxisRaw("Vertical") * -1);

                if (inp != 0)
                {
                    if (actualSelected + inp > -1 && actualSelected + inp < childQTD )
                    {
                        PositioningBolt(inp);
                    }

                    yield return new WaitForSeconds(0.2f);
                }

                yield return null;
            }
        }

        public async void ChangePartitionByIndex(int index)
        {
            GameObject hold = partitionsFather.GetChild(index).gameObject;

            await UniTask.Delay(PlayTransitionIn());

            father.SetActive(!father.activeSelf);
            hold.SetActive(!hold.activeSelf);

            await UniTask.Delay(GetBetweenTransitionTime());
            await UniTask.Delay(PlayTransitionOut());

            actualSelected = index; //Just to be sure
            
            //Sound when "clicked"
        }

        public void BackToMenu() {}

        public void Quit()
        {
            //Sound when "exit"    
            Application.Quit();
        }
    }
}