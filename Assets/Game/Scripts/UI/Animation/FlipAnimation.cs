using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Animation
{
    public class FlipAnimation : AnimatorUser, SpriteAnimator
    {
        [SerializeField] int qtdSides = 2; //This basically allows a dice flip animation, in a child script
        Queue<Image> sideSpriteRenderers = new();
        private bool flipAnimationState = true; //True to not change in the first "seek"

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < qtdSides; i++)
            {
                sideSpriteRenderers.Enqueue(transform.GetChild(i).GetComponent<Image>());
            }
        }

        public new void DoSpriteChangeAnimation(Sprite sprite)
        {
            var renderer = sideSpriteRenderers.Dequeue();
            renderer.sprite = sprite;

            sideSpriteRenderers.Enqueue(renderer);

            flipAnimationState = !flipAnimationState;
            SetAnimationState("Flip", flipAnimationState);
        }
    }
}