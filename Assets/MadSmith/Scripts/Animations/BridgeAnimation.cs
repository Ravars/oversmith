using System;
using System.Collections.Generic;
using UnityEngine;

namespace MadSmith.Scripts.Animations
{
    public class BridgeAnimation : MonoBehaviour
    {
        private List<Animator> _animators = new List<Animator>();

        private void Awake()
        {
            var a = GetComponentsInChildren<Animator>();
            _animators.AddRange(a);
            ResetBridge();
        }

        [ContextMenu("Play")]
        public void BridgeFall()
        {
            foreach (var animator in _animators)
            {
                animator.speed = 1;
            }
        }

        [ContextMenu("Reset")]
        public void ResetBridge()
        {
            foreach (var animator in _animators)
            {
                string name = CurrentAnimationName(animator);
                animator.Play(name,0,0);
                animator.speed = 0;
            }
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