using System.Linq;
using Mirror;
using UnityEditor.Localization.Plugins.XLIFF.V12;
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

        public override void OnStartServer()
        {
            Debug.Log("OnStartServer Level Manager");
            HelloWorldNetworkManager.OnServerStopped += CleanUpServer;
            HelloWorldNetworkManager.OnServerReadied += CheckToStartRound;
        }
        [ServerCallback]
        private void OnDestroy() => CleanUpServer();

        [Server]
        private void CleanUpServer()
        {
            HelloWorldNetworkManager.OnServerStopped -= CleanUpServer;
            HelloWorldNetworkManager.OnServerReadied -= CheckToStartRound;
        }

        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }
        [Server]
        private void CheckToStartRound(NetworkConnection obj)
        {
            Debug.Log("Manager.gamePlayers.Count(x => x.connectionToClient.isReady): " + Manager.gamePlayers.Count(x => x.connectionToClient.isReady));
            if (Manager.gamePlayers.Count(x => x.connectionToClient.isReady) != Manager.gamePlayers.Count) return;
            animator.enabled = true;
            RpcStartCountDown();
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcStartCountDown()
        {
            animator.enabled = true;
            Debug.Log("RpcStartCountDown");
        }
        
        [ClientRpc]
        private void RpcStartRound()
        {
            //Unlock controls
            Debug.Log("RpcStartRound");
        }

        #endregion
    }
}