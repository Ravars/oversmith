using System;
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
        [SerializeField] private GameObject inGamePlayerPrefab;
        public List<LobbyClient> players = new List<LobbyClient>();
        [Scene] [SerializeField] private string menuScene;

        [Header("Listening to")] 
        [SerializeField] private LoadEventChannelSO _loadEventChannelSo;
        public static event Action<NetworkConnection> OnServerReadied;

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
            _playersNotReady = players.Count;
            Debug.Log("new scene loaded" + arg0.sceneType + " " + players.Count);
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
            players.Clear();
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
                for (int i = 0; i < players.Count; i++)
                {
                    var conn = players[i].connectionToClient;
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
                for (int i = 0; i < players.Count; i++)
                {
                    var conn = players[i].connectionToClient;
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
            
            --_playersNotReady;
            if (_playersNotReady <= 0)
            {
                Debug.Log("All players ready: " + players.Count);
                var currentSceneLoaded = SceneLoader.Instance.GetCurrentSceneLoaded();
                if (currentSceneLoaded.sceneType == GameSceneType.Location)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        var conn = players[i].connectionToClient;
                        GameObject oldPlayer = conn.identity.gameObject;
                        var instance = Instantiate(inGamePlayerPrefab);
                        NetworkServer.ReplacePlayerForConnection(conn, instance);
                        Destroy(oldPlayer, 0.1f);
                    }
                }
            }
            else
            {
                Debug.Log("Still loading " + _playersNotReady);
            }
        }
    }
}
