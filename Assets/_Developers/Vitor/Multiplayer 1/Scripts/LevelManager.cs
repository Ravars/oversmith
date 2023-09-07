using System;
using System.Linq;
using Mirror;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer_1.Scripts
{
    public class LevelManager : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;
        private HelloWorldNetworkManager _manager;
        private HelloWorldNetworkManager Manager
        {
            get
            {
                if (_manager != null) return _manager;
                return _manager = NetworkManager.singleton as HelloWorldNetworkManager;
            }
        }
        public void CountdownEnded() // Used by animation event
        {
            animator.enabled = false;
        }

        #region Server
        private void Start()
        {
            CheckToStartRound();
        }

        [ServerCallback]
        public void StartRound()  // Used by animation event
        {
            RpcStartRound();
        }
        [Server]
        private void CheckToStartRound()
        {
            animator.enabled = true;
            RpcStartCountDown();
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcStartCountDown()
        {
            animator.enabled = true;
        }
        
        [ClientRpc]
        private void RpcStartRound()
        {
            Manager.EnableMovement();
        }

        #endregion
    }
}