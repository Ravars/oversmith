using System;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class SteamLobby : MonoBehaviour
    {
        //Callbacks
        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequested;
        protected Callback<LobbyEnter_t> LobbyEntered;

        public ulong currentLobbyID;
        private const string HostAddressKey = "HostAddress";
        private CustomNetworkManager _manager;

        public GameObject hostButton;
        public TextMeshProUGUI lobbyNameText;

        private void Start()
        {
            if (!SteamManager.Initialized)
            {
                Debug.LogWarning("Steam not initialized");
                return;
            }
            _manager = GetComponent<CustomNetworkManager>();
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }

        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _manager.maxConnections);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK) return;
            
            Debug.Log("Lobby created Successfully!");
            
            _manager.StartHost();
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
                SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
                SteamFriends.GetPersonaName() + "'s Lobby"); //TODO: change this to Localization
            
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            //Everyone
            hostButton.SetActive(false);
            currentLobbyID = callback.m_ulSteamIDLobby;
            lobbyNameText.gameObject.SetActive(true);
            lobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");
            
            //Client
            if(NetworkServer.active) return;

            _manager.networkAddress =
                SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            _manager.StartClient();
        }
    }
}