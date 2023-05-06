using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Oversmith.Scripts.Multiplayer.Managers;
using Oversmith.Scripts.Utils;
using Steamworks;

namespace Oversmith.Scripts.Multiplayer
{
    public class SteamLobby : Singleton<SteamLobby>
    {
        //Callbacks
        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequest;
        protected Callback<LobbyEnter_t> LobbyEntered;
        //Lobbies callback
        protected Callback<LobbyMatchList_t> LobbyList;
        protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;
        
        public List<CSteamID> lobbyIDs = new();
        private CustomNetworkManager _manager;
        
        //Variables
        public ulong CurrentLobbyID;
        private const string HostAddressKey = "HostAddress";

        private void Start()
        {
            if (!SteamManager.Initialized) return;
            _manager = GetComponent<CustomNetworkManager>();
            
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        
            LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
            LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
        }
        [ContextMenu("Host Steam Lobby")]
        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _manager.maxConnections);
        }
        public void JoinLobby(CSteamID lobbyID)
        {
            Debug.Log("Join called");
            SteamMatchmaking.JoinLobby(lobbyID);   
        }
        public void GetLobbiesList()
        {
            if(lobbyIDs.Count > 0) lobbyIDs.Clear();
        
            SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
            SteamMatchmaking.RequestLobbyList();
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            Debug.Log("OnLobbyCreated trying");
            if (callback.m_eResult != EResult.k_EResultOK) { return; }
            Debug.Log("Lobby created successfully.");
            _manager.StartHost();
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
                SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
                SteamFriends.GetPersonaName() + "'s Lobby");
        }
        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            Debug.Log("Lobby entered");
            //Everyone
            CurrentLobbyID = callback.m_ulSteamIDLobby;
            
            //Clients
            if (NetworkServer.active) { return; }
            _manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            _manager.StartClient();
        }

        private void OnGetLobbyList(LobbyMatchList_t result)
        {
            if (LobbiesListManager.Instance.listOfLobbies.Count > 0)
            {
                LobbiesListManager.Instance.DestroyLobbies();
            }

            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                lobbyIDs.Add(lobbyID);
                SteamMatchmaking.RequestLobbyData(lobbyID);
            }
        }

        private void OnGetLobbyData(LobbyDataUpdate_t result)
        {
            Debug.Log("A: " + LobbiesListManager.InstanceExists);
            LobbiesListManager.Instance.DisplayLobbies(lobbyIDs, result);
        }
    }
}