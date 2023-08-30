using System;
using _Developers.Vitor.Multiplayer_1.Scripts.UI;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Developers.Vitor.Multiplayer_1.Scripts
{
    public class LobbyClient : NetworkBehaviour
    {
        [SyncVar] public int ConnectionID;
        private HelloWorldNetworkManager _manager;
        private HelloWorldNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as HelloWorldNetworkManager;
            }
        }
        private bool _isLeader;
        public bool IsLeader
        {
            get => _isLeader;
            set
            {
                _isLeader = value;
                startGameButton.gameObject.SetActive(value);
            }
        }

        [SyncVar(hook = nameof(HandleDisplayNameChanged))]
        public string displayName = "Loading...";

        [SyncVar(hook = nameof(HandleReadyStatusChanged))]
        public bool isReady = false;
        
        

        [SerializeField] private GameSceneSO level1;
        [SerializeField] private InputReader inputReader;

        [SerializeField] private GameObject canvas;
        [SerializeField] private UIMenuManager uiMenuManager;

        [Header("UI")] 
        [SerializeField] private GameObject lobbyUI = null;

        [SerializeField] private TextMeshProUGUI[] playerNameTexts = new TextMeshProUGUI[4];
        [SerializeField] private TextMeshProUGUI[] playerReadyTexts = new TextMeshProUGUI[4];
        [SerializeField] private Button startGameButton = null;
        [SerializeField] private UIMenuManager2 menuManager2;
        [SerializeField] private GameObject menuManagerGameObject;
        
        
        
        
        [Header("Listening on")]
        [SerializeField] private VoidEventChannelSO sceneReady;
        
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            HelloWorldNetworkManager.OnClientDisconnected += UpdateUI;
            UpdateDisplay();
            
            if (!hasAuthority) return;
            menuManagerGameObject.SetActive(true);
            menuManager2.SetState(MenuState.MainMenu);
            // uiMenuManager.gameObject.SetActive(true);
            // uiMenuManager.SetState(MenuState.MainMenu);
        }

        private void OnDisable()
        {
            HelloWorldNetworkManager.OnClientDisconnected -= UpdateUI;
        }

        private void UpdateUI()
        {
            if (!hasAuthority) return;
            UpdateDisplay();
            Manager.NotifyPlayersOfReadyState();
        }

        /// <summary>
        /// Neste ponto deve ser ativado o input do menu
        /// </summary>
        public override void OnStartAuthority()
        {
            if (!hasAuthority) return;
            inputReader.EnableMenuInput();
            // inputReader.MenuUnpauseEvent += InputReaderOnMenuPauseEvent;
            sceneReady.OnEventRaised += OnSceneReady;
            // CmdSelect();
            CmdSetDisplayName(PlayerNameInput.DisplayName);
            lobbyUI.SetActive(true);
        }
        [Command]
        public void CmdSelect(NetworkConnectionToClient sender = null)
        {
            // GameObject characterInstance = Instantiate(canvas);
            // NetworkServer.Spawn(characterInstance, sender);
            // // characterInstance.gameObject.SetActive(false);
            // if (hasAuthority)
            // {
            //     characterInstance.gameObject.SetActive(true);
            // }
        }

        [Command]
        public void CmdChangeMenu()
        {
            Manager.UiSetState();
        }
        private void OnDestroy()
        {
            sceneReady.OnEventRaised -= OnSceneReady;
            // inputReader.MenuPauseEvent -= InputReaderOnMenuPauseEvent;
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
            Manager.ClientSceneReady();
        }
        public override void OnStartClient()
        {
            Manager.lobbyPlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Manager.lobbyPlayers.Remove(this);
        }
        
        public void SetState(MenuState menuState)
        {
            Debug.Log("Set State" + menuState);
            uiMenuManager.SetState(menuState);
        }
        public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateUI();

        public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateUI();

        private void UpdateDisplay()
        {
            if (!hasAuthority)
            {
                foreach (var lobbyPlayer in Manager.lobbyPlayers)
                {
                    if (!lobbyPlayer.hasAuthority) continue;
                    lobbyPlayer.UpdateDisplay();
                    break;
                }
                return;
            }

            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting for player...";
                playerReadyTexts[i].text = String.Empty;
            }

            for (int i = 0; i < Manager.lobbyPlayers.Count; i++)
            {
                playerNameTexts[i].text = Manager.lobbyPlayers[i].displayName;
                playerReadyTexts[i].text = Manager.lobbyPlayers[i].isReady ? "<color=green>Ready</color>" : "<color=red>Not ready</color>";
            }
        }
        
        public void HandleReadyToStart(bool readyToStart)
        {
            if (!IsLeader) return;
            startGameButton.interactable = readyToStart;
        }

        [Command]
        private void CmdSetDisplayName(string newDisplayName)
        {
            displayName = newDisplayName;
        }

        [Command]
        public void CmdReadyUp()
        {
            isReady = !isReady;
            Manager.NotifyPlayersOfReadyState();
        }

        [Command]
        public void CmdStartGame()
        {
            if (Manager.lobbyPlayers[0].connectionToClient != connectionToClient) return;
            CmdSendHelloToServer();
        }
        [Command]
        public void CmdSendHelloToServer()
        {
            RpcServerHello();
        }

        [ClientRpc]
        public void RpcServerHello()
        {
            Debug.Log("RpcServerHello");
            // inputReader.DisableAllInput();
            menuManager2.CmdSetMenuState(MenuState.CharacterSelection);
            // loadLocation.RaiseEvent(level1);
        }
    }
}
