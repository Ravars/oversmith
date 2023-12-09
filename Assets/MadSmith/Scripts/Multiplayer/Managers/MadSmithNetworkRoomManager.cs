using kcp2k;
using MadSmith.Scripts.Multiplayer.Old.Managers;
using Mirror;
using Mirror.FizzySteam;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public enum TransportLayer
    {
        Steam,
        LocalHost
    }
    public class MadSmithNetworkRoomManager : NetworkRoomManager
    {
        // Transport Layers
        public TransportLayer TransportLayer { get; private set; }
        private KcpTransport _localHostTransport;
        private FizzySteamworks _fizzySteamworksTransport;
        //Steam
        public SteamLobby SteamLobby { get; private set; }
        private SteamManager _steamManager;

        public override void Awake()
        {
            base.Awake();
            SteamLobby = GetComponent<SteamLobby>();
            _steamManager = GetComponent<SteamManager>();
            _localHostTransport = GetComponent<KcpTransport>();
            _fizzySteamworksTransport = GetComponent<FizzySteamworks>();
        }

        private bool _showStartButton;
        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
            _showStartButton = true;
#endif
        }

        /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            Debug.Log("OnRoomServerSceneLoadedForPlayer");
            // PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
            // playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
            return true;
        }
        public void HostBySteam()
        {
            TransportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            EnableSteamResources();
            Invoke(nameof(HostLobbySteamCall),1f);
        }
        public void HostLobbySteamCall()
        {
            SteamLobby.HostLobby();
        }
        public void JoinBySteam()
        {
            TransportLayer = TransportLayer.Steam;
            transport = _fizzySteamworksTransport;
            SteamLobby.enabled = true;
            _fizzySteamworksTransport.enabled = true;
            _steamManager.enabled = true;
            // lobbyController.gameObject.SetActive(true);
            LobbiesListManager.Instance.GetListOfLobbies();
        }
        private void EnableSteamResources()
        {
            _fizzySteamworksTransport.enabled = true;
            SteamLobby.enabled = true;
            _steamManager.enabled = true;
        }
        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && _showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                // set to false to hide it in the game scene
                _showStartButton = false;

                ServerChangeScene(GameplayScene);
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("OnStartClient server");
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            Debug.Log("OnServerAddPlayer");
        }
    }
}