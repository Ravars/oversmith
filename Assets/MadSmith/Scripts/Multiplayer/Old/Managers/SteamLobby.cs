using System.Collections.Generic;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.Utils;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Multiplayer.Old.Managers
{
    public struct LobbyData
    {
        public List<CSteamID> BackupCSteamIds;
        public LobbyDataUpdate_t BackupLobbyData;
    }
    
    public class SteamLobby : Singleton<SteamLobby>
    {
        //Callbacks
        protected Callback LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> JoinRequested;
        protected Callback<LobbyEnter_t> LobbyEntered;
        
        
        // Lobbies callbacks
        protected Callback<LobbyMatchList_t> LobbyList;
        protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;

        public List<CSteamID> lobbyIDs = new();
        


        public ulong currentLobbyID;
        private const string HostAddressKey = "HostAddress";
        private MadSmithNetworkRoomManager _manager;

        public UnityAction OnLobbyEnteredEvent;
        public UnityAction OnLobbyListRequestedEvent;

        // private LobbyData _temporaryData;
        
        private void Start()
        {
            if (!SteamManager.Initialized)
            {
                Debug.LogWarning("Steam not initialized");
                return;
            }
            _manager = GetComponent<MadSmithNetworkRoomManager>();
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
            LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            
            LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
            LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
        }

        

        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _manager.maxConnections);
        }

        public void JoinLobby(CSteamID lobbyID)
        {
            Debug.Log("JoinLobby");
            SteamMatchmaking.JoinLobby(lobbyID);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            Debug.Log("Lobby created Before!");
            if (callback.m_eResult != EResult.k_EResultOK) return;
            
            Debug.Log("Lobby created Successfully!");
            
            _manager.StartHost();
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
                SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
                "Mad Smith - " + SteamFriends.GetPersonaName()); //TODO: change this to Localization
            
        }

        private void OnJoinRequest(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            Debug.Log("Lobby Entered");
            //Everyone
            currentLobbyID = callback.m_ulSteamIDLobby;
            OnLobbyEnteredEvent?.Invoke();
            //Client
            if(NetworkServer.active) return;
            Debug.Log("Lobby Entered client");
            _manager.networkAddress =
                SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
            _manager.StartClient();
        }

        public void GetLobbiesList()
        {
            if(lobbyIDs.Count > 0) { lobbyIDs.Clear();}
            
            SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
            SteamMatchmaking.RequestLobbyList();
            OnLobbyListRequestedEvent?.Invoke();
        }
        
        private void OnGetLobbyList(LobbyMatchList_t result)
        {
            if(LobbiesListManager.Instance.listOfLobbies.Count > 0) {LobbiesListManager.Instance.DestroyLobbies(); }

            for (int i = 0; i < result.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyId = SteamMatchmaking.GetLobbyByIndex(i);
                lobbyIDs.Add(lobbyId);
                SteamMatchmaking.RequestLobbyData(lobbyId);
            }
        }
        private void OnGetLobbyData(LobbyDataUpdate_t result)
        {
            if (!LobbiesListManager.InstanceExists) return;
            LobbiesListManager.Instance.DisplayLobbies(lobbyIDs, result);
            
            // _temporaryData = new LobbyData()
            // {
            //     BackupLobbyData =  result,
            //     BackupCSteamIds = lobbyIDs
            // };
        }

        // public LobbyData GetTemporaryData()
        // {
        //     var lobbyData = _temporaryData;
        //     _temporaryData = new LobbyData();
        //     return lobbyData;
        // }
        
    }
}