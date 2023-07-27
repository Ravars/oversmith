using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class PlayerObjectController : NetworkBehaviour
    {
        [SyncVar] public int ConnectionID;
        [SyncVar] public int PlayerIdNumber;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar(hook = nameof(PlayerNameUpdate))] public string PlayerName;
        [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
        public GameObject playerModel;
        private CustomNetworkManager _manager;
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
        [Header("Listening on")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady = default;

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            playerModel.SetActive(false);
        }

        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += OnSceneLoaded;   
        }


        private void OnDisable()
        {
            _onSceneReady.OnEventRaised -= OnSceneLoaded;
        }
        private void OnSceneLoaded()
        {
            Debug.Log("Scene loaded");   
            if (!playerModel.activeSelf)
            {
                SetPosition();
                playerModel.SetActive(true);
            }
        }
        public void SetPosition()
        {
            transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-5, 5));
        }

        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority");
            CmdSetPlayerName(SteamFriends.GetPersonaName());
            gameObject.name = "LocalGamePlayer"; 
            LobbyController.Instance.FindLocalPlayer();
            LobbyController.Instance.UpdateLobbyName();
        }

        public override void OnStartClient()
        {
            Debug.Log("Client started");
            Manager.GamePlayers.Add(this);
            LobbyController.Instance.UpdateLobbyName();
            LobbyController.Instance.UpdatePlayerList();
        }

        public override void OnStopClient()
        {
            Manager.GamePlayers.Remove(this);
            if (LobbyController.InstanceExists)
            {
                LobbyController.Instance.UpdatePlayerList();
            }
            // SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }
        [Command]
        private void CmdSetPlayerName(string PlayerName)
        {
            Debug.Log("CmdSetPlayerName");
            this.PlayerNameUpdate(this.PlayerName,PlayerName);
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
    }
}