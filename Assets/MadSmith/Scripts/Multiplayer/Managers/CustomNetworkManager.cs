using System.Collections.Generic;
using MadSmith.Scripts.Menu;
using MadSmith.Scripts.Multiplayer.Player;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class CustomNetworkManager : NetworkManager
    {
        [SerializeField] private PlayerObjectController GamePlayerPrefab;
        public List<PlayerObjectController> GamePlayers { get; } = new();
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (SceneManager.GetActiveScene().name == MultiplayerLevelNames.SteamLobby.ToString())
            {
                PlayerObjectController gamePlayerInstance = Instantiate(GamePlayerPrefab);
                gamePlayerInstance.ConnectionID = conn.connectionId;
                gamePlayerInstance.PlayerIdNumber = GamePlayers.Count + 1;
                gamePlayerInstance.PlayerSteamID =
                    (ulong)SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.CurrentLobbyID,
                        GamePlayers.Count);
                NetworkServer.AddPlayerForConnection(conn, gamePlayerInstance.gameObject);

            }
        }

        public void StartGame(string sceneName)
        {
            ServerChangeScene(sceneName);
            
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            base.OnServerChangeScene(newSceneName);
            Debug.Log("Mudou de cena");
        }
    }
}
