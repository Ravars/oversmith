using MadSmith.Scripts.Multiplayer.Player;
using Mirror;
using UnityEngine;

namespace _Developers.Vitor
{
    public class TestNetworkManager : NetworkManager
    {
        
        public void EnableMovement()
        {
            NetworkPlayerMovement[] networkPlayerMovement = FindObjectsOfType<NetworkPlayerMovement>();
            
            
            foreach (var playerMovement in networkPlayerMovement)
            {
                playerMovement.CmdEnableMovement();
            }
            // GameObject orderManagerInstance = Instantiate(orderManager);
            // NetworkServer.Spawn(orderManagerInstance);
        }
    }
}
