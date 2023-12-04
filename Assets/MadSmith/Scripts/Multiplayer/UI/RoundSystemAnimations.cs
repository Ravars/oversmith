using MadSmith.Scripts.Multiplayer.Managers;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.UI
{
    public class RoundSystemAnimations : MonoBehaviour
    {
        private RoundSystem _roundSystem;

        private void Awake()
        {
            _roundSystem = GetComponentInParent<RoundSystem>();
        }

        public void StartRound()
        {
            // //Debug.Log("Start round");
            // _levelManager.StartRoundAnimation();
            _roundSystem.StartRound();
        }
        public void CountdownEnded()
        {
            _roundSystem.CountdownEnded();
            //Debug.Log("CountdownEnded");
            // _levelManager.CountdownEnded();
        }
    }
}