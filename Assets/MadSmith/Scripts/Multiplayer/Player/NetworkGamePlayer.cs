using MadSmith.Scripts.Multiplayer.Managers;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Player
{
    public class NetworkGamePlayer : NetworkBehaviour
    {
        [SyncVar]
        private string displayName = "Loading...";
        private MadSmithNetworkManager room;
        private MadSmithNetworkManager Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }

        public override void OnStartAuthority()
        {
            // base.OnStartAuthority();
            // NetworkClient.PrepareToSpawnSceneObjects();
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            // Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            // Room.GamePlayers.Remove(this);
        }

        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }
    }
}