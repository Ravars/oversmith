using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SceneManagement;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Developers.Vitor.Multiplayer_1.Scripts
{
    public class HelloWorldNetworkManager : NetworkManager
    {
        [SerializeField] private LobbyClient gamePlayerPrefab;
        [SerializeField] private PlayerMovement inGamePlayerPrefab;
        public List<LobbyClient> lobbyPlayers = new List<LobbyClient>();
        public List<PlayerMovement> gamePlayers { get; } = new List<PlayerMovement>();
        [Scene] [SerializeField] private string menuScene;

        [Header("Listening to")] 
        [SerializeField] private LoadEventChannelSO _loadEventChannelSo;
        
        [Header("Game")]
        [SerializeField] private GameObject roundSystem = null;
        
        
        
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        private int _playersNotReady;
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("Server Started");
            spawnPrefabs = Resources.LoadAll<GameObject>("HelloNetworkPrefabs").ToList();
            _loadEventChannelSo.OnLoadingRequested += OnLoadingRequested;
        }

        private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
        {
            _playersNotReady = lobbyPlayers.Count;
            Debug.Log("new scene loaded" + arg0.sceneType + " " + lobbyPlayers.Count);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _loadEventChannelSo.OnLoadingRequested -= OnLoadingRequested;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            Debug.Log("Server stopped");
            OnServerStopped?.Invoke();
            lobbyPlayers.Clear();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // base.OnServerAddPlayer(conn);
            Debug.Log("On server add player");
            LobbyClient lobbyClient = Instantiate(gamePlayerPrefab);
            lobbyClient.ConnectionID = conn.connectionId;

            NetworkServer.AddPlayerForConnection(conn, lobbyClient.gameObject);
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            Debug.Log("Client changed scene" + newSceneName);
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            Debug.Log("Client scene changed");
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            Debug.Log("OnServerChangeScene "  + newSceneName);
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Level"))
            {
                for (int i = 0; i < lobbyPlayers.Count; i++)
                {
                    var conn = lobbyPlayers[i].connectionToClient;
                    var instance = Instantiate(gamePlayerPrefab);
                    NetworkServer.Destroy(conn.identity.gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
                }
            }
            base.OnServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            Debug.Log("OnServerSceneChanged");
            // base.OnServerSceneChanged(sceneName);
            if (sceneName.StartsWith("Level"))
            {
                //
            }
        }

        public void Opa(string newSceneName)
        {
            // ServerChangeScene("Level1");
            Debug.Log(newSceneName);
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Level"))
            {
                for (int i = 0; i < lobbyPlayers.Count; i++)
                {
                    var conn = lobbyPlayers[i].connectionToClient;
                    // var instance = Instantiate(gamePlayerPrefab);
                    NetworkServer.Destroy(conn.identity.gameObject);
                    // NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
                }
            }
        }
        public override void OnStartClient()
        {
            // base.OnStartClient();
            var spawnablePrefabs = Resources.LoadAll<GameObject>("HelloNetworkPrefabs");
            foreach (var spawnablePrefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(spawnablePrefab);
            }
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            Debug.Log("On server readied - message from server");
            OnServerReadied?.Invoke(conn);
        }

        public void ClientSceneReady()
        {
            //TODO: if Level scene only   
            --_playersNotReady;
            if (_playersNotReady <= 0)
            {
                Debug.Log("All players ready: " + lobbyPlayers.Count);
                var currentSceneLoaded = SceneLoader.Instance.GetCurrentSceneLoaded();
                if (currentSceneLoaded.sceneType == GameSceneType.Location)
                {
                    gamePlayers.Clear();
                    for (int i = 0; i < lobbyPlayers.Count; i++)
                    {
                        var conn = lobbyPlayers[i].connectionToClient;
                        GameObject oldPlayer = conn.identity.gameObject;
                        var instance = Instantiate(inGamePlayerPrefab);
                        gamePlayers.Add(instance);
                        NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
                        Destroy(oldPlayer, 0.1f);
                    }
                }
                Debug.Log("Should start countdown");
                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);
                // StartCoroutine(nameof(LevelCountDown));
            }
            else
            {
                Debug.Log("Still loading " + _playersNotReady);
            }
        }

        IEnumerator LevelCountDown()
        {
            Debug.Log("Level countdown");
            int timeCountdown = 3;
            while (timeCountdown > 0)
            {
                foreach (var playerMovement in gamePlayers)
                {
                    
                }
                timeCountdown--;
                yield return new WaitForSeconds(1);
            }
            
            Debug.Log("Level countdown 3 seconds");
            for (int i = 0; i < lobbyPlayers.Count; i++)
            {
                var conn = lobbyPlayers[i].connectionToClient;
                // GameObject player = conn.identity.gameObject;
                
                // PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                // playerMovement.EnableMovement();
            }

            foreach (var playerMovement in gamePlayers)
            {
                playerMovement.CmdEnableMovement();
            }
        } 
    }
}
