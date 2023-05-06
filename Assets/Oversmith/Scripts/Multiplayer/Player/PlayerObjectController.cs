using System;
using Mirror;
using Oversmith.Scripts.Multiplayer.Managers;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Multiplayer.Player
{
    public class PlayerObjectController : NetworkBehaviour
    {
        [SyncVar] public int ConnectionID;
        [SyncVar] public int PlayerIdNumber;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
        [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;

        private CustomNetworkManager _manager;
        public GameObject playerModel;

        public CustomNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null))
                {
                    return _manager;
                }

                return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            playerModel.SetActive(false);
        }

        public void PlayerNameUpdate(string oldValue, string newValue)
        {
            if (isServer)
            {
                this.PlayerName = newValue;
            }

            if (isClient)
            {
                SteamLobbyController.Instance.UpdatePlayerList();
            }
        }

        public override void OnStartAuthority()
        {
            CmdSetPlayerName(SteamFriends.GetPersonaName());
            gameObject.name = "LocalGamePlayer"; 
            SteamLobbyController.Instance.FindLocalPlayer();
            SteamLobbyController.Instance.UpdateLobbyName();
        }

        public override void OnStartClient()
        {
            Manager.GamePlayers.Add(this);
            SteamLobbyController.Instance.UpdateLobbyName();
            SteamLobbyController.Instance.UpdatePlayerList();
        }

        public override void OnStopClient()
        {
            Manager.GamePlayers.Remove(this);
            if (SteamLobbyController.InstanceExists)
            {
                SteamLobbyController.Instance.UpdatePlayerList();
            }
        }
        [Command]
        private void CmdSetPlayerName(string PlayerName)
        {
            this.PlayerNameUpdate(this.PlayerName,PlayerName);
        }

        private void PlayerReadyUpdate(bool oldValue, bool newValue)
        {
            if (isServer)
            {
                this.ready = newValue;
            }

            
            if (isClient)
            {
                Debug.Log("PlayerReadyUpdate: " + SteamLobbyController.InstanceExists);
                SteamLobbyController.Instance.UpdatePlayerItem();
            }
        }

        [Command]
        private void CmdSetPlayerReady()
        {
            this.PlayerReadyUpdate(this.ready, !this.ready);
        }


        public void ChangeReady()
        {
            if (hasAuthority)
            {
                CmdSetPlayerReady();
            }
        }

        public void CanStartGame(string sceneName)
        {
            if (hasAuthority)
            {
                CmdCanStartGame(sceneName);
            }
        }
        [Command]
        public void CmdCanStartGame(string sceneName)
        {
            
            _manager.StartGame(sceneName);
        }
        
        [Server]
        public void UpdatePlayerVisual()
        {
            
            Debug.Log("Update visual " + SceneManager.GetActiveScene().name);
            playerModel.SetActive(SceneManager.GetActiveScene().name == "Game");
        }
    }
}