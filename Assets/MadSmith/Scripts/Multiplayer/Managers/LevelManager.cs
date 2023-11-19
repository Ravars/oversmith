using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class LevelManager : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;
        private MadSmithNetworkManager _manager;
        private MadSmithNetworkManager Manager
        {
            get
            {
                if (_manager != null) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
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
        public void StartRoundAnimation()  // Used by animation event
        {
            Debug.Log("StartRoundAnimation");
            RpcStartRound();
            // CmdSpawnOrderManager();
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
            Debug.Log("RpcStartRound");
            Manager.EnableMovement();
        }

        // [Command]
        // private void CmdSpawnOrderManager()
        // {
        //     Manager.SpawnOrderManager();
        // }

        #endregion
    }
}