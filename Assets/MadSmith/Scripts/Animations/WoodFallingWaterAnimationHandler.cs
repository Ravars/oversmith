using System;
using UnityEngine;

namespace MadSmith.Scripts.Animations
{
    public class WoodFallingWaterAnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        [SerializeField] private BridgeAnimation _bridgeAnimation;
        [SerializeField] private float timeToFirst = 40;
        [SerializeField] private float timeToRepeat = 40;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.speed = 0;
            Invoke(nameof(PlayAnimation),timeToFirst);
            
        }

        private void PlayAnimation()
        {
            string currentAnimationName = CurrentAnimationName(_animator);
            _animator.Play(currentAnimationName,0,0);
            _animator.speed = 1;
        }

        public void HandleAnimation() //Animation event
        {
            _bridgeAnimation.BridgeFall();
        }

        public void ResetAnimation()
        {
            string currentAnimationName = CurrentAnimationName(_animator);
            _animator.Play(currentAnimationName,0,0);
            _animator.speed = 0;
            Invoke(nameof(PlayAnimation),timeToRepeat);
        }
        private string CurrentAnimationName(Animator anim)
        {
            var currAnimName = "";
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips) {
                if (anim.GetCurrentAnimatorStateInfo (0).IsName (clip.name)) {
                    currAnimName = clip.name.ToString();
                }
            }

            return currAnimName;
        }
    }
}