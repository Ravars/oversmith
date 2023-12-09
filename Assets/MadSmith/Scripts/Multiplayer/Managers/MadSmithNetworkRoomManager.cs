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
    }
}