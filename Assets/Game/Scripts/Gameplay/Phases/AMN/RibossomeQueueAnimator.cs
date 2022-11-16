using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public enum RibossomeState
{
    Sinthetizing,
    HoldingQueue,
    Exiting,
    Leave
}

/*
    This script will put the RNAt in their right position

    Sinthetizing : right
    HoldingQueue: middle
    Exiting: left

    Leave: endTarget before in ribossomeAnimator
*/

namespace Phases.AMN
{
    public class RibossomeQueueAnimator : MonoBehaviour
    {
        [SerializeField] List<Transform> ribossomeStatePosition = default;

        [SerializeField] Vector3 exitRotation = default;

        [SerializeField] Transform sleepRibossome = default;
        //In this case it the component itself
        [SerializeField] Transform ribossomeQueue = default;

        private List<RibossomeLetter> inRibossomeQueue = new List<RibossomeLetter>();

        private bool removeRb; //simply because Async doesn't allow ref

        public async UniTask MoveNewRibossome(bool moveOthers, RibossomeLetter rb, Vector3 rotation,
            float scaleMultiplier, float animationTime, AnimationCurve animationCurve)
        {
            
            List<UniTask> toMove = new List<UniTask>();

            toMove.Add(MoveRibossome(rb, rotation, scaleMultiplier, animationTime, animationCurve));
            
            if (moveOthers)
            {
                toMove.Add(MoveAllRibossome(rotation, animationTime, animationCurve));
            }

            await UniTask.WhenAll(toMove);

            inRibossomeQueue.Add(rb);
        }

        public async UniTask MoveAllRibossome(Vector3 rotation, float animationTime, AnimationCurve animationCurve)
        {

            if (inRibossomeQueue.Count == 0) return;

            List<UniTask> toMove = new List<UniTask>();

            removeRb = false;
            
            for (int i = 0; i < inRibossomeQueue.Count; i++)
            {
                toMove.Add(MoveRibossome(inRibossomeQueue[i],rotation, 1f, animationTime, animationCurve));
            }

            await UniTask.WhenAll(toMove);

            if (removeRb)
            {
                inRibossomeQueue[0].SetStateRib(0);
                inRibossomeQueue.RemoveAt(0);
            }
        }

        private async UniTask MoveRibossome(RibossomeLetter rb, Vector3 rotation, float scaleMultiplier, 
            float animationTime, AnimationCurve animationCurve)
        {
            
            int rbState = rb.GetStateRib();

            if (rb.GetStateRib() == ribossomeStatePosition.Count - 1)
            {
                await MoveTowardState(rb.transform, ribossomeStatePosition[rbState],
                    sleepRibossome,
                    exitRotation, 0,
                    animationTime, animationCurve);
                //Remove from list
                removeRb = true;
            }
            else
            {
                await MoveTowardState(rb.transform, ribossomeStatePosition[rbState],
                    ribossomeQueue,
                    rotation, scaleMultiplier, 
                    animationTime, animationCurve);
            }
            
            rb.IncreaceState();
        }

        private async UniTask MoveTowardState(Transform moveObject, Transform target, Transform newParent, 
            Vector3 rotation, float scaleMultiplier, float time, AnimationCurve animationCurve)
        {

            GameObject moveO = moveObject.gameObject;

            LeanTween.move(moveO, target, time).setEase(animationCurve);
            LeanTween.rotate(moveO, rotation, time - 0.2f);
            LeanTween.scale(moveO.GetComponent<RectTransform>(), 
                moveO.GetComponent<RectTransform>().localScale*scaleMultiplier, time - 0.2f);
            
            await UniTask.Delay(Util.ConvertToMili(time));
            
            //Different from the last commit we don't need to set the sibling index
            //Because when its move his sibling index don't change
            //And being the last it's ok in this case
            
            if (moveObject.parent != newParent)
            {
                moveObject.SetParent(newParent);
            }
        }

        public RibossomeLetter GetRibossomeSinthetizing() => inRibossomeQueue[inRibossomeQueue.Count - 1];

        public Transform GetBallRibossomeSinthetizing()
        {
            Transform rb = GetRibossomeSinthetizing().transform;
            return rb.GetChild(rb.childCount - 1);
        }

        //Just for the case of changing the structure of the game
        public RibossomeLetter GetAllRibossomesSinthetizing() => null;
    }
}
