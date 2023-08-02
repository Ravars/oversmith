using System;
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
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;

        [SerializeField] private VoidEventChannelSO sceneReady;
        [SerializeField] private GameSceneSO level1;
        
        private HelloWorldNetworkManager _manager;
        public HelloWorldNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null))
                {
                    return _manager;
                }

                return _manager = HelloWorldNetworkManager.singleton as HelloWorldNetworkManager;
            }
        }

        [SerializeField] private InputReader inputReader;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            inputReader.EnableGameplayInput();
            inputReader.MenuPauseEvent += InputReaderOnMenuPauseEvent;
            sceneReady.OnEventRaised += OnSceneReady;
        }

        private void OnSceneReady()
        {
            Debug.Log("scene loaded");
        }

        private void OnDisable()
        {
            inputReader.MenuPauseEvent -= InputReaderOnMenuPauseEvent;
        }

        private void InputReaderOnMenuPauseEvent()
        {
            if (!hasAuthority) return;
            SendHelloToServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log("Client started");
            Manager.players.Add(this);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            Debug.Log("Client stopped");
            Manager.players.Remove(this);
        }
        
        [ContextMenu("Hello")]
        public void SendHelloToServer()
        {
            CmdSendHelloToServer();
        }
        

        [Command]
        public void CmdSendHelloToServer()
        {
            Debug.Log("Hello");
            RpcServerHello();
        }

        [ClientRpc]
        public void RpcServerHello()
        {
            Debug.Log("Hello from server");
            // Manager.Opa(level1.level.ToString());   
            loadLocation.RaiseEvent(level1);
        }
        
        
    }
}
