using System;
using System.Collections;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.UI.Canvas;
using MadSmith.Scripts.UI.SettingsScripts;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.UI.Managers
{
    public enum MenuState
    {
        MainMenu,
        Settings,
        Credits,
        Tutorial,
        CharacterSelection,
        LevelSelection,
        Host,
        Join,
        Lobby,
        LobbiesList
    }
    public class UIMenuManager : Singleton<UIMenuManager>
    {
        public MenuState State { get; private set; }
        private bool _hasSaveData;
        [SerializeField] private SaveSystem _saveSystem = default;
        [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private GameDataSO currentGameData;
        [SerializeField] private GameSceneSO _locationToNewGame;
        
        [Header("UI Scripts")]
        [SerializeField] private UIMainMenu _mainMenuPanel = default;
        [SerializeField] private UISettingsController _settingsPanel = default;
        [SerializeField] private UICredits _creditsPanel = default;
        [SerializeField] private UIPopup _popupPanel = default;
        [SerializeField] private CharacterSelect _characterSelectUI;
        [SerializeField] private UILevelSelection _levelSelectUI;
        [SerializeField] private UITutorial uiTutorial;
        [SerializeField] private UIHost uiHostPanel;
        [SerializeField] private LobbyController uiLobbyControllerPanel;
        [SerializeField] private UIJoin uiJoinPanel;
        [SerializeField] private UILobbyList uiLobbiesListPanel;
        
        [Header("Cameras")]
        [SerializeField] private GameObject _mainMenuCamera;
        [SerializeField] private GameObject _characterSelectCamera;
        [SerializeField] private GameObject _creditsCamera;
        [SerializeField] private GameObject _tutorialCamera;
        [SerializeField] private GameObject _levelSelectCamera;
        
        [Header("NetworkManager")]
        private MadSmithNetworkManager _manager;
        public MadSmithNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }
        
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO _loadLocation = default;
        private IEnumerator Start()
        {
            _inputReader.EnableMenuInput(); //TODO active this
            yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded
            SetMenuScreen();
            SetSettingsScreen();
            SetCreditsScreen();
            SetCharacterSelectScreen();
            SetLevelSelectScreen();
            SetTutorialScreen();
            SetHostScreen();
            SetJoinScreen();
            SetLobbyScreen();
            SetLobbiesListScreen();
            SetState(MenuState.MainMenu);
            // _loadLocation.OnLoadingRequested += (_, _, _) => { CloseAll();};
        }

        private void OnDisable()
        {
            Debug.Log("Disable");
            // _loadLocation.OnLoadingRequested -= (_, _, _) => { CloseAll();};
            UnsetMenuScreen();
            UnsetSettingsScreen();
            UnsetCreditsScreen();
            
            UnsetCharacterSelectScreen();
            UnsetLevelSelectScreen();
            UnsetTutorialScreen();
            
            UnsetHostScreen();
            UnsetJoinScreen();
            UnsetLobbyScreen();
            UnsetLobbiesListScreen();
        }

        private void SetState(MenuState newState)
        {
            Debug.Log(newState);
            CloseAll();
            State = newState;
            
            //Enter State
            switch (newState)
            {
                case MenuState.MainMenu:
                    OpenMenuScreen();
                    break;
                case MenuState.Settings:
                    OpenSettingsScreen();
                    break;
                case MenuState.Credits:
                    OpenCreditsScreen();
                    break;
                case MenuState.Tutorial:
                    OpenTutorialScreen();
                    break;
                case MenuState.CharacterSelection:
                    OpenCharacterSelect();
                    break;
                case MenuState.LevelSelection:
                    OpenLevelSelect();
                    break;
                case MenuState.Host:
                    OpenHost();
                    break;
                case MenuState.Join:
                    OpenJoin();
                    break;
                case MenuState.Lobby:
                    OpenLobby();
                    break;
                case MenuState.LobbiesList:
                    OpenLobbiesList();
                    break;
                default:
                    Debug.Log("Nothing");
                    break;
            }
        }
        #region Main Menu
        private void SetMenuScreen()
        {
            _hasSaveData = _saveSystem.Loaded;
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
            _mainMenuPanel.HostGameButtonAction += () => SetState(MenuState.Host);
            _mainMenuPanel.JoinLobbyButtonAction += () => SetState(MenuState.Join);
            _mainMenuPanel.SettingsButtonAction += () => SetState(MenuState.Settings);
            _mainMenuPanel.CreditsButtonAction += () => SetState(MenuState.Credits);
            _mainMenuPanel.TutorialButtonAction += () => SetState(MenuState.Tutorial);
            _mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
        }
        private void UnsetMenuScreen()
        {
            _mainMenuPanel.HostGameButtonAction -= () => SetState(MenuState.Host);
            _mainMenuPanel.JoinLobbyButtonAction -= () => SetState(MenuState.Join);
            _mainMenuPanel.SettingsButtonAction -= () => SetState(MenuState.Settings);
            _mainMenuPanel.CreditsButtonAction -= () => SetState(MenuState.Credits);
            _mainMenuPanel.TutorialButtonAction -= () => SetState(MenuState.Tutorial);
            _mainMenuPanel.ExitButtonAction -= ShowExitConfirmationPopup;
        }
        private void OpenMenuScreen()
        {
            _mainMenuCamera.SetActive(true);
            _mainMenuPanel.gameObject.SetActive(true);
        }
        private void CloseMenuScreen()
        {
            _mainMenuCamera.SetActive(false);
            _mainMenuPanel.gameObject.SetActive(false);
        }
        #endregion

        #region Settings

        private void SetSettingsScreen()
        {
            _settingsPanel.Closed += () => SetState(MenuState.MainMenu);
        }

        private void UnsetSettingsScreen()
        {
            _settingsPanel.Closed -= () => SetState(MenuState.MainMenu);
        }

        private void OpenSettingsScreen()
        {
            _mainMenuCamera.SetActive(true);
            _settingsPanel.gameObject.SetActive(true);
        }

        private void CloseSettingsScreen()
        {
            _settingsPanel.Closed -= () => SetState(MenuState.MainMenu);
            _mainMenuCamera.SetActive(false);
            _settingsPanel.gameObject.SetActive(false);
        }
        #endregion

        #region Credits

        private void SetCreditsScreen()
        {
            _creditsPanel.OnCloseCredits += () => SetState(MenuState.MainMenu);
        }
        private void UnsetCreditsScreen()
        {
            _creditsPanel.OnCloseCredits -= () => SetState(MenuState.MainMenu);
        }

        private void OpenCreditsScreen()
        {
            _creditsCamera.SetActive(true);
            _creditsPanel.gameObject.SetActive(true);
        }

        private void CloseCreditsScreen()
        {
            _creditsCamera.SetActive(false);
            _creditsPanel.gameObject.SetActive(false);
        }
        #endregion

        #region Character Selection

        private void SetCharacterSelectScreen()
        {
            _characterSelectUI.Setup();
            _characterSelectUI.OnCharacterSelected += OnCharacterSelected;
            _characterSelectUI.OnCloseCharacterSelection += () => SetState(MenuState.MainMenu);
        }
        private void UnsetCharacterSelectScreen()
        {
            _characterSelectUI.OnCharacterSelected -= OnCharacterSelected;
            _characterSelectUI.OnCloseCharacterSelection -= () => SetState(MenuState.MainMenu);
        }

        private void OnCharacterSelected()
        {
            if (currentGameData.LevelScores.Count > 0)
            {
                SetState(MenuState.LevelSelection);
            }
            else
            {
                _loadLocation.RaiseEvent(_locationToNewGame, true, true);
            }
        }

        private void OpenCharacterSelect()
        {
            _characterSelectCamera.SetActive(true);
            _characterSelectUI.gameObject.SetActive(true);
        }

        public void CloseCharacterSelect()
        {
            _characterSelectUI.gameObject.SetActive(false);
            _characterSelectCamera.SetActive(false);
        }
        #endregion

        #region Tutorial

        private void SetTutorialScreen()
        {
            uiTutorial.OnCloseTutorial += () => SetState(MenuState.MainMenu);
        }

        private void UnsetTutorialScreen()
        {
            uiTutorial.OnCloseTutorial -= () => SetState(MenuState.MainMenu);
        }

        private void OpenTutorialScreen()
        {
            uiTutorial.gameObject.SetActive(true);
            _tutorialCamera.SetActive(true);
        }

        private void CloseTutorialScreen()
        {
            uiTutorial.gameObject.SetActive(false);
            _tutorialCamera.SetActive(false);
        }
        #endregion

        #region Level Selection

        private void SetLevelSelectScreen()
        {
            _levelSelectUI.OnCloseLevelSelection += () => SetState(MenuState.CharacterSelection);
            _levelSelectUI.OnLevelSelected += CloseAll;
        }

        private void UnsetLevelSelectScreen()
        {
            _levelSelectUI.OnCloseLevelSelection -= () => SetState(MenuState.CharacterSelection);
            _levelSelectUI.OnLevelSelected -= CloseAll;
        }

        private void OpenLevelSelect()
        {
            _levelSelectUI.gameObject.SetActive(true);
            _levelSelectCamera.SetActive(true);
        }

        private void CloseLevelSelect()
        {
            _levelSelectUI.gameObject.SetActive(false);
            _levelSelectCamera.SetActive(false);
        }
        #endregion

        #region Quit game Popup
        private void ShowExitConfirmationPopup()
        {
            _popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
            _popupPanel.gameObject.SetActive(true);
            _popupPanel.SetPopup(PopupType.Quit);
        }
        void HideExitConfirmationPopup(bool quitConfirmed)
        {
            _popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
            _popupPanel.gameObject.SetActive(false);
            if (quitConfirmed)
            {
                Application.Quit();
            }
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }
        #endregion
        
        #region Host
        private void SetHostScreen()
        {
            uiHostPanel.SetUIHost(Manager.SteamIsOpen());
            uiHostPanel.SteamHostButtonAction += () =>
            {
                Manager.HostBySteam();
                SetState(MenuState.Lobby);
            };
            uiHostPanel.LocalHostButtonAction += () =>
            {
                Manager.HostByLocalHost();
                SetState(MenuState.Lobby);
            };
            uiHostPanel.Closed += () => SetState(MenuState.MainMenu);
        }
        private void UnsetHostScreen()
        {
            uiHostPanel.SteamHostButtonAction -= () =>
            {
                Manager.HostBySteam();
                SetState(MenuState.Lobby);
            };
            uiHostPanel.LocalHostButtonAction -= () =>
            {
                Manager.HostByLocalHost();
                SetState(MenuState.Lobby);
            };
            uiHostPanel.Closed -= () => SetState(MenuState.MainMenu);
        }
        private void OpenHost()
        {
            _mainMenuCamera.SetActive(true);
            uiHostPanel.gameObject.SetActive(true);
        }
        private void CloseHost()
        {
            _mainMenuCamera.SetActive(false);
            uiHostPanel.gameObject.SetActive(false);
        }
        #endregion
        
        #region Join
        private void SetJoinScreen()
        {
            uiJoinPanel.SetJoinHost();
            uiJoinPanel.Closed += () => SetState(MenuState.MainMenu);
            uiJoinPanel.SteamJoinButtonAction += () =>
            {
                Manager.JoinBySteam();
                // SetState(MenuState.Lobby);
            };
        }
        private void UnsetJoinScreen()
        {
            uiJoinPanel.Closed -= () => SetState(MenuState.MainMenu);
            uiJoinPanel.SteamJoinButtonAction -= () =>
            {
                Manager.JoinBySteam();
                // SetState(MenuState.Lobby);
            };
        }
        private void OpenJoin()
        {
            _mainMenuCamera.SetActive(true);
            uiJoinPanel.gameObject.SetActive(true);
        }
        private void CloseJoin()
        {
            _mainMenuCamera.SetActive(false);
            uiJoinPanel.gameObject.SetActive(false);
        }
        #endregion
        
        #region Lobby
        private void SetLobbyScreen()
        {
            // uiLobbyControllerPanel
            uiLobbyControllerPanel.Closed += () =>
            {
                Manager.StopHostOrClientOnLobbyMenu();
                SetState(MenuState.MainMenu);
            };
            uiLobbyControllerPanel.NextPage += () => { SetState(MenuState.CharacterSelection); };
            Manager.SteamLobby.OnLobbyEnteredEvent += () => SetState(MenuState.Lobby);
        }
        private void UnsetLobbyScreen()
        {
            if (ReferenceEquals(uiLobbyControllerPanel, null)) return;
            uiLobbyControllerPanel.Closed -= () =>
            {
                Manager.StopHostOrClientOnLobbyMenu();
                SetState(MenuState.MainMenu);
            };
            uiLobbyControllerPanel.NextPage -= () => { SetState(MenuState.CharacterSelection); };
            Manager.SteamLobby.OnLobbyEnteredEvent -= () => SetState(MenuState.Lobby);
        }
        private void OpenLobby()
        {
            _mainMenuCamera.SetActive(true);
            uiLobbyControllerPanel.gameObject.SetActive(true);
        }
        private void CloseLobby()
        {
            _mainMenuCamera.SetActive(false);
            uiLobbyControllerPanel.gameObject.SetActive(false);
        }
        #endregion
        
        #region Lobbies List
        private void SetLobbiesListScreen()
        {
            SteamLobby.Instance.OnLobbyListRequestedEvent += () => SetState(MenuState.LobbiesList);
            uiLobbiesListPanel.Closed += () =>
            {
                LobbiesListManager.Instance.DestroyLobbies();
                SetState(MenuState.MainMenu);
            };
        }
        private void UnsetLobbiesListScreen()
        {
            if (SteamLobby.InstanceExists)
            {
                SteamLobby.Instance.OnLobbyListRequestedEvent -= () => SetState(MenuState.LobbiesList);
            }
            uiLobbiesListPanel.Closed -= () =>
            {
                LobbiesListManager.Instance.DestroyLobbies();
                SetState(MenuState.MainMenu);
            };
        }
        private void OpenLobbiesList()
        {
            _mainMenuCamera.SetActive(true);
            uiLobbiesListPanel.gameObject.SetActive(true);
        }
        private void CloseLobbiesList()
        {
            _mainMenuCamera.SetActive(false);
            uiLobbiesListPanel.gameObject.SetActive(false);
        }
        #endregion

        protected override void OnDestroy()
        {
            Debug.Log("OnDestroy");
            _popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
            base.OnDestroy();
        }

        private void CloseAll()
        {
            CloseMenuScreen();
            CloseSettingsScreen();
            CloseCreditsScreen();
            CloseTutorialScreen();
            CloseCharacterSelect();
            CloseLevelSelect();
            CloseHost();
        }
    }
}