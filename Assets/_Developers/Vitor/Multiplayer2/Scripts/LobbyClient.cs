using Mirror;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer2.Scripts
{
    public class LobbyClient : NetworkBehaviour
    {
        [SyncVar] public int ConnectionID;
        public bool isLeader;
        private MadSmithNetworkManager _manager;
        private MadSmithNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }

        public string[] playerNames;

        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority " + hasAuthority);
            if (!hasAuthority) return;
        }

        public override void OnStartClient()
        {
            Debug.Log("OnStartClient" + hasAuthority);    
            Manager.lobbyPlayers.Add(this);
        }
    }
}