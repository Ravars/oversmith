using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private PlayerObjectController gamePlayerPrefab;
        public List<PlayerObjectController> GamePlayers { get; } = new List<PlayerObjectController>();

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == LevelNames.MenuPrincipal.ToString())
            {
                if (gamePlayerPrefab == null) return;
                Debug.Log("Player Added");
                PlayerObjectController gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.ConnectionID = conn.connectionId;
                gamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
                gamePlayerInstance.PlayerSteamID = (ulong) SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.currentLobbyID, GamePlayers.Count);

                NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
        }
    }
}