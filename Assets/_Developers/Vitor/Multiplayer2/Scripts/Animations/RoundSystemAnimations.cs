using System;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer2.Scripts.Animations
{
    public class RoundSystemAnimations : MonoBehaviour
    {
        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = GetComponentInParent<LevelManager>();
        }

        public void StartRound()
        {
            Debug.Log("Start round");
            _levelManager.StartRoundAnimation();
        }
        public void CountdownEnded()
        {
            Debug.Log("CountdownEnded");
            _levelManager.CountdownEnded();
        }
        
    }
}