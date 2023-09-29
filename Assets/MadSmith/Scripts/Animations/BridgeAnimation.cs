using System;
using System.Collections.Generic;
using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.Animations
{
    public class BridgeAnimation : MonoBehaviour
    {
        private List<Animator> _animators = new List<Animator>();
        [SerializeField] private ParticleSystem bridgeFallEffect;
        [SerializeField] private ParticleSystem bridgeFixEffect;
        [SerializeField] private WoodFallingWaterAnimationHandler _woodFallingWaterAnimationHandler;
        [SerializeField] private BridgeInteractableFix[] bridgeInteractableFixes;
        
        private void Awake()
        {
            var a = GetComponentsInChildren<Animator>();
            _animators.AddRange(a);
            foreach (var animator in _animators)
            {
                string name = CurrentAnimationName(animator);
                animator.Play(name,0,0);
                animator.speed = 0;
            }
            bridgeFallEffect.gameObject.SetActive(false);
            bridgeFixEffect.gameObject.SetActive(false);
            foreach (var bridgeInteractableFix in bridgeInteractableFixes)
            {
                bridgeInteractableFix.gameObject.SetActive(false);
            }
        }

        [ContextMenu("Play")]
        public void BridgeFall()
        {
            foreach (var animator in _animators)
            {
                animator.speed = 1;
            }

            bridgeFallEffect.gameObject.SetActive(true);
            bridgeFallEffect.Play();
            foreach (var bridgeInteractableFix in bridgeInteractableFixes)
            {
                bridgeInteractableFix.gameObject.SetActive(true);
            }
        }

        [ContextMenu("Reset")]
        public void ResetBridge()
        {
            bridgeFixEffect.gameObject.SetActive(true);
            bridgeFixEffect.Play();
            Invoke(nameof(FixBridge),3);
            foreach (var bridgeInteractableFix in bridgeInteractableFixes)
            {
                bridgeInteractableFix.gameObject.SetActive(false);
            }
        }

        private void FixBridge()
        {
            foreach (var animator in _animators)
            {
                string name = CurrentAnimationName(animator);
                animator.Play(name,0,0);
                animator.speed = 0;
            }
            

            _woodFallingWaterAnimationHandler.ResetAnimation();
            bridgeFixEffect.gameObject.SetActive(false);
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