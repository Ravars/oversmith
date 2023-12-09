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
    }
}