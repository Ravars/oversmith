using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Multiplayer.UI;
using MadSmith.Scripts.UI.Managers;
using Mirror;
using Steamworks;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class LobbyClient : NetworkBehaviour
    {
        // Variables
        private const int NumberOfCharacters = 4; 
        private const int NumberOfLevels = 6; 
        [SyncVar] public int ConnectionID;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar] public string PlayerName;
        [SyncVar(hook = nameof(ChangeCharacter))] public int CharacterId;
        [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
        [SyncVar(hook = nameof(ChangeLevelSelected))] public int levelSelected;
        public bool isLeader;
        
        
        private MadSmithNetworkManager _manager;
        private MadSmithNetworkManager Manager
        { get {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }
        
        
        
        [Header("Listening on")]
        [SerializeField] private VoidEventChannelSO sceneReady;
        // [Header("Broadcasting on")]
        // [SerializeField] private LoadEventChannelSO loadLocation = default;
        private void OnDestroy()
        {
            sceneReady.OnEventRaised -= OnSceneReady;
            // inputReader.MenuPauseEvent -= InputReaderOnMenuPauseEvent;
        }
        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority " + hasAuthority);
            DontDestroyOnLoad(gameObject);
            gameObject.name = "LocalGamePlayer";
            sceneReady.OnEventRaised += OnSceneReady;
            CmdSetPlayerName(Manager.TransportLayer == TransportLayer.Steam
                ? SteamFriends.GetPersonaName().ToString()
                : PlayerNameInput.DisplayName); //Test

            LobbyController.Instance.FindLocalPlayer();
            LobbyController.Instance.UpdateLobbyName();
        }

        public override void OnStopClient()
        {
            Manager.lobbyPlayers.Remove(this);
            if (LobbyController.InstanceExists)
            {
                LobbyController.Instance.UpdatePlayerList();
            }
        }
        public override void OnStartClient()
        {
            Debug.Log("OnStartClient" + hasAuthority);    
            Manager.lobbyPlayers.Add(this);
            LobbyController.Instance.UpdateLobbyName();
            LobbyController.Instance.UpdatePlayerList();
        }
        
        private void OnSceneReady()
        {
            CmdSceneReady();
            NetworkClient.PrepareToSpawnSceneObjects(); //Aparentemente tenho que fazer isso aqui
        }
        /// <summary>
        /// Quando o SceneLoader termina de carregar o level ele executa um evento.
        /// Esse evento vai ser executado 
        /// </summary>
        [Command]
        private void CmdSceneReady()
        {
            Debug.Log("Scene ready");
            Manager.ClientSceneReady();
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
                LobbyController.Instance.UpdatePlayerList(); // Verificar pq esse atualiza a lista e o ready usa  Item
            }
        }

        #region Ready State
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
                LobbyController.Instance.UpdatePlayerList();
            }
        }
        #endregion
        
        #region Character
        public void NextCharacter()
        {
            if (hasAuthority)
            {
                int id = (this.CharacterId + 1) % NumberOfCharacters;
                CmdChangeCharacter(this.CharacterId, id);
            }
        }

        public void PreviousCharacter()
        {
            if (hasAuthority)
            {
                int id = this.CharacterId - 1 < 0 ? NumberOfCharacters - 1 : this.CharacterId - 1;
                CmdChangeCharacter(this.CharacterId,id);
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
        #endregion
        
        #region Level
        public void NextLevel()
        {
            if (hasAuthority)
            {
                int id = (this.levelSelected + 1) % NumberOfLevels;
                CmdChangeLevelSelected(this.levelSelected, id);
            }
        }

        public void PreviousLevel()
        {
            if (hasAuthority)
            {
                int id = this.levelSelected - 1 < 0 ? NumberOfLevels - 1 : this.levelSelected - 1;
                CmdChangeLevelSelected(this.levelSelected,id);
            }
        }
        [Command]
        private void CmdChangeLevelSelected(int oldLevel, int newLevel)
        {
            this.ChangeLevelSelected(oldLevel, newLevel);
        }
        private void ChangeLevelSelected(int oldLevel, int newId)
        {
            if (isServer)
            {
                this.levelSelected = newId;
            }
            
            if (isClient)
            {
                // LobbyController.Instance.UpdatePlayerList();
            }
        }
        #endregion
        
        

        public void CanStartGame(string sceneName)
        {
            // if (hasAuthority)
            // {
            // }
            CmdCanStartGame(sceneName);
        }
        [Command]
        public void CmdCanStartGame(string sceneName)
        { 
            // _manager.StartGame(sceneName);
            RpcStartGame();
        }
        [ClientRpc]
        private void RpcStartGame()
        {
            Debug.Log("Rpc Server Star Game");
            LobbyController.Instance.LoadingRequested();
            // loadLocation.RaiseEvent(level1,true);
        }
    }
}