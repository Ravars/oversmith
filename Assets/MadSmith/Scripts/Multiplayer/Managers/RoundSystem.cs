using Mirror;
using System.Linq;
using MadSmith.Scripts.Input;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class RoundSystem : NetworkBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private InputReader inputReader;
        private MadSmithNetworkManager room;
        private MadSmithNetworkManager Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }

        public void CountdownEnded()
        {
            animator.enabled = false;
        }

        #region Server

        public override void OnStartServer()
        {
            MadSmithNetworkManager.OnServerStopped += CleanUpServer;
            MadSmithNetworkManager.OnServerReadied += CheckToStartRound;
        }

        [ServerCallback]
        private void OnDestroy() => CleanUpServer();

        [Server]
        private void CleanUpServer()
        {
            MadSmithNetworkManager.OnServerStopped -= CleanUpServer;
            MadSmithNetworkManager.OnServerReadied -= CheckToStartRound;
        }

        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }

        [Server]
        private void CheckToStartRound(NetworkConnection conn)
        {
            if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { return; }

            animator.enabled = true;

            RpcStartCountdown();
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcStartCountdown()
        {
            animator.enabled = true;
        }

        [ClientRpc]
        private void RpcStartRound()
        {
            Debug.Log("Enable movement RpcStartRound");
            // inputReader.EnableGameplayInput();
            Room.EnableMovement();
            // InputManager.Remove(ActionMapNames.Player);
        }

        #endregion
    }
}