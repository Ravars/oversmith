using System;
using MadSmith.Scripts.Multiplayer.Managers;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Multiplayer.Player
{
    [RequireComponent(typeof(PlayerMovementController))]
    public class PlayerObjectController : NetworkBehaviour
    {
        [SyncVar] public int ConnectionID;
        [SyncVar] public int PlayerIdNumber;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
        [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;

        private CustomNetworkManager _manager;
        public GameObject playerModel;
        private PlayerMovementController _playerMovementController;

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
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
            _playerMovementController = GetComponent<PlayerMovementController>();
            _playerMovementController.enabled = false;
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
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
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

        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SetInitialPosition();
            UpdatePlayerVisual();
            UpdatePlayerControls();
        }

        private void SetInitialPosition()
        {
            if (IsGameScene())
            {
                _playerMovementController.SetInitialPosition(new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-5, 5)));
            }
        }

        private void UpdatePlayerVisual()
        {
            playerModel.SetActive(IsGameScene());
        }

        private void UpdatePlayerControls()
        {
            if (hasAuthority && IsGameScene())
            {
                _playerMovementController.enabled = true;
            }
        }

        private bool IsGameScene()
        {
            return SceneManager.GetActiveScene().name == "Game";
        }
    }
}