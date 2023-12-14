using System;
using System.Collections;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.UI.SettingsScripts;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.UI
{
    public enum MenuRoomState
    {
        CharacterSelection,
        LevelSelection
    }
    public class UiRoomManager : Singleton<UiRoomManager>
    {
        [Header("UI Scripts")]
        [SerializeField] private UILevelSelection _levelSelectUI;
        [SerializeField] private LobbyControllerCanvas _lobbyControllerCanvas;
        
        [SerializeField] private InputReader _inputReader = default;
        private MenuRoomState _state;
        public MadSmithNetworkRoomPlayer LocalClient { get; private set; }

        [Header("NetworkManager")]
        private MadSmithNetworkRoomManager _manager;
        public MadSmithNetworkRoomManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkRoomManager;
            }
        }
        [Header("Cameras")]
        [SerializeField] private GameObject _mainMenuCamera;
        [SerializeField] private GameObject _levelSelectCamera;
        
        



        private void Start()
        {
            _inputReader.EnableMenuInput();
            Debug.Log("Start UiRoom");
            SetLevelSelection();
            SetCharacterController();

            SetState(MenuRoomState.CharacterSelection);
        }

        private void OnDisable()
        {
            UnsetLevelSelection();
            UnsetCharacterSelection();
        }
        public void SetState(MenuRoomState newState) // era private
        {
            CloseAll();
            _state = newState;

            switch (newState)
            {
                case MenuRoomState.CharacterSelection:
                    OpenCharacterSelection();
                    break;
                case MenuRoomState.LevelSelection:
                    OpenLevelSelection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }

        private void CloseAll()
        {
            CloseCharacterSelection();
            CloseLevelSelection();
        }
        public void SetLocalPlayer(MadSmithNetworkRoomPlayer localGamePlayer)
        {
            LocalClient = localGamePlayer;
        }

        #region Character Selection
        
        private void SetCharacterController()
        {
            _lobbyControllerCanvas.Setup(LocalClient);
            _lobbyControllerCanvas.Closed += () => { Debug.Log("You sure you want to quit?");}; // Confirm Exit
        }
        private void UnsetCharacterSelection()
        {
            _lobbyControllerCanvas.Closed -= () => { Debug.Log("You sure you want to quit?");}; // Confirm Exit
        }
        private void OpenCharacterSelection()
        {
            _mainMenuCamera.SetActive(true);
            _lobbyControllerCanvas.gameObject.SetActive(true);
        }
        private void CloseCharacterSelection()
        {
            _mainMenuCamera.SetActive(false);
            _lobbyControllerCanvas.gameObject.SetActive(false);
        }
        #endregion

        #region Level Selection
        private void SetLevelSelection()
        {
            _levelSelectUI.Setup(LocalClient);
            _levelSelectUI.OnCloseLevelSelection += () => { SetState(MenuRoomState.CharacterSelection); };
        }
        private void UnsetLevelSelection()
        {
            _levelSelectUI.OnCloseLevelSelection -= () => { SetState(MenuRoomState.CharacterSelection); };
        }

        private void OpenLevelSelection()
        {
            _levelSelectCamera.SetActive(true);
            _levelSelectUI.gameObject.SetActive(true);
        }
        private void CloseLevelSelection()
        {
            _levelSelectCamera.SetActive(false);
            _levelSelectUI.gameObject.SetActive(false);
            
        }
        #endregion

    }
}