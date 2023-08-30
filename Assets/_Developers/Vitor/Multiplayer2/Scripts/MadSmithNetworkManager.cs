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
        private TransportLayer _transportLayer;
        private KcpTransport _localHostTransport;
        private FizzySteamworks _fizzySteamworksTransport;

        [Tooltip("Player lobby prefab")][SerializeField] private LobbyClient lobbyPrefab;
        public override void Awake()
        {
            base.Awake();
            _localHostTransport = GetComponent<KcpTransport>();
            _fizzySteamworksTransport = GetComponent<FizzySteamworks>();
        }

        public bool SteamIsOpen()
        {
            return SteamManager.Initialized;
        }

        public void HostBySteam()
        {
            Debug.Log("Host by steam");
            _transportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            _fizzySteamworksTransport.enabled = true;
            StartHost();
        }
        public void HostByLocalHost()
        {
            Debug.Log("Host by localhost");
            _transportLayer = TransportLayer.LocalHost;
            transport = _localHostTransport;
            _localHostTransport.enabled = true;
            StartHost();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("Player added");
            LobbyClient lobbyClient = Instantiate(lobbyPrefab);
            lobbyClient.isLeader = lobbyPlayers.Count == 0;
            lobbyClient.ConnectionID = conn.connectionId;
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
        }
    }
}