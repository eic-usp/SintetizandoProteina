using System.Collections;
using UnityEngine;

namespace UI.Animation
{
    public class AnimatorUser : MonoBehaviour
    {
        // Start is called before the first frame update
        protected Animator myAnimator;

        protected virtual void Awake()
        {
            myAnimator = this.gameObject.GetComponent<Animator>();
        }

        protected IEnumerator WaitForSecondsAnimation(float time)
        {
            yield return new WaitForSeconds(time);
        }

        protected void SetAnimationState(string animationIdentifier, bool animationState)
        {
            myAnimator.SetBool(animationIdentifier, animationState);
        }
    }
}