using System;
using System.Collections.Generic;
using System.Linq;
using kcp2k;
using UnityEngine;
using Mirror;
using Mirror.FizzySteam;
using Steamworks;

namespace _Developers.Vitor.Multiplayer2.Scripts
{
    public enum TransportLayer
    {
        Steam,
        LocalHost
    }
    public class MadSmithNetworkManager : NetworkManager
    {
        public List<LobbyClient> lobbyPlayers = new();
        
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        private readonly string ResourcesPath = "Multiplayer2NetworkPrefabs";
        public TransportLayer TransportLayer { get; private set; }
        private KcpTransport _localHostTransport;
        //Steam
        private FizzySteamworks _fizzySteamworksTransport;
        private SteamLobby _steamLobby;
        private SteamManager _steamManager;
        [SerializeField] private LobbyController lobbyController;
        [SerializeField] private LobbiesListManager lobbiesListManager;

        [Tooltip("Player lobby prefab")][SerializeField] private LobbyClient lobbyPrefab;
        public override void Awake()
        {
            base.Awake();
            _localHostTransport = GetComponent<KcpTransport>();
            _fizzySteamworksTransport = GetComponent<FizzySteamworks>();
            _steamLobby = GetComponent<SteamLobby>();
            _steamManager = GetComponent<SteamManager>();
            DisableSteamResources();
            DisableLocalhostResources();
        }

        private void DisableSteamResources()
        {
            _fizzySteamworksTransport.enabled = false;
            _steamLobby.enabled = false;
            _steamManager.enabled = false;
            lobbyController.gameObject.SetActive(false);
        }

        private void DisableLocalhostResources()
        {
            _localHostTransport.enabled = false;
        }
        

        public bool SteamIsOpen()
        {
            return SteamManager.Initialized;
        }

        public void HostBySteam()
        {
            Debug.Log("Host by steam");
            TransportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            _fizzySteamworksTransport.enabled = true;
            _steamManager.enabled = true;
            _steamLobby.enabled = true;
            lobbyController.gameObject.SetActive(true);
            Invoke(nameof(HostLobbySteamCall),1f);
        }

        public void JoinBySteam()
        {
            TransportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            _fizzySteamworksTransport.enabled = true;
            _steamManager.enabled = true;
            _steamLobby.enabled = true;
            lobbiesListManager.gameObject.SetActive(true);
            lobbyController.gameObject.SetActive(true);
            lobbiesListManager.GetListOfLobbies();
            // _steamLobby.
        }

        public void HostLobbySteamCall()
        {
            _steamLobby.HostLobby();
        }
        public void HostByLocalHost()
        {
            Debug.Log("Host by localhost");
            TransportLayer = TransportLayer.LocalHost;
            transport = _localHostTransport;
            _localHostTransport.enabled = true;
            lobbyController.gameObject.SetActive(true);
            StartHost();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("Player added");
            LobbyClient lobbyClient = Instantiate(lobbyPrefab);
            lobbyClient.isLeader = lobbyPlayers.Count == 0;
            lobbyClient.ConnectionID = conn.connectionId;
            if (TransportLayer == TransportLayer.Steam)
            {
                lobbyClient.PlayerSteamID = (ulong) SteamMatchmaking.GetLobbyMemberByIndex((CSteamID)SteamLobby.Instance.currentLobbyID, lobbyPlayers.Count);
            }
            NetworkServer.AddPlayerForConnection(conn, lobbyClient.gameObject);
        }

        public override void OnStartServer()
        {
            Debug.Log("OnStartServer");
            spawnPrefabs = Resources.LoadAll<GameObject>(ResourcesPath).ToList();
            
            // _loadEventChannelSo.OnLoadingRequested += OnLoadingRequested;
        }

        public override void OnStartClient()
        {
            Debug.Log("OnStartClient");
            var spawnablePrefabs = Resources.LoadAll<GameObject>(ResourcesPath);
            foreach (var spawnablePrefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(spawnablePrefab);
            }

            OnClientConnected?.Invoke();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            OnClientDisconnected?.Invoke();
        }

        public void StartGame(string sceneName)
        {
            Debug.Log("Start game");
        }
    }
}