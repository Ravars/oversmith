using System;
using System.Collections.Generic;
using System.Linq;
using kcp2k;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Multiplayer.Player;
using MadSmith.Scripts.SceneManagement;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using Mirror.FizzySteam;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public enum TransportLayer
    {
        Steam,
        LocalHost
    }
    public class MadSmithNetworkManager : NetworkManager
    {
        // Variable
        // private int _playersNotReady;
        public List<LobbyClient> lobbyPlayers = new();
        public List<NetworkGamePlayer> GamePlayers { get; } = new();
        public TransportLayer TransportLayer { get; private set; }
        public SteamLobby SteamLobby { get; private set; }
        [SerializeField] private bool startWithSteam;
        
        // Transport layers
        private KcpTransport _localHostTransport;
        private FizzySteamworks _fizzySteamworksTransport;
        
        // Prefabs
        [SerializeField] private NetworkGamePlayer gamePlayerPrefab = null;
        [SerializeField] private NetworkPlayerMovement[] inGamePlayerPrefab;
        [SerializeField] private GameObject roundSystem = null;
        [SerializeField] private GameObject orderManager = null;
        [SerializeField] private PlayerSpawnSystem playerSpawnSystem = null;
        [Tooltip("Player lobby prefab")][SerializeField] private LobbyClient lobbyPrefab;
        [SerializeField] private LobbiesListManager lobbiesListManager;
        
        // 
        private readonly string ResourcesPath = "NetworkResources";
        private SteamManager _steamManager;
        [SerializeField] private LobbyControllerCanvas lobbyControllerCanvas;
        
        // Actions
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;
        
        [Header("Listening to")] 
        [SerializeField] private LoadEventChannelSO _loadEventChannelSo;

        public override void Awake()
        {
            base.Awake();
            _localHostTransport = GetComponent<KcpTransport>();
            _fizzySteamworksTransport = GetComponent<FizzySteamworks>();
            SteamLobby = GetComponent<SteamLobby>();
            _steamManager = GetComponent<SteamManager>();
            DisableSteamResources();
            DisableLocalhostResources();
            if (startWithSteam)
            {
                EnableSteamResources();
            }
            else
            {
                EnableLocalhostResources();
            }
        }
        // public override void OnDestroy()
        // {
        //     base.OnDestroy();
        //     // _loadEventChannelSo.OnLoadingRequested -= OnLoadingRequested;
        // }

        #region Enable/Disable Resources

        private void DisableSteamResources()
        {
            _fizzySteamworksTransport.enabled = false;
            SteamLobby.enabled = false;
            _steamManager.enabled = false;
            lobbyControllerCanvas.gameObject.SetActive(false);
            
        }
        private void DisableLocalhostResources()
        {
            _localHostTransport.enabled = false;
        }
        private void EnableSteamResources()
        {
            _fizzySteamworksTransport.enabled = true;
            SteamLobby.enabled = true;
            _steamManager.enabled = true;
            // lobbyController.gameObject.SetActive(true);
        }
        private void EnableLocalhostResources()
        {
            _localHostTransport.enabled = true;
        }

        #endregion



        #region Network Override Functions
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            //Debug.Log("On server add player");
            var currentSceneSo = GameManager.Instance.GetSceneSo();
            if (currentSceneSo.sceneType != GameSceneType.Menu) return;
            //Debug.Log("Player added");
            LobbyClient lobbyClient = Instantiate(lobbyPrefab);
            lobbyClient.isLeader = lobbyPlayers.Count == 0;
            lobbyClient.ConnectionID = conn.connectionId;
            if (TransportLayer == TransportLayer.Steam)
            {
                lobbyClient.PlayerSteamID = (ulong) SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.currentLobbyID, lobbyPlayers.Count);
            }
            NetworkServer.AddPlayerForConnection(conn, lobbyClient.gameObject);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                conn.identity.TryGetComponent(out LobbyClient player);
                if (player != null)
                {
                    lobbyPlayers.Remove(player);
                }
                else
                {
                    conn.identity.TryGetComponent(out NetworkGamePlayer playerMovement);
                    if (playerMovement != null)
                    {
                        GamePlayers.Remove(playerMovement);
                    }
                }
                
                // NotifyPlayersOfReadyState
            }
            base.OnServerDisconnect(conn);
        }

        public override void OnStartServer()
        {
            //Debug.Log("OnStartServer");
            spawnPrefabs = Resources.LoadAll<GameObject>(ResourcesPath).ToList();
            // _loadEventChannelSo.OnLoadingRequested += OnLoadingRequested;
        }

        public override void OnStartClient()
        {
            //Debug.Log("OnStartClient");
            var spawnablePrefabs = Resources.LoadAll<GameObject>(ResourcesPath);
            foreach (var spawnablePrefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(spawnablePrefab);
            }
            base.OnStartClient();

            OnClientConnected?.Invoke();
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


        public override void OnStopClient()
        {
            base.OnStopClient();
            OnClientDisconnected?.Invoke();
        }
        public override void OnStopServer()
        {
            OnServerStopped?.Invoke();

            lobbyPlayers.Clear();
            GamePlayers.Clear();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
            // if(numPlayers >= max)
            // {
            //     conn.Disconnect();
            //     return;
            // }
            var currentSceneLoaded = GameManager.Instance.GetSceneSo();
            if (currentSceneLoaded.sceneType != GameSceneType.Menu)
            {
                conn.Disconnect();
                return;
            }
        }

        #endregion
        
        public override void ServerChangeScene(string newSceneName)
        {
            // From menu to game
            Debug.Log("ServerChangeScene");
            var currentSceneLoaded = GameManager.Instance.GetSceneSo();
            // if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Scene_Map"))
            if (currentSceneLoaded.sceneType == GameSceneType.Menu)
            {
                GameManager.Instance.SetGameSceneSo(newSceneName);
                Debug.Log("lobbyPlayers.Count: " + lobbyPlayers.Count);
                // _playersNotReady = lobbyPlayers.Count;
                for (int i = lobbyPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = lobbyPlayers[i].connectionToClient;
                    // var gamePlayerInstance = Instantiate(inGamePlayerPrefab[lobbyPlayers[i].CharacterId]);
                    var gamePlayerInstance = Instantiate(gamePlayerPrefab);
                    gamePlayerInstance.SetDisplayName(lobbyPlayers[i].PlayerName);
                    
                    NetworkServer.Destroy(conn.identity.gameObject);
                    Debug.Log("ServerChangeScene" + gamePlayerInstance.name);
                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
                }
            }
            
            base.ServerChangeScene(newSceneName);
        }
        public override void OnServerSceneChanged(string sceneName)
        {
            //Debug.Log("sceneName");
            Debug.Log("OnServerSceneChanged" + sceneName);
            if (sceneName.StartsWith("Level"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem.gameObject);
                NetworkServer.Spawn(playerSpawnSystemInstance);
        
                GameObject roundSystemInstance = Instantiate(roundSystem);
                NetworkServer.Spawn(roundSystemInstance);
                
                GameObject orderManagerInstance = Instantiate(orderManager);
                NetworkServer.Spawn(orderManagerInstance);
            }
        }

        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);
            OnServerReadied?.Invoke(conn);
        }
        

        // public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        // {
        //     base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        //     Debug.Log("OnClientChangeScene");
        // }
        
        // public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        // {
        //     
        //     Debug.Log("OnClientChangeScene");
        //     for (int i = lobbyPlayers.Count - 1; i >= 0; i--)
        //     {
        //         var conn = lobbyPlayers[i].connectionToClient;
        //         Debug.Log("Is Ready: " + conn.isReady);
        //     }
        //     base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        // }

        // public override void OnClientSceneChanged()
        // {
        //     base.OnClientSceneChanged();
        //     
        //     Debug.Log("OnClientSceneChanged");
        //     // NetworkClient.localPlayer.TryGetComponent(out LobbyClient lobbyClient);
        //     // if (lobbyClient != null)
        //     // {
        //     //     lobbyClient.OnSceneReady();
        //     // }
        //     // CmdSceneReady();
        //     // ClientSceneReady(); //Deveria usar isso?
        // }

        // /// <summary>
        // /// Evento do SceneLoader que avisa quando foi requisitado um loading.
        // /// A ideia aqui é só inicializar a quantidade de players in-game
        // /// </summary>
        // /// <param name="arg0"></param>
        // /// <param name="arg1"></param>
        // /// <param name="arg2"></param>
        // private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
        // {
        //     //Debug.Log("OnLoadingRequested");
        //     // _playersNotReady = lobbyPlayers.Count;
        // }

        // /// <summary>
        // /// Os clients chamam essa função depois que o SceneLoader sinaliza que o level foi carregado.
        // /// Essa função vai destruir o "lobby client" e spawnar o personagem de fato.
        // /// </summary>
        // public void ClientSceneReady()
        // {
        //     Debug.Log("ClientSceneReady before: " + _playersNotReady);
        //     //TODO: if Level scene only   
        //     --_playersNotReady;
        //     //Debug.Log("ClientSceneReady");
        //     if (_playersNotReady <= 0)
        //     {
        //         Debug.Log("ClientSceneReady inside");
        //         var currentSceneLoaded = SceneLoader.Instance.GetCurrentSceneLoaded();
        //         Debug.Log("Scene: " + currentSceneLoaded.sceneType);
        //         if (currentSceneLoaded.sceneType == GameSceneType.Location)
        //         {
        //             GamePlayers.Clear();
        //             float offset = 0;
        //             foreach (var lobbyClient in lobbyPlayers)
        //             {
        //                 var conn = lobbyClient.connectionToClient;
        //                 GameObject oldPlayer = conn.identity.gameObject;
        //                 var instance = Instantiate(inGamePlayerPrefab[lobbyClient.CharacterId], new Vector3(-8f + offset, 0, -8f), Quaternion.identity);
        //                 GamePlayers.Add(instance);
        //                 NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
        //                 Destroy(oldPlayer, 0.1f);
        //                 
        //                 offset += 1f;
        //             }
        //         }
        //         GameObject roundSystemInstance = Instantiate(roundSystem);
        //         NetworkServer.Spawn(roundSystemInstance);
        //         GameObject orderManagerInstance = Instantiate(orderManager);
        //         NetworkServer.Spawn(orderManagerInstance);
        //     }
        //     else
        //     {
        //         //Debug.Log("Still loading " + _playersNotReady);
        //     }
        // }

        /// <summary>
        /// Esta função vai ser chamada pelo "levelManager" depois do termino do timer de countdown.
        /// A função dela é ativar o movimento depois do timer.
        /// </summary>
        public void EnableMovement()
        {
            Debug.Log("EnableMovement: " + GamePlayers.Count);
            // for (int i = lobbyPlayers.Count - 1; i >= 0; i--)
            // {
            //     var conn = lobbyPlayers[i].connectionToClient;
            //     var gamePlayerInstance = Instantiate(inGamePlayerPrefab[lobbyPlayers[i].CharacterId]);
            //     // gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);
            //     //Debug.Log("conn.identity" + conn.identity.netId + " " + conn.isReady);
            //     GamePlayers.Add(gamePlayerInstance);
            //     NetworkServer.Destroy(conn.identity.gameObject);
            //     //Debug.Log("EnableMovement" + gamePlayerInstance.name);
            //     NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            // }
            
            
            // foreach (var playerMovement in GamePlayers)
            // {
            //     playerMovement.CmdEnableMovement();
            // }
            // GameObject orderManagerInstance = Instantiate(orderManager);
            // NetworkServer.Spawn(orderManagerInstance);
        }

        public bool SteamIsOpen()
        {
            return SteamManager.Initialized;
        }

        public void HostBySteam()
        {
            //Debug.Log("Host by steam");
            TransportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            EnableSteamResources();
            // UIMenuManager.Instance.set
            // lobbyController.gameObject.SetActive(true); //change to UI menu manager
            Invoke(nameof(HostLobbySteamCall),1f);
        }
        public void HostLobbySteamCall()
        {
            SteamLobby.HostLobby();
        }
        public void HostByLocalHost()
        {
            //Debug.Log("Host by localhost");
            TransportLayer = TransportLayer.LocalHost;
            transport = _localHostTransport;
            _localHostTransport.enabled = true;
            lobbyControllerCanvas.gameObject.SetActive(true);
            StartHost();
        }
        public void StopHostOrClientOnLobbyMenu()
        {
            //Debug.Log("StopHostOrClientOnLobbyMenu");
            if (NetworkServer.active)
            {
                StopHost();
            }
            else
            {
                StopClient();
            }
            lobbiesListManager.DestroyLobbies();
            // lobbyController.gameObject.SetActive(false); // changed to MenuManager
        }
        
        public void JoinBySteam()
        {
            TransportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            SteamLobby.enabled = true;
            _fizzySteamworksTransport.enabled = true;
            _steamManager.enabled = true;
            // lobbyController.gameObject.SetActive(true);
            lobbiesListManager.GetListOfLobbies();
        }
        public void JoinByLocalhost()
        {
            DisableSteamResources();
            TransportLayer = TransportLayer.LocalHost;
            transport = _localHostTransport;
            _localHostTransport.enabled = true;
            // lobbyController.gameObject.SetActive(true);
            
            // lobbyController.gameObject.SetActive(true);
            // StartClient();
            // transport.ClientConnect("localhost");
            
            // Invoke(nameof(StartClient),3f);
        }

        // public void SpawnOrderManager()
        // {
        //     
        // }
        public void StartGame()
        {
            string sceneName = GameManager.Instance.sceneSos[UiNetworkHandler.Instance.currentLevelSelected + 1].name;
            ServerChangeScene(sceneName);
        }
    }
}