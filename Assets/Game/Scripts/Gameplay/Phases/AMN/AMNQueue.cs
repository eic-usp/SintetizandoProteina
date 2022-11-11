using System;
using Cysharp.Threading.Tasks;

using UnityEngine;

using GameUserInterface.Text;

/*
    Manages the AMNQueue and all its animations

    Changes the Action depending on the actual stage of the cell
*/

namespace Phases.AMN{
    public class AMNQueue : MonoBehaviour{
        private Color actualColor = new Color(0f, 0f, 0f);
        [SerializeField] Letter amnPrefab = default;
        [SerializeField] float movementYDistance = 65f;

        bool firstOne = true;

        [Space]
        [Header ("To use exclusivily in the Ribossome Animations")]
        [Space]

        [SerializeField] RibossomeAnimator transporterList = default;

        private int actualAMNNumber = 0; //Correct one
        //private int actualAMNNumber = 1; //Used in tests

        Func<int, UniTask> moveAction;

        public async UniTask NewAMNInLine(bool lastOnes, bool newRb, string amnNumber, string amnName){
            if(newRb){
               moveAction = async act => {
                    await PushNewAMN(transporterList.GetSinthetizingFromQueue(), amnName);
                }; 
            }else{
                moveAction = async act => {
                    await UniTask.Yield();
                };
            }

            actualAMNNumber++;
            actualColor = Util.CreateNewDifferentColor(actualColor);
            
            await transporterList.RibossomeExit(!lastOnes, actualColor, (actualAMNNumber + 1), moveAction);
        }

        public async UniTask SetAllRibossomeEnter(int ribossomeMaxNumber){
            transporterList.SetChildPrefab(amnPrefab.gameObject);
            transporterList.SetPool(ribossomeMaxNumber);

            actualColor = Util.CreateNewDifferentColor(actualColor);
            await transporterList.RibossomeEnter(actualColor, (actualAMNNumber + 1).ToString(), false);

            await UniTask.Yield();
        }

        public async UniTask PushNewAMN(Transform sinthetizing, string amnName){
            float animationTime = transporterList.GetAnimationsTime();

            RectTransform fatherPosition = this.transform.parent.GetComponent<RectTransform>();
            Vector3 saveInitial = fatherPosition.anchoredPosition;
            Transform newAMN = sinthetizing.GetChild(sinthetizing.childCount - 1);

            Vector3 newPosition = new Vector3(
                saveInitial.x,
                saveInitial.y + movementYDistance, 
                saveInitial.z);

            LeanTween.move(fatherPosition, newPosition , 1f);
            
            SetVisibleGroupName(newAMN.GetComponent<AMNLetter>(), amnName, animationTime);
            await UniTask.Delay(Util.ConvertToMili(animationTime));    
            
            newAMN.SetParent(this.transform);
        }

        private void SetVisibleGroupName(AMNLetter amnLetter, string amnName, float time){
            CanvasGroup cG = amnLetter.GetAMNGroupName();
            amnLetter.SetupAMNName(amnName);            

            Util.ChangeAlphaCanvasImageAnimation(cG, 1f, time);
        }

        public void SetFirstOne(bool state){
            firstOne = state;
        }
    }
}
