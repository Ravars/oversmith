using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Developers.Vitor.Multiplayer_1.Scripts.UI;
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
        private List<PlayerMovement> GamePlayers { get; } = new();
        public List<LobbyClient> lobbyPlayers = new();

        [Header("Game")]
        [SerializeField] private LobbyClient gamePlayerPrefab;
        [SerializeField] private PlayerMovement inGamePlayerPrefab;
        [SerializeField] private GameObject roundSystem = null;
        private int _playersNotReady;
        
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        
        [Header("Listening to")] 
        [SerializeField] private LoadEventChannelSO _loadEventChannelSo;
        
        
        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("HelloNetworkPrefabs").ToList();
            _loadEventChannelSo.OnLoadingRequested += OnLoadingRequested;
        }
        
        /// <summary>
        /// Esta função vai adicionar todos os prefabs spawnaveis no "networkManager" no lado do client
        /// </summary>
        public override void OnStartClient()
        {
            var spawnablePrefabs = Resources.LoadAll<GameObject>("HelloNetworkPrefabs");
            foreach (var spawnablePrefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(spawnablePrefab);
            }
        }
        
        /// <summary>
        /// Evento do SceneLoader que avisa quando foi requisitado um loading.
        /// A ideia aqui é só inicializar a quantidade de players in-game
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
        {
            _playersNotReady = lobbyPlayers.Count;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _loadEventChannelSo.OnLoadingRequested -= OnLoadingRequested;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            lobbyPlayers.Clear();
            GamePlayers.Clear();
        }
        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in lobbyPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }
        private bool IsReadyToStart()
        {
            if (numPlayers < 2) { return false; }

            foreach (var player in lobbyPlayers)
            {
                if (!player.isReady) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Função chamada na primeira vez que o cliente é adicionado
        /// </summary>
        /// <param name="conn"></param>
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            LobbyClient lobbyClient = Instantiate(gamePlayerPrefab);
            lobbyClient.IsLeader = lobbyPlayers.Count == 0;
            lobbyClient.ConnectionID = conn.connectionId;
            NetworkServer.AddPlayerForConnection(conn, lobbyClient.gameObject);
        }

        
        
        /// <summary>
        /// Os clients chamam essa função depois que o SceneLoader sinaliza que o level foi carregado.
        /// Essa função vai destruir o "lobby client" e spawnar o personagem de fato.
        /// </summary>
        public void ClientSceneReady()
        {
            //TODO: if Level scene only   
            --_playersNotReady;
            if (_playersNotReady <= 0)
            {
                var currentSceneLoaded = SceneLoader.Instance.GetCurrentSceneLoaded();
                if (currentSceneLoaded.sceneType == GameSceneType.Location)
                {
                    GamePlayers.Clear();
                    foreach (var lobbyClient in lobbyPlayers)
                    {
                        var conn = lobbyClient.connectionToClient;
                        GameObject oldPlayer = conn.identity.gameObject;
                        var instance = Instantiate(inGamePlayerPrefab);
                        GamePlayers.Add(instance);
                        NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
                        Destroy(oldPlayer, 0.1f);
                    }
                }
                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);
            }
            else
            {
                Debug.Log("Still loading " + _playersNotReady);
            }
        }

        /// <summary>
        /// Esta função vai ser chamada pelo "levelManager" depois do termino do timer de countdown.
        /// A função dela é ativar o movimento depois do timer.
        /// </summary>
        public void EnableMovement()
        {
            foreach (var playerMovement in GamePlayers)
            {
                playerMovement.CmdEnableMovement();
            }
        }

        public void UiSetState()
        {
            foreach (var lobbyPlayer in lobbyPlayers)
            {
                lobbyPlayer.SetState(MenuState.CharacterSelection);
            }
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            Debug.Log("Client disconnected");
            OnClientDisconnected?.Invoke();
            base.OnClientDisconnect();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            //verify maximum number of players
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            Debug.Log("On server disconnect");
            base.OnServerDisconnect(conn);
            OnClientDisconnected?.Invoke();
        }
    }
}
