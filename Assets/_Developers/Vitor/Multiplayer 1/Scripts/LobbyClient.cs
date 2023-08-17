using System;
using _Developers.Vitor.Multiplayer_1.Scripts.UI;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using UnityEngine;

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
        [SerializeField] private GameSceneSO level1;
        [SerializeField] private InputReader inputReader;

        [SerializeField] private GameObject canvas;
        [SerializeField] private UIMenuManager uiMenuManager;
        [Header("Listening on")]
        [SerializeField] private VoidEventChannelSO sceneReady;
        
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
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
            CmdSelect();
            
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

        // private void OnDisable()
        // {
        //     inputReader.MenuPauseEvent -= InputReaderOnMenuPauseEvent;
        // }

        // private void InputReaderOnMenuPauseEvent()
        // {
        //     if (!hasAuthority) return;
        //     SendHelloToServer();
        // }

        public override void OnStartClient()
        {
            Manager.lobbyPlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Manager.lobbyPlayers.Remove(this);
        }
        
        // public void SendHelloToServer()
        // {
        //     CmdSendHelloToServer();
        // }
        

        // [Command]
        // public void CmdSendHelloToServer()
        // {
        //     RpcServerHello();
        // }

        // [ClientRpc]
        // public void RpcServerHello()
        // {
        //     inputReader.DisableAllInput();
        //     loadLocation.RaiseEvent(level1);
        // }
        public void SetState(MenuState menuState)
        {
            uiMenuManager.SetState(menuState);
        }
    }
}
