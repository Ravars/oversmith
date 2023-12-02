using MadSmith.Scripts.Multiplayer.Managers;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.UI
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
            //Debug.Log("Start round");
            _levelManager.StartRoundAnimation();
        }
        public void CountdownEnded()
        {
            //Debug.Log("CountdownEnded");
            _levelManager.CountdownEnded();
        }
    }
}