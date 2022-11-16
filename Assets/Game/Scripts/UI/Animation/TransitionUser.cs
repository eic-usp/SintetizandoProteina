using UnityEngine;

/*
    This make the SelectionManager shorter, so it's a good idea
*/

namespace UI.Animation
{
    public class TransitionUser : MonoBehaviour
    {
        [Space] [Header("Transition User Variables")] [Space]
        [SerializeField] TransitionController tC = default;
        [SerializeField] TransitionType transition = default;

        public int PlayTransitionIn() => Util.ConvertToMili(tC.PlayTransitionIn(transition));

        public int PlayTransitionOut() => Util.ConvertToMili(tC.PlayTransitionOut(transition));

        public int GetBetweenTransitionTime() => Util.ConvertToMili(tC.GetBetweenTransition());
    }
}