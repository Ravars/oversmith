using System;
using System.Collections.Generic;
using System.Linq;
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
        [Scene] [SerializeField] private string menuScene = string.Empty;
        
        
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;
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
                
                //TODO: Set the first one as leader
            }
        }
        public void StartGame(string sceneName)
        {
            // ServerChangeScene(sceneName);
            // Server
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            base.OnServerChangeScene(newSceneName);
            Debug.Log("OnServerChangeScene");
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            Debug.Log("OnClientChangeScene" + " " + newSceneName);
        }

        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
            foreach (var spawnablePrefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(spawnablePrefab);
            }
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            // if (numPlayers >= maxConnections)
            // {
            //     conn.Disconnect();
            //     return;
            // }
            //
            // if (SceneManager.GetActiveScene().name != menuScene)
            // {
            //     conn.Disconnect();
            //     return;
            // }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                // var pla
            }
            base.OnServerDisconnect(conn);
        }
    }
}