using _Developers.Vitor.Multiplayer_1.Scripts;
using Mirror;
using Steamworks;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer2.Scripts
{
    public class LobbyClient : NetworkBehaviour
    {
        private const int NumberOfCharacters = 4; 
        [SyncVar] public int ConnectionID;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar] public string PlayerName;
        [SyncVar] public int CharacterId;
        [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
        public bool isLeader;
        private MadSmithNetworkManager _manager;
        private MadSmithNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }

        public string[] playerNames;

        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority " + hasAuthority);
            // if (!hasAuthority) return;
            gameObject.name = "LocalGamePlayer";

            CmdSetPlayerName(Manager.TransportLayer == TransportLayer.Steam
                ? SteamFriends.GetPersonaName().ToString()
                : PlayerNameInput.DisplayName); //Test
            LobbyController.Instance.FindLocalPlayer();
            LobbyController.Instance.UpdateLobbyName();
            
        }

        public override void OnStartClient()
        {
            Debug.Log("OnStartClient" + hasAuthority);    
            Manager.lobbyPlayers.Add(this);
            LobbyController.Instance.UpdateLobbyName();
            LobbyController.Instance.UpdatePlayerList();
        }

        public override void OnStopClient()
        {
            Manager.lobbyPlayers.Remove(this);
            LobbyController.Instance.UpdatePlayerList();
        }

        public void ChangeReady()
        {
            if (hasAuthority)
            {
                CmdSetPlayerReady();
            }
        }
        [Command]
        private void CmdSetPlayerReady()
        {
            this.PlayerReadyUpdate(this.ready, !this.ready);
        }
        private void PlayerReadyUpdate(bool oldValue, bool newValue)
        {
            if (isServer)
            {
                this.ready = newValue;
            }

            
            if (isClient)
            {
                LobbyController.Instance.UpdatePlayerItem();
            }
        }
        public void NextCharacter()
        {
            if (hasAuthority)
            {
                int id = (CharacterId + 1) % NumberOfCharacters;
                CmdChangeCharacter(CharacterId, id);
            }
        }

        public void PreviousCharacter()
        {
            if (hasAuthority)
            {
                int id = CharacterId - 1 < 0 ? NumberOfCharacters - 1 : CharacterId - 1;
                CmdChangeCharacter(CharacterId,id);
            }
        }

        [Command]
        private void CmdChangeCharacter(int oldId, int newId)
        {
            this.ChangeCharacter(oldId, newId);
        }

        private void ChangeCharacter(int oldId, int newId)
        {
            if (isServer)
            {
                this.CharacterId = newId;
            }
            
            if (isClient)
            {
                LobbyController.Instance.UpdatePlayerList();
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

        [Command]
        public void CmdSetPlayerName(string playerName)
        {
            this.PlayerNameUpdate(this.PlayerName, playerName);
        }

        public void PlayerNameUpdate(string oldValue, string newValue)
        {
            if (isServer)
            {
                this.PlayerName = newValue;
            }

            if (isClient)
            {
                LobbyController.Instance.UpdatePlayerList();
            }
        }
    }
}