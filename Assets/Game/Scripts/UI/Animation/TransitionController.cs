using System.Collections.Generic;
using UnityEngine;

namespace UI.Animation
{
    //This object do not await, the other object will await
    
    public enum TransitionType
    {
        Falling,
        CloseOfCurtains
    }

    public class TransitionController : AnimatorUser
    {
        [System.Serializable]
        private class Transition
        {
            public float animationTimeIn = default;
            public float animationTimeOut = default;
        }

        float betweenTransition = 0.05f;

        [SerializeField] List<Transition> videos = default;
        [SerializeField] GameObject painelClickBlock = default;

        //[SerializeField] Animator myAnimator = default;
        [SerializeField] RectTransform transitionScreen = default;
        [SerializeField] float animationTime = default;

        //Black to invisible
        public int PlayTransitionFadeIn()
        {
            Util.ChangeAlphaImageAnimation(transitionScreen, 0f, animationTime);
            return Util.ConvertToMili(animationTime);
        }

        //Not used yet
        public int PlayTransitionFadeOut()
        {
            Util.ChangeAlphaImageAnimation(transitionScreen, 1f, animationTime);
            return Util.ConvertToMili(animationTime);
        }

        public void DisableTransition()
        {
            transitionScreen.gameObject.SetActive(false);
        }

        public void EnableTransition()
        {
            transitionScreen.gameObject.SetActive(true);
        }

        public float PlayTransitionIn(TransitionType type)
        {
            SetAnimationState(type.ToString(), true);
            painelClickBlock.SetActive(true);
            return videos[(int) type].animationTimeIn;
        }

        public float PlayTransitionOut(TransitionType type)
        {
            SetAnimationState(type.ToString(), false);
            painelClickBlock.SetActive(false);
            return videos[(int) type].animationTimeOut;
        }

        public float GetBetweenTransition() => betweenTransition;
    }
}
